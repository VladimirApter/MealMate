from pydantic import BaseModel

from API.tg_bots.Model.MenuItem import MenuItem


class Drink(MenuItem, BaseModel):
    volume: float
