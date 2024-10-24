from pydantic import BaseModel
from typing import Optional


class Table(BaseModel):
    id: Optional[int]
    restaurant_id: int
    number: int

    def __init__(self,
                 restaurant_id: int,
                 number: int,
                 id: Optional[int] = None):
        super().__init__(
            id=id,
            restaurant_id=restaurant_id,
            number=number
        )
