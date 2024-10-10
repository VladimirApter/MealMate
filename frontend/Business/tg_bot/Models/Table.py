from pydantic import BaseModel


class Table(BaseModel):
    id: int = None
    restaurant_id: int = None
    number: int
