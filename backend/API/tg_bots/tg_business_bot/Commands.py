from telebot import types
from ApiClient.ApiClient import ApiClient
from Config import bot
from Model.Owner import Owner
from Model.Restaurant import Restaurant
from tg_business_bot.SendQr import send_qrs
from tg_business_bot.Registration import register_restaurant_name
from tg_business_bot.Update import get_restaurants_markup, get_update_parts_markup, get_update_part
from tg_business_bot.DeleteRestaurant import delete_restaurant
from DataValidationAndPost import get_yes_no_markup


def register_commands():
    @bot.message_handler(commands=['help'])
    def help_to_use(message: types.Message):
        bot.send_message(message.chat.id, "Данный бот позволяет Вам быстро подключить свой ресторан к нашей системе.\n"
                                          "При помощи qr кода клиент будет перенаправлен на сайт вашего ресторана с меню.\n"
                                          "После совершения заказа будет отправляться уведомление в telegram бота с деталями заказа.\n"
                                          "Пользователя, которому будут отправляться уведоления, Вы выбираете сами.\n"
                                          "Краткая справка о поддерживаемых командах\n"
                                          "/start - запуск бота.\n"
                                          "/register - регистрация, сохраним ваши данные.\n"
                                          "/new_rest -добавить новый ресторан.\n"
                                          "/qr - выслать qr коды всех столов.\n"
                                          "/update - обновить данные о ресторане.\n"
                                          "/delete - удалить ресторан.\n"
                                          "Команды работают только для зарегистрированных пользователей.\n"
                                          "Бот автоматически сохраняет изменения в базе данных.\n")

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
        owner, restaurants = get_owner_and_restaurants(message)
        if not owner or not restaurants:
            return

        if len(restaurants) == 1:
            send_qrs(message, restaurants[0])
        else:
            markup = get_restaurants_markup(restaurants)
            bot.send_message(message.chat.id, 'Выберите ресторан', reply_markup=markup)
            bot.register_next_step_handler(message, get_restaurant_for_send_qr, restaurants)

    @bot.message_handler(commands=['update'])
    def update(message: types.Message):
        owner, restaurants = get_owner_and_restaurants(message)
        if not owner or not restaurants:
            return

        if len(restaurants) == 1:
            markup = get_update_parts_markup()
            bot.send_message(message.chat.id,
                             f'Что вы хотели бы изменить в ресторане {restaurants[0].name}?',
                             reply_markup=markup)
            bot.register_next_step_handler(message, get_update_part, restaurants[0])
        else:
            markup = get_restaurants_markup(restaurants)
            bot.send_message(message.chat.id, 'Выберите ресторан', reply_markup=markup)
            bot.register_next_step_handler(message, get_restaurant_for_update, restaurants)

    @bot.message_handler(commands=['delete'])
    def delete(message: types.Message):
        owner, restaurants = get_owner_and_restaurants(message)
        if not owner or not restaurants:
            return

        if len(restaurants) == 1:
            markup = get_update_parts_markup()
            restaurant = restaurants[0]
            bot.send_message(message.chat.id,
                             f'Вы уверены что хотите удалить ресторан {restaurant.name}?',
                             reply_markup=markup)
            bot.register_next_step_handler(message, delete_restaurant, restaurant)
        else:
            markup = get_restaurants_markup(restaurants)
            bot.send_message(message.chat.id, 'Выберите ресторан', reply_markup=markup)
            bot.register_next_step_handler(message, get_restaurant_for_delete, restaurants)


def get_owner_and_restaurants(message: types.Message):
    api_client = ApiClient(Owner)
    owner = api_client.get(message.chat.id)

    if not owner:
        bot.send_message(message.chat.id, "Сначала нужно зарегистрироваться при помощи /new_rest")
        return None, None
    if not owner.restaurant_ids:
        bot.send_message(message.chat.id, "У вас пока нет ресторанов, создайте первый при помощи /new_rest")
        return owner, None

    api_client = ApiClient(Restaurant)
    restaurants = [api_client.get(rest_id) for rest_id in owner.restaurant_ids]

    return owner, restaurants


def get_restaurant_for_update(message: types.Message, restaurants):
    if not message.text:
        bot.send_message(message.chat.id, 'Чтобы выбрать ресторан нажмите на одну из кнопок ниже')
        bot.register_next_step_handler(message, get_restaurant_for_update, restaurants)
        return

    restaurant = None
    user_rest_name = message.text.strip()
    for rest in restaurants:
        if rest.name == user_rest_name:
            restaurant = rest

    if restaurant is None:
        bot.send_message(message.chat.id, 'Чтобы выбрать ресторан нажмите на одну из кнопок ниже')
        bot.register_next_step_handler(message, get_restaurant_for_update, restaurants)
        return

    markup = get_update_parts_markup()
    bot.send_message(message.chat.id, 'Что вы хотели бы изменить?', reply_markup=markup)
    bot.register_next_step_handler(message, get_update_part, restaurant)


def get_restaurant_for_delete(message: types.Message, restaurants):
    if not message.text:
        bot.send_message(message.chat.id, 'Чтобы выбрать ресторан нажмите на одну из кнопок ниже')
        bot.register_next_step_handler(message, get_restaurant_for_delete, restaurants)
        return

    restaurant = None
    user_rest_name = message.text.strip()
    for rest in restaurants:
        if rest.name == user_rest_name:
            restaurant = rest

    if restaurant is None:
        bot.send_message(message.chat.id, 'Чтобы выбрать ресторан нажмите на одну из кнопок ниже')
        bot.register_next_step_handler(message, get_restaurant_for_delete, restaurants)
        return

    markup = get_yes_no_markup()
    bot.send_message(message.chat.id, f'Вы уверены что хотите удалить ресторан {restaurant.name}?', reply_markup=markup)
    bot.register_next_step_handler(message, delete_restaurant, restaurant)


def get_restaurant_for_send_qr(message: types.Message, restaurants):
    if not message.text:
        bot.send_message(message.chat.id, 'Чтобы выбрать ресторан нажмите на одну из кнопок ниже')
        bot.register_next_step_handler(message, get_restaurant_for_send_qr, restaurants)
        return

    restaurant = None
    user_rest_name = message.text.strip()
    for rest in restaurants:
        if rest.name == user_rest_name:
            restaurant = rest

    if restaurant is None:
        bot.send_message(message.chat.id, 'Чтобы выбрать ресторан нажмите на одну из кнопок ниже')
        bot.register_next_step_handler(message, get_restaurant_for_send_qr, restaurants)
        return

    send_qrs(message, restaurant)
