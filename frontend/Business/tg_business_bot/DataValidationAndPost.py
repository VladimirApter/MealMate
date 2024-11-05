import re
import pydantic_core
from telebot import types

import Geocoder
from Config import *
from ApiClient import ApiClient
from Models.Restaurant import Restaurant
from Models.Menu import Menu
from Models.Owner import Owner
from Models.NotificationGetter import NotificationGetter
from excel_tables_work.parse_table import parse_menu_from_excel
from excel_tables_work.validate_menu_template import *


INCORRECT_EXCEL_TABLE_MESSAGE = "Файл заполнен некорректно. Попробуйте еще " \
                                "раз заполнить шаблон, который я высылал вам " \
                                "ранее, и отправьте корректную версию файла"


def _get_yes_no_markup():
    markup = types.ReplyKeyboardMarkup(resize_keyboard=True)
    markup.add(*[types.KeyboardButton('да'), types.KeyboardButton('нет')])
    return markup


def validate_and_post_restaurant_name(message: types.Message, restaurant: Restaurant, func_to_return_after_post=None, is_registration=False):
    name = message.text.strip()

    api_client = ApiClient(Owner)
    owner = api_client.get(message.chat.id)
    restaurant_ids = owner.restaurant_ids
    if restaurant_ids is not None:
        api_client = ApiClient(Restaurant)
        restaurants = [api_client.get(rest_id) for rest_id in restaurant_ids]

        if name in [rest.name for rest in restaurants]:
            bot.send_message(message.chat.id, 'Ресторан с таким названием у Вас уже подключен, введите другое')
            bot.register_next_step_handler(message, validate_and_post_restaurant_name, restaurant, func_to_return_after_post, is_registration)
            return
    if name == '':
        bot.send_message(message.chat.id, 'Название ресторана должно быть не пустым, введите другое')
        bot.register_next_step_handler(message, validate_and_post_restaurant_name, restaurant, func_to_return_after_post, is_registration)
        return

    restaurant.name = name
    if not is_registration:
        api_client = ApiClient(Restaurant)
        api_client.post(restaurant)

    bot.send_message(message.chat.id, f'Название ресторана сохранено')

    if func_to_return_after_post is not None:
        func_to_return_after_post(message, restaurant)
        return


def validate_and_post_address(message: types.Message, restaurant: Restaurant, func_to_return_after_post=None, is_registration=False):
    address = message.text
    parsed_coordinates = _parse_coordinates(address)
    if parsed_coordinates is not None:
        coordinates = parsed_coordinates
    else:
        coordinates = Geocoder.get_coordinates(address)

    if coordinates is None:
        text = 'Не можем найти этот адрес, попробуйте записать адрес в более ' \
               'подробной форме или отправьте координаты, их можно найти на ' \
               'Яндекс картах'

        formatted_text = text.replace("Яндекс картах", '<a href="https://yandex.ru/maps">Яндекс картах</a>')
        bot.send_message(message.chat.id, formatted_text, parse_mode='HTML', disable_web_page_preview=True)

        bot.register_next_step_handler(message, validate_and_post_address, restaurant, func_to_return_after_post, is_registration)
        return

    latitude, longitude = coordinates
    bot.send_location(message.chat.id, latitude, longitude)

    bot.send_message(message.chat.id, "Ваш ресторан находится здесь, верно?", reply_markup=_get_yes_no_markup())
    bot.register_next_step_handler(message, _accept_address, restaurant, coordinates, func_to_return_after_post, is_registration)


def _parse_coordinates(coord_str):
    pattern = r'^-?\d{1,3}\.\d+,\s*-?\d{1,3}\.\d+$'
    if re.match(pattern, coord_str):
        lat, lon = coord_str.split(',')
        return float(lat.strip()), float(lon.strip())
    else:
        return None


def _accept_address(message: types.Message, restaurant: Restaurant, coordinates, func_to_return_after_post, is_registration):
    if message.text == 'да':
        restaurant.address = str(coordinates)
        if not is_registration:
            api_client = ApiClient(Restaurant)
            api_client.post(restaurant)

        bot.send_message(message.chat.id, f'Адрес ресторана сохранен', reply_markup=types.ReplyKeyboardRemove())

        if func_to_return_after_post is not None:
            func_to_return_after_post(message, restaurant)
            return

    elif message.text == 'нет':
        text = 'Тогда попробуйте записать адрес в более ' \
               'подробной форме или отправьте координаты, их можно найти на ' \
               'Яндекс картах'

        formatted_text = text.replace("Яндекс картах", '<a href="https://yandex.ru/maps">Яндекс картах</a>')
        bot.send_message(message.chat.id, formatted_text, parse_mode='HTML', disable_web_page_preview=True, reply_markup=types.ReplyKeyboardRemove())

        bot.register_next_step_handler(message, validate_and_post_address, restaurant, func_to_return_after_post, is_registration)

    else:
        bot.send_message(message.chat.id, 'Для выбора нажмите на одну из кнопок ниже')
        bot.register_next_step_handler(message, _accept_address, restaurant, coordinates, func_to_return_after_post, is_registration)


