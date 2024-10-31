import httpx
from threading import Lock

from Config import *


class ApiClient:
    def __init__(self, obj_class):
        self.obj_class = obj_class
        self.endpoint = obj_class.__name__.lower()
        self.lock = Lock()

    def _get_url(self, id=None):
        if id is None:
            return f"{api_base_url}/api/{self.endpoint}"
        else:
            return f"{api_base_url}/api/{self.endpoint}/{id}"

    def get(self, id: int):
        with self.lock:
            with httpx.Client() as client:
                response = client.get(self._get_url(id))
                if response.status_code == 200:
                    data = response.json()
                    obj = self.obj_class.parse_obj(data)
                    return obj
                else:
                    return None

    def post(self, obj):
        with self.lock:
            with httpx.Client() as client:
                response = client.post(self._get_url(), json=obj.dict(by_alias=True), headers={'Content-Type': 'application/json'})
                if response.status_code == 200:
                    obj_id = int(response.json())
                    return obj_id
                else:
                    response.raise_for_status()
