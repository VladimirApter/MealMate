import os
import threading

from flask import Flask
from py_server.controllers.order_controller import order_bp
from controllers.notification_getter_controller import notification_bp
from tg_notification_bot.main import start_bot

app = Flask(__name__)

# Регистрация blueprints
app.register_blueprint(order_bp, url_prefix="/order")
app.register_blueprint(notification_bp, url_prefix="/notificationgetter")

if __name__ == "__main__":
    threading.Thread(target=lambda: app.run(host="0.0.0.0", port=5059)).start()

    start_bot()
