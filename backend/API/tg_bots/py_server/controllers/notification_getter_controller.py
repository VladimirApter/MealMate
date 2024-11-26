from flask import Blueprint, request, jsonify
from pydantic import ValidationError

from Model.NotificationGetter import NotificationGetter
from tg_notification_bot.NotificationRequestClient.NotificationRequest import send_notification_request
from tg_notification_bot.NotificationRequestClient.Config import client
from ApiClient.ApiClient import ApiClient

from telethon.errors import UserPrivacyRestrictedError

notification_bp = Blueprint("notificationgetter", __name__)


@notification_bp.route("/", methods=["POST"])
def handle_notification():
    try:
        data = request.json
        notification_getter = NotificationGetter(**data)

        try:
            with client:
                client.loop.run_until_complete(
                    send_notification_request(notification_getter)
                )
        except UserPrivacyRestrictedError:
            notification_getter.is_blocked = True
            api_client = ApiClient(NotificationGetter)
            api_client.post(notification_getter)

        return jsonify(notification_getter.dict()), 200
    except ValidationError as e:
        return jsonify({"error": e.errors()}), 400
