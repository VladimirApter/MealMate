from flask import Blueprint, request, jsonify
from pydantic import ValidationError
from API.tg_bots.Model.Order import Order

order_bp = Blueprint("order", __name__)


@order_bp.route("/", methods=["POST"])
def handle_order():
    try:
        order_data = request.json
        order = Order(**order_data)

        return jsonify(order.dict()), 200
    except ValidationError as e:
        return jsonify({"error": e.errors()}), 400