def validate_and_post_notification_getter(message: types.Message, restaurant: Restaurant, func_to_return_after_post=None, is_registration=False):
    if message.text == 'назначить себя':
        new_notification_getter = NotificationGetter(restaurant_id=restaurant.id, id=message.chat.id, username=message.from_user.username)
    elif message.users_shared is not None:
        user = message.users_shared.users[0]
        username = user.username if user.username is not None else ''
        new_notification_getter = NotificationGetter(restaurant_id=restaurant.id, id=user.user_id, username=username)
    else:
        bot.send_message(message.chat.id, 'Для выбора нажмите на одну из кнопок ниже')
        bot.register_next_step_handler(message, validate_and_post_notification_getter, restaurant, func_to_return_after_post, is_registration)
        return

    if restaurant.notification_getter is None or restaurant.notification_getter.id != new_notification_getter.id:
        restaurant.notification_getter = new_notification_getter

    if not is_registration:
        api_client = ApiClient(NotificationGetter)
        api_client.post(new_notification_getter)

    bot.send_message(message.chat.id, f'Получатель уведомлений сохранен', reply_markup=types.ReplyKeyboardRemove())

    if func_to_return_after_post is not None:
        func_to_return_after_post(message, restaurant)
        return


def validate_and_post_menu(message: types.Message, restaurant: Restaurant, func_to_return_after_post=None, is_registration=False):
    if not message.document:
        bot.send_message(message.chat.id, 'Заполните и отправьте документ, который я высылал вам ранее')
        bot.register_next_step_handler(message, validate_and_post_menu, restaurant, func_to_return_after_post, is_registration)
        return
    elif message.document.mime_type != 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet':
        bot.reply_to(message, 'Кажется это не эксель документ, заполните и отправьте документ, который я высылал вам ранее')
        bot.register_next_step_handler(message, validate_and_post_menu, restaurant, func_to_return_after_post, is_registration)
        return

    file_info = bot.get_file(message.document.file_id)
    downloaded_file = bot.download_file(file_info.file_path)

    temp_excel_file_path = os.path.join(current_dir, "excel_tables_work", f'{message.chat.id}_temp.xlsx')
    with open(temp_excel_file_path, 'wb') as new_file:
        new_file.write(downloaded_file)

    try:
        valid, invalid_cells_dishes, invalid_cells_drinks = validate_menu_template(temp_excel_file_path)
    except:
        bot.send_message(message.chat.id, "Пожалуйста, заполните именно тот шаблон, который я высылал вам ранее. Я могу обработать только его", reply_markup=types.ReplyKeyboardRemove())
        bot.register_next_step_handler(message, validate_and_post_menu, restaurant, func_to_return_after_post, is_registration)
        os.remove(temp_excel_file_path)
        return

    if not valid:
        text = get_validation_error_messages(invalid_cells_dishes, invalid_cells_drinks)
        bot.send_message(message.chat.id, f"{text}\n\nПожалуйста, исправьте ошибки и отправьте корректную версию файла",
                         reply_markup=types.ReplyKeyboardRemove())
        bot.register_next_step_handler(message, validate_and_post_menu, restaurant, func_to_return_after_post, is_registration)
        os.remove(temp_excel_file_path)
        return

    menu = Menu(restaurant_id=restaurant.id)
    try:
        menu = parse_menu_from_excel(temp_excel_file_path, menu)
    except pydantic_core._pydantic_core.ValidationError:
        bot.send_message(message.chat.id, INCORRECT_EXCEL_TABLE_MESSAGE, reply_markup=types.ReplyKeyboardRemove())
        bot.register_next_step_handler(message, validate_and_post_menu, restaurant, func_to_return_after_post, is_registration)
        os.remove(temp_excel_file_path)
        return

    os.remove(temp_excel_file_path)

    restaurant.menu = menu

    if not is_registration:
        api_client = ApiClient(Menu)
        api_client.post(menu)

    bot.send_message(message.chat.id, "Меню сохранено", reply_markup=types.ReplyKeyboardRemove())

    if func_to_return_after_post is not None:
        func_to_return_after_post(message, restaurant)
        return
