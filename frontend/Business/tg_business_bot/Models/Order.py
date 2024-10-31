from pydantic import BaseModel
from typing import List, Optional
from datetime import datetime
from enum import Enum

from tg_business_bot.Models.OrderItem import OrderItem


class OrderStatus(Enum):
    IN_ASSEMBLY = "InAssembly"
    COOKING = "Cooking"
    DONE = "Done"


class Order(BaseModel):
    id: Optional[int]
    client_id: int
    restaurant_id: int
    order_items: List[OrderItem]
    comment: str
    date_time: datetime
    status: OrderStatus

    def __init__(self,
                 client_id: int,
                 restaurant_id: int,
                 order_items: List[OrderItem],
                 comment: str,
                 date_time: datetime,
                 status: OrderStatus,
                 id: Optional[int] = None):
        super().__init__(
            id=id,
            client_id=client_id,
            restaurant_id=restaurant_id,
            order_items=order_items,
            comment=comment,
            date_time=date_time,
            status=status
        )
