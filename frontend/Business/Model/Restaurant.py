from pydantic import BaseModel
from typing import List, Optional

from Model.NotificationGetter import NotificationGetter
from Model.Menu import Menu
from Model.Table import Table
from Model.GeoCoordinates import GeoCoordinates


class Restaurant(BaseModel):
    id: Optional[int]
    owner_id: int
    notification_getter: Optional[NotificationGetter]
    name: str
    coordinates: Optional[GeoCoordinates]
    menu: Optional[Menu]
    tables: Optional[List[Table]]

    def __init__(self,
                 owner_id: int,
                 name: str,
                 id: Optional[int] = None,
                 coordinates: Optional[GeoCoordinates] = None,
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
