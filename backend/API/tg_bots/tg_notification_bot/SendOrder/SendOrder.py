from ApiClient.ApiClient import ApiClient
from Model.Table import Table
from Model.Restaurant import Restaurant
from Model.Order import Order
from tg_notification_bot.Config import bot
from tg_notification_bot.main import create_keyboard
from telebot.apihelper import ApiException


def send_order(order: Order):
    try:
        try:
            api_client = ApiClient(Table)
            table = api_client.get(order.table_id)
        except Exception as e:
            bot.send_message(chat_id=1488093047, text=f'Возникла ошибка при получении table: {e}')
            bot.send_message(chat_id=1139957269, text=f'Возникла ошибка при получении table: {e}')
            return

        try:
            api_client = ApiClient(Restaurant)
            restaurant = api_client.get(table.restaurant_id)
            notification_getter = restaurant.notification_getter
        except Exception as e:
            bot.send_message(chat_id=1488093047, text=f'Возникла ошибка при получении ресторана: {e}')
            bot.send_message(chat_id=1139957269, text=f'Возникла ошибка при получении ресторана: {e}')
            return

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
        try:
            bot.send_message(chat_id=notification_getter.id, text=message, reply_markup=keyboard)
        except ApiException as e:
            bot.send_message(chat_id=1488093047, text=f'Возникла ошибка при отправке сообщения: {e}')
            bot.send_message(chat_id=1139957269, text=f'Возникла ошибка при отправке сообщения: {e}')

    except Exception as e:
        bot.send_message(chat_id=1488093047, text=f'Возникла ошибка при подготовке данных: {e}')
        bot.send_message(chat_id=1139957269, text=f'Возникла ошибка при подготовке данных: {e}')
