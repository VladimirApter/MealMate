import httpx
from threading import Lock
from typing import Any

api_base_url = os.getenv("API_URL", None)
if api_base_url is None:
    api_base_url = "http://localhost:5051"


class ApiClient:
    def __init__(self, obj_class):
        self.obj_class = obj_class
        self.endpoint = obj_class.__name__.lower().split('.')[-1]
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
                    # obj = self.obj_class.parse_obj(data)
                    obj = self.obj_class.model_validate(data)
                    return obj
                else:
                    return None

    def post(self, obj):
        with self.lock:
            with httpx.Client() as client:
                data = obj.dict(by_alias=True)

                if 'date_time' in data:
                    data['date_time'] = data['date_time'].isoformat()
                if 'status' in data:
                    data['status'] = int(data['status'])

                response = client.post(self._get_url(), json=data, headers={'Content-Type': 'application/json'})
                if response.status_code == 200:
                    obj_id = int(response.json())
                    return obj_id
                else:
                    response.raise_for_status()

    def delete(self, id: int):
        with self.lock:
            with httpx.Client() as client:
                response = client.delete(self._get_url(id))
                if response.status_code in {200, 204}:
                    return True
                else:
                    response.raise_for_status()

    @staticmethod
    def _to_dict(obj: Any) -> Any:
        if isinstance(obj, list):
            return [ApiClient._to_dict(item) for item in obj]
        elif isinstance(obj, dict):
            return {key: ApiClient._to_dict(value) for key, value in obj.items()}
        elif hasattr(obj, '__dict__'):
            return {key: ApiClient._to_dict(value) for key, value in obj.__dict__.items()}
        else:
            return obj
