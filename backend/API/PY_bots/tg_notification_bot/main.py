from Config import *


async def main():
    await client.start()

    user_name = None
    user = await client.get_entity(user_name)

    await client.send_message(user, 'Привет! Это сообщение от бота.')


with client:
    try:
        client.loop.run_until_complete(main())
    except Exception as e:
        print(e)
