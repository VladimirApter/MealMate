from pydantic import BaseModel
from typing import List

from Models.Category import Category


class Menu(BaseModel):
    id: int = None
    restaurant_id: int = None
    categories: List[Category]
