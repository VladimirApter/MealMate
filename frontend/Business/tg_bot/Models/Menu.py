from pydantic import BaseModel
from typing import List

from Models.Category import Category


class Menu(BaseModel):
    id: int
    categories: List[Category]
