from pydantic import BaseModel


class TgAccount(BaseModel):
    id: None = None
    username: str
