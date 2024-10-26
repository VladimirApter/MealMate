from pydantic import BaseModel
from typing import Optional

from Models.MenuItem import MenuItem


class Drink(MenuItem):
    volume: float

    def __init__(self,
                 category_id: int,
                 price: float,
                 volume: float,
                 name: str,
                 description: str,
                 cooking_time_minutes: int,
                 id: Optional[int] = None):
        super().__init__(
            id=id,
            category_id=category_id,
            price=price,
            name=name,
            description=description,
            cooking_time_minutes=cooking_time_minutes,
            weight=volume
        )