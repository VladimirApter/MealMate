from typing import List
from flask import Blueprint, request, jsonify
from pydantic import ValidationError, parse_obj_as
from Model.OrderItem import OrderItem
from Model.Order import Order
from tg_notification_bot.SendOrder.SendOrder import send_order

order_bp = Blueprint("order", __name__)


@order_bp.route("/", methods=["POST"])
def handle_order():
    try:
        order_data = request.json
        #order_data.pop("cooking_time", None)

        order_data["order_items"] = parse_obj_as(List[OrderItem], order_data["order_items"])

        order = Order(**order_data)

        send_order(order)

        return jsonify(order.dict()), 200
    except ValidationError as e:
        return jsonify({"error": e.errors()}), 400
