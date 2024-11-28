from typing import List, Dict, Union

from ApiClient.ApiClient import ApiClient
from Model import Table, Restaurant, NotificationGetter, Dish, Drink
from Model.Order import Order
from tg_notification_bot.Config import bot
from tg_notification_bot.main import create_keyboard


def send_order(order: Order):
    # api_client = ApiClient(Table)
    # table = api_client.get(order.table_id)
    #
    # api_client = ApiClient(Restaurant)
    # restaurant = api_client.get(table.restaurant_id)
    #
    # notification_getter_user_id = restaurant.notification_getter.id
    dishes = [item for item in order.order_items if "weight" in item.menu_item.dict()]
    drinks = [item for item in order.order_items if "volume" in item.menu_item.dict()]

    formatted_dishes = [
        f"{item.menu_item.name}, "
        f"{int(item.menu_item.weight)}г"
        f" ({item.count})"
        for item in dishes
    ]
    formatted_drinks = [
        f"{item.menu_item.name}, "
        f"{int(item.menu_item.volume)}мл"
        f" ({item.count})"
        for item in drinks
    ]

    message = (f"Новый заказ!\n\n"
               f"Заказ №{order.id}\n"
               f"Стол: {order.table_id}\n"
               f"Время заказа: {order.date_time.strftime('%H:%M')}\n"
               f"Комментарии: {order.comment}\n\n")

    if formatted_dishes:
        message += "Блюда:\n" + "\n".join(formatted_dishes) + "\n\n"

    if formatted_drinks:
        message += "Напитки:\n" + "\n".join(formatted_drinks) + "\n\n"

    message += f"Итоговая цена: {order.price} руб."

    keyboard = create_keyboard(order)

    bot.send_message(chat_id=1488093047, text=message, reply_markup=keyboard)  # replace 'chat_id' on n_g_u_i
