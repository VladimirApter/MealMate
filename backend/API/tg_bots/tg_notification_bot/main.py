import time
from telebot.types import InlineKeyboardMarkup, InlineKeyboardButton
from ApiClient.ApiClient import ApiClient
from tg_notification_bot.Config import *
from Model.Order import Order, OrderStatus
from Model.WaiterCall import WaiterCall, WaiterCallStatus


def start_bot():
    while True:
        try:
            bot.polling(non_stop=True, timeout=50)
        except Exception as e:
            bot.send_message(chat_id=1488093047, text=f'Бот упал\n{e}')
            bot.send_message(chat_id=1139957269, text=f'Бот упал\n{e}')

            time.sleep(10)
            continue


def create_keyboard(item):
    keyboard = InlineKeyboardMarkup()
    if isinstance(item, Order):
        accept_button = InlineKeyboardButton("Принять", callback_data=f"{item.id}:accept")
        cancel_button = InlineKeyboardButton("Отменить", callback_data=f"{item.id}:cancel")
        keyboard.add(accept_button, cancel_button)
    if isinstance(item, WaiterCall):
        accept_button = InlineKeyboardButton("Принять", callback_data=f"{item.id}:accept_waiter")
        keyboard.add(accept_button)

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
                          "После выполнения заказа подтвердите его нажатием кнопки 'Выполнено'.\n"
                          "Вы также можете отменить заказ, нажав кнопку 'Отменить'.\n"
                          "После нажатия подтвердите действие и заказ будет отменен")


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


@bot.callback_query_handler(func=lambda call: call.data.endswith("cancel"))
def handle_cancel(call):
    order_id = call.data.split(":")[0]
    chat_id = call.message.chat.id

    bot.edit_message_reply_markup(chat_id=chat_id,
                                  message_id=call.message.message_id,
                                  reply_markup=None)

    confirm_keyboard = InlineKeyboardMarkup()
    yes_button = InlineKeyboardButton("Да", callback_data=f"{order_id}:confirm_cancel:{call.message.message_id}")
    no_button = InlineKeyboardButton("Нет", callback_data=f"{order_id}:restore_buttons:{call.message.message_id}")
    confirm_keyboard.add(yes_button, no_button)

    bot.send_message(chat_id=chat_id,
                     text="Вы уверены, что хотите отменить заказ?",
                     reply_markup=confirm_keyboard)


@bot.callback_query_handler(func=lambda call: "restore_buttons" in call.data)
def handle_restore(call):
    parts = call.data.split(':')
    order_id = parts[0]
    message_id = int(parts[-1])

    bot.edit_message_reply_markup(chat_id=call.message.chat.id,
                                  message_id=call.message.message_id,
                                  reply_markup=None)

    original_keyboard = InlineKeyboardMarkup()
    accept_button = InlineKeyboardButton("Принять", callback_data=f"{order_id}:accept")
    cancel_button = InlineKeyboardButton("Отменить", callback_data=f"{order_id}:cancel")
    original_keyboard.add(accept_button, cancel_button)

    bot.edit_message_reply_markup(chat_id=call.message.chat.id,
                                  message_id=message_id,
                                  reply_markup=original_keyboard)


@bot.callback_query_handler(func=lambda call: "confirm_cancel" in call.data)
def handle_confirm_cancel(call):
    parts = call.data.split(":")
    order_id = parts[0]
    original_message_id = parts[-1]

    api_client = ApiClient(Order)

    order = api_client.get(order_id)
    order.status = OrderStatus.COOKING
    order.status = int(order.status.value)

    api_client.post(order)

    bot.edit_message_reply_markup(chat_id=call.message.chat.id,
                                  message_id=call.message.message_id,
                                  reply_markup=None)
    bot.edit_message_reply_markup(chat_id=call.message.chat.id,
                                  message_id=int(original_message_id),
                                  reply_markup=None)

    bot.send_message(chat_id=call.message.chat.id,
                     text=f"Заказ {order_id} отменен.")


@bot.callback_query_handler(func=lambda call: call.data.endswith("accept"))
def handle_callback(call):
    chat_id = call.message.chat.id
    message_id = call.message.message_id

    order_id = call.data.split(':')[0]

    api_client = ApiClient(Order)

    order = api_client.get(order_id)
    order.status = OrderStatus.COOKING
    order.status = int(order.status.value)

    api_client.post(order)

    keyboard = InlineKeyboardMarkup()
    done_button = InlineKeyboardButton("Выполнено", callback_data=f"{order.id}:done")
    keyboard.add(done_button)

    bot.edit_message_reply_markup(chat_id=chat_id, message_id=message_id, reply_markup=keyboard)
    bot.reply_to(call.message, "Заказ подтвержден")


@bot.callback_query_handler(func=lambda call: call.data.endswith("done"))
def handle_callback(call):
    chat_id = call.message.chat.id
    message_id = call.message.message_id

    order_id = call.data.split(':')[0]

    api_client = ApiClient(Order)

    order = api_client.get(order_id)
    order.status = OrderStatus.DONE
    order.status = int(order.status.value)

    api_client.post(order)

    bot.answer_callback_query(call.id, "Заказ завершен!")
    bot.edit_message_reply_markup(chat_id=chat_id, message_id=message_id, reply_markup=None)
    bot.reply_to(call.message, "Заказ выполнен")


@bot.callback_query_handler(func=lambda call: call.data.endswith("accept_waiter"))
def handle_callback(call):
    chat_id = call.message.chat.id
    message_id = call.message.message_id
    waiter_id = call.data.split(':')[0]

    api_client = ApiClient(WaiterCall)

    waiter = api_client.get(waiter_id)
    waiter.status = WaiterCallStatus.DONE
    waiter.status = int(waiter.status.value)

    api_client.post(waiter)

    bot.edit_message_reply_markup(chat_id=chat_id, message_id=message_id, reply_markup=None)
    bot.reply_to(call.message, "Официант вышел")




