from pydantic import BaseModel
from typing import Optional


class Dish(BaseModel):
    id: Optional[int]
    category_id: int
    price: float
    weight: float
    name: str
    description: str
    cooking_time_minutes: int

    def __init__(self,
                 category_id: int,
                 price: float,
                 weight: float,
                 name: str,
                 description: str,
                 cooking_time_minutes: int,
                 id: Optional[int] = None):
        super().__init__(
            id=id,
            category_id=category_id,
            price=price,
            weight=weight,
            name=name,
            description=description,
            cooking_time_minutes=cooking_time_minutes
        )
