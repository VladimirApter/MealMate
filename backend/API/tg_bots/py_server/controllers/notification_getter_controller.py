import threading
from flask import Blueprint, request, jsonify
from pydantic import ValidationError
from Model.NotificationGetter import NotificationGetter
from tg_notification_bot.NotificationRequestClient.NotificationRequest import send_notification

notification_getter_bp = Blueprint("notificationgetter", __name__)


@notification_getter_bp.route("/", methods=["POST"])
def handle_notification_getter():
    try:
        data = request.json
        notification_getter = NotificationGetter(**data)

        threading.Thread(target=send_notification, args=(notification_getter,)).start()

        return jsonify(notification_getter.dict()), 200
    except ValidationError as e:
        return jsonify({"error": e.errors()}), 400
