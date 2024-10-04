from pydantic import BaseModel


class Dish(BaseModel):
    name: str
    description: str
    price: float
    weight: float
