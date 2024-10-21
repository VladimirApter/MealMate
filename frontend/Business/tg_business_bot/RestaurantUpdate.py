from DataValidationAndPost import *


@bot.message_handler(commands=['update'])
def update_start(message: types.Message):
    api_client = ApiClient(Owner)
    owner = api_client.get(message.chat.id)

    restaurant_ids = owner.restaurant_ids
    api_client = ApiClient(Restaurant)
    restaurants = [api_client.get(rest_id) for rest_id in restaurant_ids]

    if len(restaurants) > 1:
        markup = get_restaurants_markup(restaurants)
        bot.send_message(message.chat.id, 'Выберите ресторан', reply_markup=markup)
        bot.register_next_step_handler(message, get_restaurant, restaurants)


def get_restaurants_markup(restaurants):
    markup = types.ReplyKeyboardMarkup()
    row = []
    for rest in restaurants:
        row.append(types.KeyboardButton(rest.name))
        if len(row) == 2:
            markup.add(*row)
            row = []
    if row:
        markup.add(*row)
    return markup


def get_restaurant(message: types.Message, restaurants):
    restaurant = None
    user_rest_name = message.text.strip()
    for rest in restaurants:
        if rest.name == user_rest_name:
            restaurant = rest

    if restaurant is None:
        bot.send_message(message.chat.id, 'Чтобы выбрать ресторан нажмите на одну из кнопок ниже')
        bot.register_next_step_handler(message, get_restaurant, restaurants)

    markup = get_update_parts_markup()
    bot.send_message(message.chat.id, 'Что Вы хотели бы изменить?', reply_markup=markup)
    bot.register_next_step_handler(message, get_update_part, restaurant)


def get_update_parts_markup():
    markup = types.ReplyKeyboardMarkup()

    rows = [
        [types.KeyboardButton('адрес'), types.KeyboardButton('меню')],
        [types.KeyboardButton('столы'), types.KeyboardButton('название')],
        [types.KeyboardButton('получатель уведомлений')]
    ]

    for row in rows:
        markup.add(*row)

    return markup


def get_update_part(message: types.Message, restaurant: Restaurant):
    part = message.text.strip()

    if part == 'адрес':
        bot.send_message('Введите адрес ресторана')
        bot.register_next_step_handler(message, validate_and_post_address, restaurant)
    elif part == 'меню':
        pass
    elif part == 'столы':
        pass
    elif part == 'название':
        bot.send_message('Введите название ресторана')
        bot.register_next_step_handler(message, validate_and_post_address, restaurant)
    elif part == 'получатель уведомлений':
        pass
    else:
        bot.send_message('Для выбора части, которую вы хотели бы изменить, нажмите на одну из кнопок ниже')
        bot.register_next_step_handler(message, get_update_part, restaurant)


