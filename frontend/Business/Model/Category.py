from pydantic import BaseModel
from typing import List, Optional

from Model.MenuItem import MenuItem


class Category(BaseModel):
    id: Optional[int]
    menu_id: int
    name: str
    menu_items: Optional[List[MenuItem]]

    def __init__(self,
                 menu_id: int,
                 name: str,
                 id: Optional[int] = None,
                 menu_items: Optional[List[MenuItem]] = None):
        super().__init__(
            id=id,
            menu_id=menu_id,
            name=name,
            menu_items=menu_items
        )
