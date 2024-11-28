from pydantic import BaseModel
from typing import Optional, Union


from Model.Drink import Drink
from Model.Dish import Dish


class OrderItem(BaseModel):
    id: Optional[int]
    order_id: int
    menu_item_id: int
    menu_item: Union[Dish, Drink]
    count: int
    price: float

    def __init__(self,
                 menu_item_id: int,
                 order_id: int,
                 menu_item: Union[Dish, Drink],
                 count: int,
                 price: float,
                 id: Optional[int] = None):
        super().__init__(
            id=id,
            menu_item_id=menu_item_id,
            order_id=order_id,
            menu_item=menu_item,
            count=count,
            price=price,
        )
