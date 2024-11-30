import asyncio
import threading
from flask import Blueprint, request, jsonify
from pydantic import ValidationError

from Model.NotificationGetter import NotificationGetter
from tg_notification_bot.NotificationRequestClient.NotificationRequest import send_notification_request

notification_bp = Blueprint("notificationgetter", __name__)


def run_async_task(coroutine):
    loop = asyncio.new_event_loop()
    asyncio.set_event_loop(loop)
    loop.run_until_complete(coroutine)


@notification_bp.route("/", methods=["POST"])
def handle_notification_getter():
    try:
        data = request.json
        notification_getter = NotificationGetter(**data)
        return jsonify(notification_getter.dict()), 200

        threading.Thread(target=run_async_task, args=(send_notification_request(notification_getter),)).start()

    except ValidationError as e:
        return jsonify({"error": e.errors()}), 400
