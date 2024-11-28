from telebot.types import InlineKeyboardMarkup, InlineKeyboardButton
from ApiClient.ApiClient import ApiClient
from tg_notification_bot.Config import *
from Model.Order import Order, OrderStatus


def start_bot():
    bot.polling()


def create_keyboard(order: Order):
    keyboard = InlineKeyboardMarkup()
    button1 = InlineKeyboardButton("Принять", callback_data=f"{order.id}:accept")
    button2 = InlineKeyboardButton("Выполнено", callback_data=f"{order.id}:done")
    keyboard.add(button1, button2)
    return keyboard


@bot.message_handler(commands=['start'])
def send_welcome(message):
    bot.reply_to(message, "Добро пожаловать! Используйте /help для получения справки.")


@bot.message_handler(commands=['help'])
def send_help(message):
    bot.reply_to(message, "Когда создается новый заказ, бот автоматически отправляет вам уведомление.\n"
                          "Уведомление будет содержать информацию о заказе\n"
                          "После получения уведомления о заказе, вам нужно подтвердить, что заказ в обработке.\n"
                          "Для этого нажмите кнопку 'Принять'.\n"
                          "После выполнения заказа подтвердите его нажатием кнопки 'Выполнено'")


@bot.message_handler(func=lambda message: message.content_type != 'text')
def handle_non_text_messages(message):
    bot.reply_to(message, "Поддерживаются только команды /start и /help.")


@bot.message_handler(content_types=['text'])
def handle_text(message):
    bot.reply_to(message, "Эта команда или сообщение не поддерживаются. Используйте /start или /help.")


@bot.message_handler(content_types=['photo'])
def handle_photo(message):
    bot.reply_to(message, "Фото не поддерживаются. Используйте /start или /help.")


@bot.message_handler(content_types=['sticker', 'video', 'document', 'audio', 'voice', 'animation'])
def handle_other_content(message):
    bot.reply_to(message, "Этот тип сообщений не поддерживается. Используйте /start или /help.")


@bot.callback_query_handler(func=lambda call: True)
def handle_callback(call):
    chat_id = call.message.chat.id
    message_id = call.message.message_id

    api_client = ApiClient(Order)
    order_id, order_status = call.data.split(':')
    order = api_client.get(order_id)

    if order_status == 'accept':
        order.status = OrderStatus.COOKING
        order.status = int(order.status.value)
        api_client.post(order)

        keyboard = InlineKeyboardMarkup()
        button2 = InlineKeyboardButton("Выполнено", callback_data=f"{order.id}:done")
        keyboard.add(button2)

        bot.edit_message_reply_markup(chat_id=chat_id, message_id=message_id, reply_markup=keyboard)
        bot.reply_to(call.message, "Заказ подтвержден")

    if order_status == "done":
        if order.status != OrderStatus.COOKING.value:
            bot.answer_callback_query(call.id, "Нельзя завершить заказ без подтверждения!")
        else:
            order.status = OrderStatus.DONE
            order.status = int(order.status.value)
            api_client.post(order)

            bot.answer_callback_query(call.id, "Заказ завершен!")
            bot.edit_message_reply_markup(chat_id=chat_id, message_id=message_id, reply_markup=None)
            bot.reply_to(call.message, "Заказ выполнен")
