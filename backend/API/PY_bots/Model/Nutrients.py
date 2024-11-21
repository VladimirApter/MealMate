from pydantic import BaseModel
from typing import Optional


class Nutrients(BaseModel):
    id: Optional[int]
    menu_item_id: int
    kilocalories: Optional[int]
    proteins: Optional[int]
    fats: Optional[int]
    carbohydrates: Optional[int]

    def __init__(self,
                 menu_item_id: int,
                 kilocalories: Optional[int] = None,
                 proteins: Optional[int] = None,
                 fats: Optional[int] = None,
                 carbohydrates: Optional[int] = None,
                 id: Optional[int] = None):
        super().__init__(
            kilocalories=kilocalories,
            proteins=proteins,
            fats=fats,
            carbohydrates=carbohydrates,
            menu_item_id=menu_item_id,
            id=id
        )