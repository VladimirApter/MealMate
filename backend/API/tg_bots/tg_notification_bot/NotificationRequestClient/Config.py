from telethon import TelegramClient

phone_number = '+79090020934'
api_id = 21160399
api_hash = '83b9c867df4ca55e73fd74bb796e02a2'

client = TelegramClient('tg_notification_bot_session', api_id, api_hash,
                        system_version='4.16.30-vxCUSTOM').start()
