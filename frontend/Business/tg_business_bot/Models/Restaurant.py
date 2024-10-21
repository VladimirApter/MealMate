from pydantic import BaseModel
from typing import List

from NotificationGetter import NotificationGetter
from Menu import Menu
from Table import Table


class Restaurant(BaseModel):
    id: int = None
    notification_getter: NotificationGetter
    name: str
    address: str
    menu: Menu
    tables: List[Table]
