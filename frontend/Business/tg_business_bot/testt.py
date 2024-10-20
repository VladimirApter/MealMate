import asyncio
import telebot
from telethon import TelegramClient
from telethon.errors import UsernameNotOccupiedError

# Замените 'YOUR_API_ID' и 'YOUR_API_HASH' на ваши данные
api_id = 21160399  # 20475882
api_hash = '83b9c867df4ca55e73fd74bb796e02a2'  # 'e8a0c24616f7d7994a7cf55c3e792efc'
phone = '+79041706275'
phone = '+79090020934'


def get_id_by_username(username):
    async def main():
        async with TelegramClient('session_name', api_id, api_hash) as client:
            await client.start(phone)
            try:
                user = await client.get_input_entity(username)
                return user.id
            except UsernameNotOccupiedError:
                return None

    return asyncio.run(main())

bot = telebot.TeleBot('7449757271:AAF5uM4dBn9PHJ-ka5jEIp00Mt5-TR9Wqds')


@bot.message_handler(commands=['start'])
def send_welcome(message):
    bot.send_message(message.chat.id, 'Enter username')
    bot.register_next_step_handler(message, find_user)


def find_user(message):
    username = message.text.strip()
    user_id = get_id_by_username(username)
    if user_id:
        bot.send_message(message.chat.id, f"User ID: {user_id}")
    else:
        bot.send_message(message.chat.id, "User not found")


bot.polling()
