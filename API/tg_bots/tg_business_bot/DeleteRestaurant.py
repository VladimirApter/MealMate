from telebot import types

from ApiClient.ApiClient import ApiClient
from Config import *
from Model.Restaurant import Restaurant


def delete_restaurant(message, restaurant: Restaurant):
    if message.text == 'да':
        api_client = ApiClient(Restaurant)
        api_client.delete(restaurant.id)
        bot.send_message(message.chat.id, "Ресторан удален", reply_markup=types.ReplyKeyboardRemove())
    elif message.text == 'нет':
        bot.send_message(message.chat.id, "Ок, удаление отменено", reply_markup=types.ReplyKeyboardRemove())
    else:
        bot.send_message(message.chat.id, 'Для выбора нажмите на одну из кнопок ниже')
        bot.register_next_step_handler(message, delete_restaurant, restaurant)
