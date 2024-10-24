from typing import List, Optional
from Models.TgAccount import TgAccount


class Owner(TgAccount):
    restaurant_ids: Optional[List[int]]

    def __init__(self,
                 username: str,
                 id: Optional[int] = None,
                 restaurant_ids: Optional[List[int]] = None,
                 **kwargs):
        super().__init__(id=id,
                         username=username,
                         restaurant_ids=restaurant_ids,
                         **kwargs)
