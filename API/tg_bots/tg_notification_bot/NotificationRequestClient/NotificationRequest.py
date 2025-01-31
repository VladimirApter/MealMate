import asyncio
from telethon import TelegramClient
from telethon.errors import UserPrivacyRestrictedError
from telethon.sessions import SQLiteSession
from telethon.tl.types import InputPeerUser
from tg_notification_bot.NotificationRequestClient.Config import api_id, api_hash, session_path
from Model.NotificationGetter import NotificationGetter
from Model.Owner import Owner
from Model.Restaurant import Restaurant
from ApiClient.ApiClient import ApiClient


async def send_notification_request(notification_getter: NotificationGetter):
    loop = asyncio.new_event_loop()

    client = TelegramClient(SQLiteSession(session_path), api_id, api_hash,
                            system_version='4.16.30-vxCUSTOM', loop=loop)
    async with client:
        try:
            if notification_getter.is_blocked:
                return

            owner, restaurant = _get_owner_and_restaurant_by_notification_getter(notification_getter)
            message = f'Здравствуйте, пользователь @{owner.username} назначил вас ' \
                      f'получателем уведомлений в ресторане "{restaurant.name}". ' \
                      f'Если готовы начать работу, запустите бота ' \
                      f'@MealMateNotification_bot. Если вас назначили по ошибке, ' \
                      f'то ничего делать не нужно, я вас больше не побеспокою.'

            user = await client.get_entity(notification_getter.username)
            peer = InputPeerUser(user.id, user.access_hash)
            await client.send_message(peer, message)
        except UserPrivacyRestrictedError:
            pass
        except Exception as e:
            print(f"An error occurred: {e}")


def _get_owner_and_restaurant_by_notification_getter(notification_getter: NotificationGetter):
    api_client = ApiClient(Restaurant)
    restaurant = api_client.get(notification_getter.restaurant_id)

    if restaurant is None:
        return None

    api_client = ApiClient(Owner)
    owner = api_client.get(restaurant.owner_id)

    if owner is None:
        return None
    return owner, restaurant


def send_notification(notification_getter):
    asyncio.run(send_notification_request(notification_getter))
