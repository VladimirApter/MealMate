from pydantic import BaseModel
from typing import List, Optional

from tg_business_bot.Models.Dish import Dish


class Category(BaseModel):
    id: Optional[int]
    menu_id: int
    name: str
    dishes: Optional[List[Dish]]

    def __init__(self,
                 menu_id: int,
                 name: str,
                 id: Optional[int] = None,
                 dishes: Optional[List[Dish]] = None):
        super().__init__(
            id=id,
            menu_id=menu_id,
            name=name,
            dishes=dishes
        )
