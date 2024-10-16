from pydantic import BaseModel
from typing import List

from Models.Dish import Dish


class Category(BaseModel):
    id: int
    name: str
    dishes: List['Dish']
