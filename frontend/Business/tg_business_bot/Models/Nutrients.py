from pydantic import BaseModel
from typing import Optional


class Nutrients(BaseModel):
    kilocalories: Optional[int]
    proteins: Optional[int]
    fats: Optional[int]
    carbohydrates: Optional[int]

    def __init__(self,
                 kilocalories: Optional[int] = None,
                 proteins: Optional[int] = None,
                 fats: Optional[int] = None,
                 carbohydrates: Optional[int] = None):
        super().__init__(
            kilocalories=kilocalories,
            proteins=proteins,
            fats=fats,
            carbohydrates=carbohydrates
        )