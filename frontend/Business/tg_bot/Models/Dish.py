from pydantic import BaseModel


class Dish(BaseModel):
    id: int = None
    category_id: int = None
    price: float
    weight: float
    name: str
    description: str
    cooking_time_minutes: int
