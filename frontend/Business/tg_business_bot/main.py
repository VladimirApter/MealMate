from telebot import types
from Config import *
from Registration import register_commands


register_commands()


@bot.message_handler(commands=['start'])
def start(message: types.Message):
    bot.send_message(message.chat.id, 'Для регистрации используйте /register')


bot.polling()
