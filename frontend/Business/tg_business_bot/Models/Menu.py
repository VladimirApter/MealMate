from pydantic import BaseModel
from typing import List

from Category import Category


class Menu(BaseModel):
    id: int
    categories: List[Category]
