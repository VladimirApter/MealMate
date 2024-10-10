from pydantic import BaseModel
from typing import List
from datetime import datetime
from enum import Enum

from Models.OrderItem import OrderItem


class OrderStatus(Enum):
    IN_ASSEMBLY = "InAssembly"
    COOKING = "Cooking"
    DONE = "Done"


class Order(BaseModel):
    id: int = None
    client_id: int = None
    restaurant_id: int = None
    order_items: List[OrderItem]
    comment: str
    date_time: datetime
    status: OrderStatus = OrderStatus.IN_ASSEMBLY

