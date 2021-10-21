# Messenger
Messenger using SignalR and Blazor Server side

We use SignalR in our Blazor server side application and store all messages in database.

To send a message to specific user we use ConnectionId instead User property and then store the connection id in our database.
This is a way create a messenger and online status of users.
