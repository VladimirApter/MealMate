from pydantic import BaseModel


class Table(BaseModel):
    id: int
    number: int
