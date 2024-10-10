from pydantic import BaseModel
from typing import List

from Category import Category


class Menu(BaseModel):
    id: int = None
    restaurant_id: int = None
    categories: List[Category]
