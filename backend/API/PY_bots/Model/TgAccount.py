from typing import Optional
from pydantic import BaseModel


class TgAccount(BaseModel):
    id: int
    username: str

    def __init__(self,
                 id: int,
                 username: str,
                 **kwargs):
        super().__init__(id=id,
                         username=username,
                         **kwargs)
