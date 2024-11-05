from time import sleep

from DataValidationAndPost import *
from Update import get_notification_getter_markup
from Config import current_dir


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

        restaurant = Restaurant(address='', name='', owner_id=owner.id)
        register_restaurant_name(message, restaurant)


def register_restaurant_name(message: types.Message, restaurant: Restaurant):
    bot.send_message(message.chat.id, 'Введите название ресторана')
    bot.register_next_step_handler(message, validate_and_post_restaurant_name, restaurant, register_restaurant_address, True)


def register_restaurant_address(message: types.Message, restaurant: Restaurant):
    bot.send_message(message.chat.id, 'Теперь введите адрес заведения (в свободной форме)')
    bot.register_next_step_handler(message, validate_and_post_address, restaurant, register_notification_getter, True)


def register_notification_getter(message: types.Message, restaurant: Restaurant):
    api_client = ApiClient(Restaurant)
    restaurant.id = api_client.post(restaurant)

    bot.send_message(message.chat.id, 'Сейчас нужно назначить человека, которому будут приходить уведомления о заказах', reply_markup=get_notification_getter_markup())
    bot.register_next_step_handler(message, validate_and_post_notification_getter, restaurant, register_restaurant_menu, True)


def register_restaurant_menu(message: types.Message, restaurant: Restaurant):
    bot.send_message(message.chat.id, 'Перейдем к созданию меню')

    menu_template_path = os.path.join(current_dir, "excel_tables_work", "menu_template.xlsx")
    with open(menu_template_path, 'rb') as file:
        bot.send_document(message.chat.id, file, caption="Этот файл - шаблон меню, в нем есть две таблицы: Блюда и Напитки. "
                                                         "Заполните таблицы в соответствии с Вашим меню и отправьте мне файл")

    bot.register_next_step_handler(message, validate_and_post_menu, restaurant, restaurant_register_finish, True)


def restaurant_register_finish(message: types.Message, restaurant: Restaurant):
    api_client = ApiClient(Restaurant)
    api_client.post(restaurant)

    bot.send_message(message.chat.id, "Ресторан добавлен!")


def set_pause_between_messages(message, sleep_time):
    bot.send_chat_action(message.chat.id, 'typing')
    sleep(sleep_time)
