from telebot import types
from Config import *
from Commands import register_commands
from excel_tables_work.create_menu_template import create_menu_template
from time import sleep

create_menu_template()
register_commands()


@bot.message_handler(commands=['start'])
def start(message: types.Message):
    bot.send_message(message.chat.id, 'Для регистрации используйте /register')

try:
    bot.polling()
except:
    print("exception in tg bot ")
