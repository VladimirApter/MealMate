from pydantic import BaseModel
from typing import List

from Dish import Dish


class Category(BaseModel):
    id: int
    name: str
    dishes: List['Dish']
