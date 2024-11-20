from pydantic import BaseModel
from typing import Optional


class Client(BaseModel):
    id: Optional[int]
    ip: str

    def __init__(self,
                 ip: str,
                 id: Optional[int] = None):
        super().__init__(id=id,
                         ip=ip)
