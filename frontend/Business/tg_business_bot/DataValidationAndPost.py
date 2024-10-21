import re
from telebot import types
import asyncio
from telethon import TelegramClient

import Geocoder
from Config import *
from ApiClient import ApiClient
from Models.Restaurant import Restaurant
from Models.Owner import Owner
from Models.NotificationGetter import NotificationGetter


def _get_yes_no_markup():
    markup = types.ReplyKeyboardMarkup()
    markup.add(*[types.KeyboardButton('да'), types.KeyboardButton('нет')])
    return markup


def validate_and_post_restaurant_name(message: types.Message, restaurant: Restaurant):
    api_client = ApiClient(Owner)
    owner = api_client.get(message.chat.id)

    restaurant_ids = owner.restaurant_ids
    api_client = ApiClient(Restaurant)
    restaurants = [api_client.get(rest_id) for rest_id in restaurant_ids]

    name = message.text
    if name in [rest.name for rest in restaurants]:
        bot.send_message('Ресторан с таким именем у Вас уже подключен, введите другое имя')
        bot.register_next_step_handler(message.chat.id, validate_and_post_restaurant_name, restaurant)
        return

    restaurant.name = name
    api_client = ApiClient(Restaurant)
    api_client.post(restaurant)

    bot.send_message(message.chat.id, f'Имя ресторана изменено на: {restaurant.name}')


def validate_and_post_address(message: types.Message, restaurant: Restaurant):
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

        bot.register_next_step_handler(message, validate_and_post_address, restaurant)
        return

    latitude, longitude = coordinates
    bot.send_location(message.chat.id, latitude, longitude)

    bot.send_message(message.chat.id, "Ваш ресторан находится здесь, верно?", reply_markup=_get_yes_no_markup())
    bot.register_next_step_handler(message, _accept_address, restaurant, coordinates)


def _parse_coordinates(coord_str):
    pattern = r'^-?\d{1,3}\.\d+,\s*-?\d{1,3}\.\d+$'
    if re.match(pattern, coord_str):
        lat, lon = coord_str.split(',')
        return float(lat.strip()), float(lon.strip())
    else:
        return None


def _accept_address(message: types.Message, restaurant: Restaurant, coordinates):
    if message.text == 'да':
        restaurant.address = str(coordinates)
        api_client = ApiClient(Restaurant)
        api_client.post(restaurant)

        bot.send_message(message.chat.id, f'Адрес ресторана сохранен')
    elif message.text == 'нет':
        text = 'Тогда попробуйте записать адрес в более ' \
               'подробной форме или отправьте координаты, их можно найти на ' \
               'Яндекс картах'

        formatted_text = text.replace("Яндекс картах", '<a href="https://yandex.ru/maps">Яндекс картах</a>')
        bot.send_message(message.chat.id, formatted_text, parse_mode='HTML', disable_web_page_preview=True)

        bot.register_next_step_handler(message, validate_and_post_address(message, restaurant))
    else:
        bot.send_message(message.chat.id, 'Для выбора нажмите на одну из кнопок ниже')
        bot.register_next_step_handler(message, _accept_address, restaurant, coordinates)


def validate_and_post_notification_getter(message: types.Message, restaurant: Restaurant):
    username = message.text.strip()
    if not username.startswith('@'):
        bot.send_message(message.chat.id, 'Имя пользователя должно начинаться с @, попробуйте еще раз')
        bot.register_next_step_handler(message, validate_and_post_notification_getter, restaurant)
        return
    elif len(username) > 32:
        bot.send_message(message.chat.id, 'Имя пользователя должно быть короче 32 символов, попробуйте еще раз')
        bot.register_next_step_handler(message, validate_and_post_notification_getter, restaurant)
        return

    new_notification_getter = NotificationGetter()
    new_notification_getter.tg_id = None
    new_notification_getter.username = username
    new_notification_getter.restaurant_id = restaurant.id

    api_client = ApiClient(NotificationGetter)
    api_client.post(new_notification_getter)

    bot.send_message(message.chat.id, f'Получатель уведомлений изменен на {restaurant.notification_getter.username}')

