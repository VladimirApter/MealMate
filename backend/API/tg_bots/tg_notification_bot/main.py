from Config import *


@bot.message_handler(commands=['start'])
def send_welcome(message):
    bot.reply_to(message, "<инструкция по работе с ботом>")


bot.polling()
