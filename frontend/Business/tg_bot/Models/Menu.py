from pydantic import BaseModel
from typing import List

from Models.Dish import Dish


class Menu(BaseModel):
    dish_list: List[Dish]

    def add_dish(self, dish):
        self.dish_list.append(dish)
