import os

phone_number = '+79090020934'
api_id = 21160399
api_hash = '83b9c867df4ca55e73fd74bb796e02a2'

current_file_path = os.path.abspath(__file__)
current_directory = os.path.dirname(current_file_path)
session_path = os.path.join(current_directory, "sessions", 'tg_notification_bot_session')
os.makedirs(os.path.dirname(session_path), exist_ok=True)
