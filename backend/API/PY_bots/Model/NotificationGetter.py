from Model.TgAccount import TgAccount


class NotificationGetter(TgAccount):
    restaurant_id: int

    def __init__(self,
                 id: int,
                 username: str,
                 restaurant_id: int,
                 **kwargs):
        super().__init__(id=id,
                         username=username,
                         restaurant_id=restaurant_id,
                         **kwargs)
