from pydantic import BaseModel
from typing import List

from Models.NotificationGetter import NotificationGetter
from Models.Menu import Menu
from Models.Table import Table


class Restaurant(BaseModel):
    id: int = None
    notification_getter: NotificationGetter
    name: str
    address: str
    menu: Menu
    tables: List[Table]
