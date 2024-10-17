import requests

from Config import geocoder_api_key


def get_coordinates(address):
    base_url = "https://geocode-maps.yandex.ru/1.x/"
    params = {
        "apikey": geocoder_api_key,
        "geocode": address,
        "format": "json"
    }

    response = requests.get(base_url, params=params)
    data = response.json()

    if response.status_code == 200:
        found = int(data["response"]["GeoObjectCollection"]["metaDataProperty"]["GeocoderResponseMetaData"]["found"])
        if found > 0:
            coordinates = data["response"]["GeoObjectCollection"]["featureMember"][0]["GeoObject"]["Point"]["pos"]
            lon, lat = map(float, coordinates.split())
            return lat, lon
        else:
            return None
    else:
        return None
