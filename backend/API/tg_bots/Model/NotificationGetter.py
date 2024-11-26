from Model.TgAccount import TgAccount


class NotificationGetter(TgAccount):
    restaurant_id: int
    is_blocked: bool

    def __init__(self,
                 id: int,
                 username: str,
                 restaurant_id: int,
                 is_blocked: bool = False,
                 **kwargs):
        super().__init__(id=id,
                         username=username,
                         restaurant_id=restaurant_id,
                         is_blocked=is_blocked,
                         **kwargs)
