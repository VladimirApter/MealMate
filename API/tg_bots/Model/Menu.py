from pydantic import BaseModel
from typing import List, Optional

from Model.Category import Category


class Menu(BaseModel):
    id: Optional[int]
    restaurant_id: int
    excel_table_path: Optional[str]
    categories: Optional[List[Category]]

    def __init__(self,
                 restaurant_id: int,
                 id: Optional[int] = None,
                 excel_table_path: Optional[str] = None,
                 categories: Optional[List[Category]] = None):
        super().__init__(
            id=id,
            restaurant_id=restaurant_id,
            excel_table_path=excel_table_path,
            categories=categories
        )
