# Messenger
Messenger using SignalR and Blazor Server side

We use SignalR in our Blazor server side application and store all messages in database.

To send a message to specific user we use ConnectionId instead of User property and then store the connection id in our database.
This is a way create a messenger and online status of users.
Based on below link:
https://docs.microsoft.com/en-us/aspnet/signalr/overview/guide-to-the-api/mapping-users-to-connections

The best approach for your application depends on:

1- The number of web servers hosting your application.
2- Whether you need to get a list of the currently connected users.
3- Whether you need to persist group and user information when the application or server restarts.
4- Whether the latency of calling an external server is an issue.

                           || More than one server ||	Get list of currently connected users || Persist information after restarts || Optimal performance
                           ||                      ||                                       ||                                    ||
                           ||                      ||                                       ||                                    ||
UserID Provider            ||           X          ||                                       ||                                    ||      X
                           ||                      ||                                       ||                                    ||
                           ||                      ||                  X                    ||                                    ||      X
In-memory                  ||                      ||                                       ||                                    ||
                           ||                      ||                                       ||                                    ||
                           ||                      ||                                       ||                                    ||
Single-user groups         ||            X         ||                                       ||                                    ||      X
                           ||                      ||                                       ||                                    ||
                           ||                      ||                                       ||                                    ||  
Permanent, external        ||            X         ||                  X                    ||                    X               ||


We use fourth way and use SQL Server to store all the information that we need for our application, for instance, we can mention Connection Id or Senderuserid and Recieveruserid and then we can send information of connection to Sender and Reciever user id.
