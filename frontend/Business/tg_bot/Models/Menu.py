from pydantic import BaseModel
from typing import List

from Models.Category import Category


class Menu(BaseModel):
    categories: List[Category]
