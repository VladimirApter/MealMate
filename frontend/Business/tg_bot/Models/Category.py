from pydantic import BaseModel
from typing import List

from Models.Dish import Dish


class Category(BaseModel):
    name: str
    dishes: List['Dish']
