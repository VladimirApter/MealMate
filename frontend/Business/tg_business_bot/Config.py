import telebot
from telebot.storage import StateMemoryStorage

bot_token = "7449757271:AAF5uM4dBn9PHJ-ka5jEIp00Mt5-TR9Wqds"
bot = telebot.TeleBot(bot_token, state_storage=StateMemoryStorage())

geocoder_api_key = "47ff557b-d013-4f66-8d62-11de6da89ad1"

api_base_url = "http://localhost:5211"
