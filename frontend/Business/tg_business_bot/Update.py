import os
from time import time
from telebot.types import ReplyKeyboardMarkup, KeyboardButton, \
    KeyboardButtonRequestUsers

from DataValidationAndPost import *
from excel_tables_work.fill_menu_template import *


@bot.message_handler(commands=['update'])
def update_start(message: types.Message):
    api_client = ApiClient(Owner)
    owner = api_client.get(message.chat.id)

    restaurant_ids = owner.restaurant_ids

    if len(restaurant_ids) == 0:
        bot.send_message(message.chat.id, "У вас пока нет ресторанов, создайте первый при помощи /new_rest")
        return
    elif len(restaurant_ids) == 1:
        api_client = ApiClient(Restaurant)
        restaurant = api_client.get(restaurant_ids[0])

        markup = get_update_parts_markup()
        bot.send_message(message.chat.id, f'Что вы хотели бы изменить в ресторане {restaurant.name}?', reply_markup=markup)
        bot.register_next_step_handler(message, get_update_part, restaurant)
        return
    elif len(restaurant_ids) > 1:
        api_client = ApiClient(Restaurant)
        restaurants = [api_client.get(rest_id) for rest_id in restaurant_ids]

        markup = get_restaurants_markup(restaurants)
        bot.send_message(message.chat.id, 'Выберите ресторан', reply_markup=markup)
        bot.register_next_step_handler(message, get_restaurant, restaurants)


def get_restaurant(message: types.Message, restaurants):
    if not message.text:
        bot.send_message(message.chat.id, 'Чтобы выбрать ресторан нажмите на одну из кнопок ниже')
        bot.register_next_step_handler(message, get_restaurant, restaurants)
        return

    restaurant = None
    user_rest_name = message.text.strip()
    for rest in restaurants:
        if rest.name == user_rest_name:
            restaurant = rest

    if restaurant is None:
        bot.send_message(message.chat.id, 'Чтобы выбрать ресторан нажмите на одну из кнопок ниже')
        bot.register_next_step_handler(message, get_restaurant, restaurants)
        return

    markup = get_update_parts_markup()
    bot.send_message(message.chat.id, 'Что вы хотели бы изменить?', reply_markup=markup)
    bot.register_next_step_handler(message, get_update_part, restaurant)


def get_update_part(message: types.Message, restaurant: Restaurant):
    if not message.text:
        bot.send_message(message.chat.id, 'Для выбора части, которую вы хотели бы изменить, нажмите на одну из кнопок ниже')
        bot.register_next_step_handler(message, get_update_part, restaurant)
        return

    part = message.text.strip()

    if part == 'адрес':
        bot.send_message(message.chat.id, 'Введите адрес ресторана')
        bot.register_next_step_handler(message, validate_and_post_address, restaurant)
        return
    elif part == 'меню':
        menu_template_path = os.path.join(current_dir, "excel_tables_work", "menu_template.xlsx")
        filled_menu_temp_path = os.path.join(f'excel_tables_work', f'{hash(message.chat.id) + hash(time())}.xlsx')
        fill_menu_template(menu_template_path, filled_menu_temp_path, restaurant.menu)
        with open(filled_menu_temp_path, 'rb') as file:
            bot.send_document(message.chat.id, file,
                              caption="В этом файле текущая версия вашего "
                                      "меню. Отредактируйте его и отправьте "
                                      "мне новую версию")
        os.remove(filled_menu_temp_path)
        bot.register_next_step_handler(message, validate_and_post_menu, restaurant)
        return
    elif part == 'столы':
        pass
    elif part == 'название':
        bot.send_message(message.chat.id, 'Введите название ресторана')
        bot.register_next_step_handler(message, validate_and_post_restaurant_name, restaurant)
        return
    elif part == 'получатель уведомлений':
        bot.send_message(message.chat.id, 'Выберите получателя уведомлений', reply_markup=get_notification_getter_markup())
        bot.register_next_step_handler(message, validate_and_post_notification_getter, restaurant)
        return
    else:
        bot.send_message(message.chat.id, 'Для выбора части, которую вы хотели бы изменить, нажмите на одну из кнопок ниже')
        bot.register_next_step_handler(message, get_update_part, restaurant)
        return


def get_restaurants_markup(restaurants):
    markup = types.ReplyKeyboardMarkup(resize_keyboard=True, one_time_keyboard=True)
    row = []
    for rest in restaurants:
        row.append(types.KeyboardButton(rest.name))
        if len(row) == 2:
            markup.add(*row)
            row = []
    if row:
        markup.add(*row)
    return markup


def get_notification_getter_markup():
    markup = ReplyKeyboardMarkup(resize_keyboard=True, one_time_keyboard=True)

    owner_self_button = KeyboardButton(text='назначить себя')
    request_user_button = KeyboardButton(
        text='выбрать человека',
        request_users=KeyboardButtonRequestUsers(
            request_id=1,
            user_is_bot=False,
            request_username=True,
            max_quantity=1
        )
    )

    markup.add(*[owner_self_button, request_user_button])
    return markup


def get_update_parts_markup():
    markup = types.ReplyKeyboardMarkup(resize_keyboard=True, one_time_keyboard=True)

    rows = [
        [types.KeyboardButton('адрес'), types.KeyboardButton('меню')],
        [types.KeyboardButton('столы'), types.KeyboardButton('название')],
        [types.KeyboardButton('получатель уведомлений')]
    ]

    for row in rows:
        markup.add(*row)

    return markup

