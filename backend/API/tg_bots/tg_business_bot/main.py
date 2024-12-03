import time
from telebot import types

from Config import *
from Commands import register_commands
from excel_tables_work.create_menu_template import create_menu_template

create_menu_template()
register_commands()


@bot.message_handler(commands=['start'])
def start(message: types.Message):
    bot.send_message(message.chat.id, 'Для регистрации используйте /register\n'
                                      'Для получения подробной информации о боте используйте /help')


while True:
    try:
        bot.polling(non_stop=True, timeout=50)
    except Exception as e:
        bot.send_message(chat_id=1488093047, text=f'Бот упал\n{e}')
        bot.send_message(chat_id=1139957269, text=f'Бот упал\n{e}')

        time.sleep(10)
        continue
