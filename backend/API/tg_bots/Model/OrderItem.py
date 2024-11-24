from pydantic import BaseModel
from typing import Optional, Union

from Model.MenuItem import MenuItem
from Model.Drink import Drink
from Model.Dish import Dish
from Model.Client import Client


class OrderItem(BaseModel):
    id: Optional[int]
    order_id: int
    menu_item: Union[Dish, Drink]
    count: int
    price: float

    def __init__(self,
                 order_id: int,
                 menu_item: Union[Dish, Drink],
                 count: int,
                 price: float,
                 id: Optional[int] = None):
        super().__init__(
            id=id,
            order_id=order_id,
            menu_item=menu_item,
            count=count,
            price=price,
        )
