from pydantic import BaseModel

from API.tg_bots.Model.MenuItem import MenuItem


class Dish(MenuItem, BaseModel):
    weight: float
