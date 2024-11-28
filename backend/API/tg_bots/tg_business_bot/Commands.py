from telebot import types

from ApiClient.ApiClient import ApiClient
from Config import bot
from Model.Owner import Owner
from Model.Restaurant import Restaurant
from tg_business_bot.SendQr import get_restaurant, send_qrs
from tg_business_bot.Registration import register_restaurant_name
from tg_business_bot.Update import get_restaurants_markup, \
    get_update_parts_markup, get_update_part


def register_commands():
    @bot.message_handler(commands=['register'])
    def register_start(message: types.Message):
        api_client = ApiClient(Owner)

        owner_id = message.chat.id
        owner = api_client.get(owner_id)
        if owner is None:
            owner = Owner(id=owner_id, username=message.from_user.username)
            api_client.post(owner)

        bot.send_message(message.chat.id, 'Вы зарегистрированы, ваши данные сохранены! Можете добавить свой ресторан с помощью команды /new_rest')

    @bot.message_handler(commands=['new_rest'])
    def restaurant_register_start(message: types.Message):
        api_client = ApiClient(Owner)
        owner = api_client.get(message.chat.id)

        if owner is None:
            bot.send_message(message.chat.id, "Для начала вам нужно зарегестрироваться, используйте /register")
            return

        restaurant = Restaurant(name='', owner_id=owner.id)
        register_restaurant_name(message, restaurant)

    @bot.message_handler(commands=['qr'])
    def qr(message: types.Message):
        api_client = ApiClient(Owner)
        owner = api_client.get(message.chat.id)

        restaurant_ids = owner.restaurant_ids

        if len(restaurant_ids) == 0 or owner is None:
            bot.send_message(message.chat.id, "У вас пока нет ресторанов, создайте первый при помощи /new_rest")
            return
        elif len(restaurant_ids) == 1:
            api_client = ApiClient(Restaurant)
            restaurant = api_client.get(restaurant_ids[0])
            send_qrs(message, restaurant)
            return
        elif len(restaurant_ids) > 1:
            api_client = ApiClient(Restaurant)
            restaurants = [api_client.get(rest_id) for rest_id in restaurant_ids]

            markup = get_restaurants_markup(restaurants)
            bot.send_message(message.chat.id, 'Выберите ресторан', reply_markup=markup)
            bot.register_next_step_handler(message, get_restaurant, restaurants)

    @bot.message_handler(commands=['update'])
    def update_start(message: types.Message):
        api_client = ApiClient(Owner)
        owner = api_client.get(message.chat.id)

        restaurant_ids = owner.restaurant_ids

        if len(restaurant_ids) == 0 or owner is None:
            bot.send_message(message.chat.id, "У вас пока нет ресторанов, создайте первый при помощи /new_rest")
            return
        elif len(restaurant_ids) == 1:
            api_client = ApiClient(Restaurant)
            restaurant = api_client.get(restaurant_ids[0])

            markup = get_update_parts_markup()
            bot.send_message(message.chat.id,
                             f'Что вы хотели бы изменить в ресторане {restaurant.name}?',
                             reply_markup=markup)
            bot.register_next_step_handler(message, get_update_part, restaurant)
            return
        elif len(restaurant_ids) > 1:
            api_client = ApiClient(Restaurant)
            restaurants = [api_client.get(rest_id) for rest_id in restaurant_ids]

            markup = get_restaurants_markup(restaurants)
            bot.send_message(message.chat.id, 'Выберите ресторан', reply_markup=markup)
            bot.register_next_step_handler(message, get_restaurant, restaurants)
