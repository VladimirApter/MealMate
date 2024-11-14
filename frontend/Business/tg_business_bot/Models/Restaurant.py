from pydantic import BaseModel
from typing import List, Optional

from Models.NotificationGetter import NotificationGetter
from Models.Menu import Menu
from Models.Table import Table
from Models.GeoCoordinates import GeoCoordinates

class Restaurant(BaseModel):
    id: Optional[int]
    owner_id: int
    notification_getter: Optional[NotificationGetter]
    name: str
    coordinates: GeoCoordinates
    menu: Optional[Menu]
    tables: Optional[List[Table]]

    def __init__(self,
                 owner_id: int,
                 name: str,
                 coordinates: GeoCoordinates,
                 id: Optional[int] = None,
                 notification_getter: Optional[NotificationGetter] = None,
                 menu: Optional[Menu] = None,
                 tables: Optional[List[Table]] = None):
        super().__init__(
            id=id,
            owner_id=owner_id,
            notification_getter=notification_getter,
            name=name,
            coordinates=coordinates,
            menu=menu,
            tables=tables
        )
