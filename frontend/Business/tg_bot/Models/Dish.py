from pydantic import BaseModel


class Dish(BaseModel):
    id: int
    price: float
    weight: float
    name: str
    description: str
    cooking_time_minutes: int
