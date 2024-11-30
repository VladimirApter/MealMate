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


while True:  # Бот будет пытаться работать бесконечно
    try:
        bot.polling(non_stop=True)  # Запускаем polling с опцией non_stop, чтобы бот продолжал работу
    except telebot.apihelper.ApiTelegramException as e:
        if e.result_json.get("error_code") == 409:
            print("Ошибка 409: Конфликт, другой запрос getUpdates.")
            time.sleep(1)  # Подождем секунду и пробуем снова
            continue  # Переходим к следующей попытке
        else:
            print(f"Произошла другая ошибка: {e}")
            time.sleep(1)  # Подождем секунду перед повторной попыткой
            continue  # Переходим к следующей попытке
