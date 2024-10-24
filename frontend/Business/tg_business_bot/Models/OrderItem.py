from pydantic import BaseModel
from typing import Optional

from Models.Dish import Dish


class OrderItem(BaseModel):
    id: Optional[int]
    order_id: int
    dish_id: int
    dish: Dish
    count: int
    price: float

    def __init__(self,
                 order_id: int,
                 dish_id: int,
                 dish: Dish,
                 count: int,
                 price: float,
                 id: Optional[int] = None):
        super().__init__(
            id=id,
            order_id=order_id,
            dish_id=dish_id,
            dish=dish,
            count=count,
            price=price
        )
