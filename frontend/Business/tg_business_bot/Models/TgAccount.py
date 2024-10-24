from typing import Optional
from pydantic import BaseModel


class TgAccount(BaseModel):
    id: Optional[int]
    username: str

    def __init__(self,
                 username: str,
                 id: Optional[int] = None,
                 **kwargs):
        super().__init__(id=id,
                         username=username,
                         **kwargs)
