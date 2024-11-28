import os
import threading

from telethon import TelegramClient
from telethon.sessions import SQLiteSession

phone_number = '+79090020934'
api_id = 21160399
api_hash = '83b9c867df4ca55e73fd74bb796e02a2'

current_file_path = os.path.abspath(__file__)
current_directory = os.path.dirname(current_file_path)
session_path = os.path.join(current_directory, 'tg_notification_bot_session')


db_lock = threading.Lock()
with db_lock:
    client = TelegramClient(SQLiteSession(session_path), api_id, api_hash,
                            system_version='4.16.30-vxCUSTOM')
