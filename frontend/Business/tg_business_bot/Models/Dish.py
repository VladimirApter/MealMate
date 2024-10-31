from pydantic import BaseModel
from typing import Optional

from Models.MenuItem import MenuItem


class Dish(MenuItem):
    weight: float

    def __init__(self,
                 category_id: int,
                 price: float,
                 weight: float,
                 name: str,
                 description: str,
                 cooking_time_minutes: int,
                 id: Optional[int] = None,
                 **kwargs):
        super().__init__(
            id=id,
            category_id=category_id,
            price=price,
            name=name,
            description=description,
            cooking_time_minutes=cooking_time_minutes,
            weight=weight,
            **kwargs
        )
