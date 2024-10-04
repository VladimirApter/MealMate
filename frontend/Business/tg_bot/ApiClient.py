import requests
import json


class ApiClient:
    def __init__(self, base_url, obj_class):
        self.base_url = base_url
        self.obj_class = obj_class
        self.endpoint = obj_class.__name__.lower()

    def get_url(self):
        return f"{self.base_url}/api/{self.endpoint}"

    def get(self):
        response = requests.get(self.get_url())
        if response.status_code == 200:
            data = response.json()
            obj = self.obj_class.parse_obj(data)
            return obj
        else:
            response.raise_for_status()

    def create(self, obj):
        response = requests.post(self.get_url(), data=json.dumps(obj.dict(by_alias=True)), headers={'Content-Type': 'application/json'})
        if response.status_code == 201:
            data = response.json()
            obj = self.obj_class.parse_obj(data)
            return obj
        else:
            response.raise_for_status()
