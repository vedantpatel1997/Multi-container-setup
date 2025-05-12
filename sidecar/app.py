from flask import Flask, request, render_template_string
import logging
import os

app = Flask(__name__)

logging.basicConfig(level=logging.INFO)
logger = logging.getLogger(__name__)

# Default route for the startup page
@app.route('/')
def home():
    return render_template_string('''
        <!DOCTYPE html>
        <html lang="en">
        <head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <title>Welcome to the Sidecar App</title>
            <style>
                body {
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    display: flex;
                    justify-content: center;
                    align-items: center;
                    height: 100vh;
                    margin: 0;
                }
                .container {
                    text-align: center;
                    background-color: #fff;
                    padding: 20px;
                    border-radius: 8px;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                }
                h1 {
                    color: #333;
                }
                p {
                    color: #666;
                }
            </style>
        </head>
        <body>
            <div class="container">
                <h1>Welcome to the Sidecar App</h1>
                <p>This is the default startup page.</p>
                <p>Version: <strong>v1.0</strong></p>
            </div>
        </body>
        </html>
    ''')

@app.route('/log', methods=['POST'])
def log_request():
    data = request.json
    logger.info(f"Received request sidecar 1 v1.0: {data}")
    return "Logged", 200

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5001)
