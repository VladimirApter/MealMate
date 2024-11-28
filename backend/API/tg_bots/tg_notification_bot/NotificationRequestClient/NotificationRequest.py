from API.tg_bots.tg_notification_bot.NotificationRequestClient.Config import client
from API.tg_bots.Model.NotificationGetter import NotificationGetter
from API.tg_bots.Model.Owner import Owner
from API.tg_bots.Model.Restaurant import Restaurant
from API.tg_bots.ApiClient.ApiClient import ApiClient


async def send_notification_request(notification_getter: NotificationGetter):
    if notification_getter.is_blocked:
        return

    owner, restaurant = _get_owner_and_restaurant_by_notification_getter(notification_getter)
    message = f'Здравствуйте, пользователь @{owner.username} назначил вас ' \
              f'получателем уведомлений в ресторане "{restaurant.name}". ' \
              f'Если готовы начать работу, запустите бота ' \
              f'@MealMateNotification_bot. Если вас назначили по ошибке, ' \
              f'то ничего делать не нужно, я вас больше не побеспокою.'

    user = await client.get_entity(notification_getter.username)
    await client.send_message(user, message)


def _get_owner_and_restaurant_by_notification_getter(notification_getter: NotificationGetter):
    api_client = ApiClient(Restaurant)
    restaurant = api_client.get(notification_getter.restaurant_id)

    if restaurant is None:
        return None

    api_client = ApiClient(Owner)
    owner = api_client.get(restaurant.owner_id)

    return owner, restaurant