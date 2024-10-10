from pydantic import BaseModel
from typing import List

from NotificationGetter import NotificationGetter
from Owner import Owner
from Menu import Menu
from Table import Table


class Restaurant(BaseModel):
    id: int = None
    owner_id: int = None
    owner: Owner
    notification_getter: NotificationGetter
    name: str
    address: str
    menu: Menu
    tables: List[Table]
