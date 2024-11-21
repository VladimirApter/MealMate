from pydantic import BaseModel
from typing import List, Optional


class GeoCoordinates(BaseModel):
    id: Optional[int]
    restaurant_id: int
    latitude: float
    longitude: float

    def __init__(self,
                 latitude: float,
                 longitude: float,
                 restaurant_id: int,
                 id: Optional[int] = None):
        super().__init__(
            id=id,
            restaurant_id=restaurant_id,
            latitude=latitude,
            longitude=longitude
        )

    def __str__(self):
        return f"({self.latitude}, {self.longitude})"
