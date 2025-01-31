from ApiClient.ApiClient import ApiClient
from Model.Table import Table
from Model.Restaurant import Restaurant
from Model.WaiterCall import WaiterCall
from tg_notification_bot.Config import bot
from tg_notification_bot.main import create_keyboard


def send_waiter_call(waiter_call: WaiterCall):
    api_client = ApiClient(Table)
    table = api_client.get(waiter_call.table_id)

    api_client = ApiClient(Restaurant)
    restaurant = api_client.get(table.restaurant_id)
    notification_getter = restaurant.notification_getter

    message = (f"Вызов официанта!\n\n"
               f"Стол: {waiter_call.table_id}\n"
               f"Время: {waiter_call.date_time.strftime('%H:%M')}\n")

    keyboard = create_keyboard(waiter_call)

    bot.send_message(chat_id=notification_getter.id, text=message, reply_markup=keyboard)
