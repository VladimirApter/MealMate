from pydantic import BaseModel
from typing import List, Optional

from tg_business_bot.Models.NotificationGetter import NotificationGetter
from tg_business_bot.Models.Menu import Menu
from tg_business_bot.Models.Table import Table


class Restaurant(BaseModel):
    id: Optional[int]
    owner_id: int
    notification_getter: Optional[NotificationGetter]
    name: str
    address: str
    menu: Optional[Menu]
    tables: Optional[List[Table]]

    def __init__(self,
                 owner_id: int,
                 name: str,
                 address: str,
                 id: Optional[int] = None,
                 notification_getter: Optional[NotificationGetter] = None,
                 menu: Optional[Menu] = None,
                 tables: Optional[List[Table]] = None):
        super().__init__(
            id=id,
            owner_id=owner_id,
            notification_getter=notification_getter,
            name=name,
            address=address,
            menu=menu,
            tables=tables
        )
