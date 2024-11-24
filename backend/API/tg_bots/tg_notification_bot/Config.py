import telebot
from telebot.storage import StateMemoryStorage

bot_token = "8197472429:AAF_iqDMpHYeeg3nf9nGNjU1V9uHBgb9_NA"
bot = telebot.TeleBot(bot_token, state_storage=StateMemoryStorage())

api_base_url = "http://localhost:5211"
