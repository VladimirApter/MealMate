import os

import telebot

menu_item_images_dir = os.getenv("MENUITEMIMAGES_PATH", os.path.join('..', '..', '..', 'Application', 'wwwroot', 'MenuItemImages'))
excel_tables_dir = os.getenv("TABLES_PATH", os.path.join('..', '..', '..', 'DataBase', 'ExcelTables'))
qr_images_dir = os.getenv("QRIMAGES_PATH", os.path.join('..', '..', '..', 'DataBase', 'QRCodeImages'))

bot_token = "7449757271:AAF5uM4dBn9PHJ-ka5jEIp00Mt5-TR9Wqds"
bot = telebot.TeleBot(bot_token)

geocoder_api_key = "47ff557b-d013-4f66-8d62-11de6da89ad1"

current_dir = os.getcwd()
