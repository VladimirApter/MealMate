from typing import List
from flask import Blueprint, request, jsonify
from pydantic import ValidationError, parse_obj_as
from Model.OrderItem import OrderItem
from Model.WaiterCall import WaiterCall
from tg_notification_bot.SendWaiterCall.SendWaiterCall import send_waiter_call

waiter_call_bp = Blueprint("waitercall", __name__)


@waiter_call_bp.route("/", methods=["POST"])
def handle_order():
    try:
        waiter_data = request.json

        waiter_call = WaiterCall(**waiter_data)

        send_waiter_call(waiter_call)

        return jsonify(waiter_call.dict()), 200
    except ValidationError as e:
        return jsonify({"error": e.errors()}), 400
