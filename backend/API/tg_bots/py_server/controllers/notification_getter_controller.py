from flask import Blueprint, request, jsonify
from pydantic import ValidationError
from API.tg_bots.Model.NotificationGetter import NotificationGetter

notification_bp = Blueprint("notification", __name__)


@notification_bp.route("/", methods=["POST"])
def handle_notification():
    try:
        data = request.json
        notification_getter = NotificationGetter(**data)

        return jsonify(notification_getter.dict()), 200
    except ValidationError as e:
        return jsonify({"error": e.errors()}), 400
