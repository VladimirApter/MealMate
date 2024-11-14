from pydantic import BaseModel
from typing import Optional


class Table(BaseModel):
    id: Optional[int]
    restaurant_id: int
    number: int
    token: Optional[str]
    qr_code_image_path: Optional[str]

    def __init__(self,
                 restaurant_id: int,
                 number: int,
                 qr_code_image_path: Optional[str] = None,
                 token: Optional[str] = None,
                 id: Optional[int] = None):
        super().__init__(
            id=id,
            restaurant_id=restaurant_id,
            number=number,
            qr_code_image_path=qr_code_image_path,
            token=token
        )
