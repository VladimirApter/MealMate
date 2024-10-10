from pydantic import BaseModel

from Dish import Dish


class OrderItem(BaseModel):
    id: int = None
    order_id: int = None
    dish_id: int = None
    dish: Dish
    count: int
    price: float
