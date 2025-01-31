from pydantic import BaseModel

from Model.MenuItem import MenuItem


class Dish(MenuItem, BaseModel):
    weight: float
