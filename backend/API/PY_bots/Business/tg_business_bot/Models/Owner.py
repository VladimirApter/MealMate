from typing import List, Optional
from tg_business_bot.Models.TgAccount import TgAccount


class Owner(TgAccount):
    restaurant_ids: Optional[List[int]]

    def __init__(self,
                 id: int,
                 username: str,
                 restaurant_ids: Optional[List[int]] = None,
                 **kwargs):
        super().__init__(id=id,
                         username=username,
                         restaurant_ids=restaurant_ids,
                         **kwargs)
