from typing import Optional

from pydantic import BaseModel, field_validator
from Model.Nutrients import Nutrients


class MenuItem(BaseModel):
    id: Optional[int]
    category_id: int
    cooking_time_minutes: int
    price: float
    name: str
    description: str
    image_path: Optional[str]
    nutrients_of_100_grams: Optional[Nutrients]

    def __init__(self,
                 category_id: int,
                 price: float,
                 name: str,
                 description: str,
                 cooking_time_minutes: int,
                 image_path: Optional[str] = None,
                 nutrients_of_100_grams: Optional[Nutrients] = None,
                 id: Optional[int] = None,
                 **kwargs):
        super().__init__(
            id=id,
            category_id=category_id,
            price=price,
            name=name,
            description=description,
            image_path=image_path,
            cooking_time_minutes=cooking_time_minutes,
            nutrients_of_100_grams=nutrients_of_100_grams,
            **kwargs
        )

    @field_validator('__all__', mode='before', check_fields=False)
    def determine_type(cls, values):
        from API.tg_bots.Model.Dish import Dish
        from API.tg_bots.Model.Drink import Drink

        if 'weight' in values:
            return Dish(**values)
        elif 'volume' in values:
            return Drink(**values)
        return values
