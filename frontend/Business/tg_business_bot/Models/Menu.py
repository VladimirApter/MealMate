from pydantic import BaseModel
from typing import List, Optional

from tg_business_bot.Models.Category import Category


class Menu(BaseModel):
    id: Optional[int]
    restaurant_id: int
    categories: Optional[List[Category]]

    def __init__(self,
                 restaurant_id: int,
                 id: Optional[int] = None,
                 categories: Optional[List[Category]] = None):
        super().__init__(
            id=id,
            restaurant_id=restaurant_id,
            categories=categories
        )
