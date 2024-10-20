from TgAccount import TgAccount
from typing import List


class Owner(TgAccount):
    restaurant_ids: List[int]
