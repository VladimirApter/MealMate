from pydantic import BaseModel

from Model.MenuItem import MenuItem


class Drink(MenuItem, BaseModel):
    volume: float
