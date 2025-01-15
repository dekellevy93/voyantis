# Voyantis Message Queue App

This project implements a simple message queue system with a backend API built in C# (ASP.NET Core Minimal API) and a frontend UI built in Vue.js.

## Frontend Functionality

The frontend provides a user-friendly interface for interacting with the message queues. It allows users to:

* **Post Messages:**  Enter a queue name and a message to post to the specified queue.
* **Get Messages (Default Timeout):** Retrieve a message from a specified queue using a default timeout of 10 seconds.
* **Get Messages (Specific Timeout):** Retrieve a message from a specified queue with a user-defined timeout (in milliseconds).
* **Get Queues List:** Retrieve a list of all available queues and their current message counts.

The Vue.js app handles user input, makes API requests to the backend, and displays the responses dynamically. Error handling and feedback messages are provided to guide the user.

## Backend API

The backend exposes a REST API for managing the message queues.  It provides the following endpoints:

* **POST /api/queues/{queue_name}:**
    * Posts a message to the specified queue.
    * Expects the message in the request body as a JSON string.
* **GET /api/queues/{queue_name}?timeout={ms}:**
    * Retrieves the next message from the specified queue.
    * Supports an optional `timeout` parameter in milliseconds (default is 10000ms, or 10 seconds).
    * Returns a 204 No Content status if no message is available after the timeout.
* **GET /api/queues/list:**
    * Retrieves a list of all available queues and their message counts.

The backend uses an in-memory `ConcurrentDictionary` to store the queues, providing thread-safe access.  For a production-ready application, a persistent queueing system like Redis or RabbitMQ would be recommended.


## Installation and Running the App

### Backend

1. Open the C# project in Visual Studio Code or Visual Studio.
2. Ensure you have the .NET SDK installed.
3. Run the backend application by pressing `F5` to begin debugging (play button in vs code)  This will start the server on `http://localhost:5170/`.

### Frontend

1. Open a terminal in the Vue.js project directory.
2. Install the project dependencies:
   ```bash
   npm install 
   
1. Start the development server:

npm run serve

This will usually launch the app on http://localhost:8080/. The backend is configured with CORS to allow requests from this specific address during development.

Additional Notes:
This implementation provides a basic but functional message queue system. Further enhancements could include persistent storage, authentication/authorization, and more advanced queue management features.

Future Development
Here are some potential improvements for the future:

Persistent Queues: Replace the in-memory queues with a persistent solution like Redis, RabbitMQ or other database

Authentication and Authorization: Add security measures to control access to the API.

Enhanced UI: Improve the user interface with more advanced features and styling.

Client-side input validation: Limit the text that can be entered in the inputs.

This improved README provides more comprehensive documentation, including installation instructions, API details, and suggestions for future development.  You can further enhance it by adding screenshots, examples of API requests and responses, and more specific details about your application's design and architecture.