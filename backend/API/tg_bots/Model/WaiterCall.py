from pydantic import BaseModel
from typing import Optional
from datetime import datetime
from enum import Enum
from Model.Client import Client


class WaiterCallStatus(Enum):
    IN_ASSEMBLY = 0
    DONE = 1


class WaiterCall(BaseModel):
    id: Optional[int]
    client_id: int
    table_id: int
    date_time: datetime
    status: WaiterCallStatus
    client: Client

    class Config:
        use_enum_values = True

    def __init__(self,
                 client_id: int,
                 table_id: int,
                 date_time: datetime,
                 status: WaiterCallStatus,
                 client: Client,
                 id: Optional[int] = None):
        super().__init__(
            id=id,
            client_id=client_id,
            table_id=table_id,
            date_time=date_time,
            status=status,
            client=client
        )
