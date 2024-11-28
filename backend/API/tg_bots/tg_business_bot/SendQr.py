from telebot import types

from Config import *
from Model.Restaurant import Restaurant


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

    send_qrs(message, restaurant)


def send_qrs(message: types.Message, restaurant: Restaurant):
    for table in restaurant.tables:
        qr_code_image_path = os.path.join(qr_images_dir, table.qr_code_image_path)
        with open(qr_code_image_path, 'rb') as qr_image:
            bot.send_document(message.chat.id, qr_image, caption=f"#qr код стола номер {table.number}", visible_file_name=f"стол_{table.number}_qr.png")
