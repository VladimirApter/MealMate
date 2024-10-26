from typing import Optional

from pydantic import BaseModel


class MenuItem(BaseModel):
    id: Optional[int]
    category_id: int
    price: float
    name: str
    description: str
    cooking_time_minutes: int

    def __init__(self,
                 category_id: int,
                 price: float,
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
            **kwargs
        )