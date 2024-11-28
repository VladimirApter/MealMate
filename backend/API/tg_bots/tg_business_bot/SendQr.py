from telebot import types

from Config import *
from Model.Restaurant import Restaurant


def send_qrs(message: types.Message, restaurant: Restaurant):
    for table in restaurant.tables:
        qr_code_image_path = os.path.join(qr_images_dir, table.qr_code_image_path)
        with open(qr_code_image_path, 'rb') as qr_image:
            bot.send_document(message.chat.id, qr_image, caption=f"#qr код стола номер {table.number}", visible_file_name=f"стол_{table.number}_qr.png")
