from flask import Flask
from controllers.order_controller import order_bp
from controllers.notification_getter_controller import notification_bp

app = Flask(__name__)

# Регистрация blueprints
app.register_blueprint(order_bp, url_prefix="/order")
app.register_blueprint(notification_bp, url_prefix="/notificationgetter")

if __name__ == "__main__":
    app.run(host='localhost', port=5059, debug=True)