from typing import Optional
from Models.TgAccount import TgAccount


class NotificationGetter(TgAccount):
    restaurant_id: int

    def __init__(self,
                 restaurant_id: int,
                 id: Optional[int] = None,
                 username: Optional[str] = None,
                 **kwargs):
        super().__init__(id=id,
                         username=username,
                         restaurant_id=restaurant_id,
                         **kwargs)
