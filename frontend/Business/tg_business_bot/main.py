import telebot
import re

from Models.Menu import *
from Models.Dish import *
from ApiClient import ApiClient

menu_api_client = ApiClient(Menu)

bot_token = "7449757271:AAF5uM4dBn9PHJ-ka5jEIp00Mt5-TR9Wqds"
bot = telebot.TeleBot(bot_token)


# Обработчик команды /start
@bot.message_handler(commands=['start'])
def start(message):
    bot.send_message(message.chat.id, 'Привет! Отправьте блюда в формате: "(Название блюда) (Цена блюда) (Описание блюда) (Вес блюда)". Для завершения введите /stop.')


# Обработчик команды /stop
@bot.message_handler(commands=['stop'])
def stop(message):
    bot.send_message(message.chat.id, 'Сбор меню завершен.')
    menu = menu_api_client.get()
    for dish in menu.dish_list:
        bot.send_message(message.chat.id, f'Блюдо: {dish.name}, Цена: {dish.price}, Описание: {dish.description}, Вес: {dish.weight}')


# Обработчик текстовых сообщений
@bot.message_handler(func=lambda message: True)
def handle_message(message):
    text = message.text
    pattern = r'\((.*?)\)\s*\((.*?)\)\s*\((.*?)\)\s*\((.*?)\)'
    match = re.match(pattern, text)
    if match:
        name, price, description, weight = match.groups()
        try:
            price = float(price)
            weight = float(weight)

            dish = Dish(name=name, price=price, description=description, weight=weight)
            menu = menu_api_client.get()
            menu.add_dish(dish)
            menu_api_client.create(menu)

            bot.send_message(message.chat.id, f'Блюдо "{name}" добавлено в меню.')
        except ValueError as e:
            bot.send_message(message.chat.id, 'Неверный формат сообщения. Пожалуйста, используйте формат: "(Название блюда) (Цена блюда) (Описание блюда) (Вес блюда)".')

bot.polling()
