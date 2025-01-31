from pydantic import BaseModel, parse_obj_as, field_validator
from typing import List, Optional
from datetime import datetime
from enum import Enum

from Model.OrderItem import OrderItem
from Model.Client import Client


class OrderStatus(Enum):
    IN_ASSEMBLY = 0
    COOKING = 1
    DONE = 2
    CANCELLED = 3


class Order(BaseModel):
    id: Optional[int]
    client_id: int
    table_id: int
    order_items: List[OrderItem]
    comment: Optional[str]
    date_time: datetime
    status: OrderStatus
    cooking_time_minutes: float
    client: Client
    price: float

    class Config:
        use_enum_values = True

    def __init__(self,
                 client_id: int,
                 table_id: int,
                 order_items: List[OrderItem],
                 comment: Optional[str],
                 date_time: datetime,
                 status: OrderStatus,
                 client: Client,
                 cooking_time_minutes: float,
                 price: float,
                 id: Optional[int] = None):
        order_items = parse_obj_as(List[OrderItem], order_items)
        super().__init__(
            id=id,
            client_id=client_id,
            table_id=table_id,
            price=price,
            order_items=order_items,
            comment=comment,
            date_time=date_time,
            status=status,
            client=client,
            cooking_time_minutes=cooking_time_minutes
        )
