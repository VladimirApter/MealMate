import requests
from pydantic import BaseModel
from typing import List

class Dish(BaseModel):
    price: float
    weight: float
    name: str
    description: str

class Menu(BaseModel):
    dish_list: List[Dish]

def fetch_and_deserialize(url, model):
    response = requests.get(url)
    if response.status_code == 200:
        print(response.json())
        return model.parse_obj(response.json())
    else:
        raise Exception(f"Failed to retrieve data: {response.status_code}")

# URL вашего GET-запроса
url = 'http://localhost:5211/api/menu'

# Выполнение GET-запроса и десериализация JSON-ответа в объект класса Menu
try:
    menu = fetch_and_deserialize(url, Menu)
    # Вывод информации о меню
    for dish in menu.dish_list:
        print(f"Name: {dish.name}, Price: {dish.price}, Weight: {dish.weight}, Description: {dish.description}")
except Exception as e:
    print(e)
