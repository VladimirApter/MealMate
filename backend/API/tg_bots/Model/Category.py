from pydantic import BaseModel
from typing import List, Optional, Union

from Model.MenuItem import MenuItem
from Model.Dish import Dish
from Model.Drink import Drink


class Category(BaseModel):
    id: Optional[int]
    menu_id: int
    name: str
    menu_items: Optional[List[Union[Dish, Drink]]]

    def __init__(self,
                 menu_id: int,
                 name: str,
                 id: Optional[int] = None,
                 menu_items: Optional[List[Union[Dish, Drink]]] = None):
        super().__init__(
            id=id,
            menu_id=menu_id,
            name=name,
            menu_items=menu_items
        )
