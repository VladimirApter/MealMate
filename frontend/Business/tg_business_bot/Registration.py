from DataValidationAndPost import *


@bot.message_handler(commands=['register'])
def register_start(message: types.Message):
    api_client = ApiClient(Owner)

    owner_id = message.chat.id
    owner = api_client.get(owner_id)
    if owner is None:
        owner = Owner()
        owner.id = message.chat.id
        owner.username = message.from_user.username
        api_client.post(owner)

    bot.send_message('Вы зарегистрированы! Можете добавить свой ресторан с помощью команды /new_rest')


@bot.message_handler(commands=['new_rest'])
def restaurant_register_start(message: types.Message):
    api_client = ApiClient(Owner)
    owner = api_client.get(message.chat.id)
    if owner is None:
        bot.send_message("Для начала вам нужно зарегестрироваться, используйте /register")
        return

    restaurant = Restaurant()
    restaurant.owner_id = owner.id

    bot.send_message(message.chat.id, 'Введите название ресторана')
    bot.register_next_step_handler(message, validate_and_post_restaurant_name, restaurant)


def register_restaurant_name(message: types.Message, restaurant: Restaurant):
    restaurant.name = validate_and_post_restaurant_name(message)
    bot.send_message(message.chat.id, 'Принял, теперь введите адрес заведения (в свободной форме)')
    bot.register_next_step_handler(message, validate_and_post_address, restaurant)


def register_restaurant_address(message: types.Message, restaurant: Restaurant):
    restaurant.address = validate_and_post_address(message, restaurant)
    restaurant_register_finish(message, restaurant)


def restaurant_register_finish(message: types.Message, restaurant: Restaurant):
    api_client = ApiClient(Restaurant)
    api_client.post(restaurant)

    bot.send_message(message.chat.id, "Ресторан добавлен! Для добавления данных о ресторане выберете его в списке ресторанов по команде /my_rests")

