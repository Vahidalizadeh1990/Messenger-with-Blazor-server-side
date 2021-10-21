using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using PrivateMessenger.Data;
using PrivateMessenger.Models.Interface;
using PrivateMessenger.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using System.Threading;
using Microsoft.AspNetCore.Components.Server.Circuits;
using Microsoft.JSInterop;
using System.Web;



namespace PrivateMessenger.Pages
{
    public class ChatBase : ComponentBase
    {
        [Parameter]
        public string toUserId { get; set; } // It recieves a message
        public string ToEmail { get; set; } // It belongs to reciever email

        public string FromUserId { get; set; }  // It sends a message
        public string FromEmail { get; set; } // It belongs to sender email

        public string ConSender { get; set; } // This connection belongs to sender 
        public string ConReciever { get; set; } // This connection belongs to reciever

        public string Seen { get; set; } // Seen property
        public bool Online { get; set; } // Online property
        public string Typing { get; set; } // Typing property to show user status 
        
        public HubConnection _hubConnectionMessage; // Hub connection object
        public List<string> messages = new List<string>(); // We use this string object to create a string which it contains our messages
        public string _message { get; set; } // A message that will be send from a user to another user

        // Chat model and interface
        public List<Data.ChatMessage> ChatMessages = new List<Data.ChatMessage>();
        [Inject]
        public ChatMessageInterface _chatMessageInterface { get; set; }
        // Connection model and interface
        public Connection Connection { get; set; } = new Connection();
        [Inject]
        public ConnectionInterface _connectionInterface { get; set; }

        // Use IMapper to map a model to another model
        [Inject]
        public IMapper Mapper { get; set; }
        // it returns information about user from userManager
        [Inject]
        protected UserInformation userInformation { get; set; }
        // Use navigation manager to navigate a user between other components
        [Inject]
        protected NavigationManager NavigationManager { get; set; }
        // We use this chat address bar (url) to reload (refresh) a page 
        protected string ChatAddressBar { get; set; }

        // it returns current user name that is loged in
        [Inject]
        public IHttpContextAccessor httpContextAccessor { get; set; }
        // We use CircuitHandler to handle the life cycle of our component
        [Inject]
        public CircuitHandler circuitHandler { get; set; }

        // We use this interface to invoke some java script file
        [Inject]
        protected IJSRuntime _jsRuntime { get; set; }
        
        // We use startIndex property to set number of messages that we want to load each time 
        private int startIndex = 10;
        // We use loading property to show a spinner when we click on load more button
        public bool Loading { get; set; } = false;

        // We invoke javascript to scroll our component to the bottom of chat page
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
                await _jsRuntime.InvokeAsync<string>("ScrollToBottom", "chatContainer");
        }


        protected async override Task OnInitializedAsync()
        {

            (circuitHandler as CircuitHandlerService).CircuitsChanged +=
                   HandleCircuitsChanged;
            _hubConnectionMessage = new HubConnectionBuilder().WithUrl(NavigationManager.ToAbsoluteUri("/chatHub")).Build();
            ChatAddressBar = NavigationManager.Uri;
            var userName = await userInformation.userInformation(httpContextAccessor.HttpContext.User.Identity.Name);

            var recieverUsername = await userInformation.userInformationById(toUserId);
            ToEmail = recieverUsername.Email;
            FromEmail = userName.Email;
            FromUserId = userName.Id;

            // Retrieve all chats that is related to these users
            var chatInfo = await _chatMessageInterface.GetAllRecieverMessageFromSpecificUser(FromUserId, toUserId);
            var existConnection_SenderWay = _connectionInterface.GetBySenderAndRecieverId(FromUserId, toUserId);
            var existConnection_Reciever = await _connectionInterface.GetByRecieverAndSenderId(FromUserId, toUserId);
            if (chatInfo == null)
            {
                messages = null;
            }
            else if (chatInfo.Count() != 0)
            {
                foreach (var item in chatInfo)
                {
                    // When a user open a chat, all the messages with false seen value will be updated to true seen value
                    if (FromUserId == item.ToUserId)
                    {
                        item.Seen = true;
                        var msg = await _chatMessageInterface.UpdateMessage(item);
                    }
                    //ChatMessages.Add(item);
                }
            }
            // If our connection is disconnect, start it
            if (_hubConnectionMessage.State == HubConnectionState.Disconnected)
            {
                await _hubConnectionMessage.StartAsync();
            }
            // Send FromUserId,Online,toUserId to OnlineSender method in Chat hub
            await _hubConnectionMessage.SendAsync("OnlineSender", FromUserId, Online = true, toUserId);
            // Recieve information from Chat hub
            _hubConnectionMessage.On<string, string, bool, string>("RecieveMessage", async (user, message, seen, date) =>
              {
                  //ChatMessages = chatInfo.ToList();
                  //messages.Clear();
                  if (seen == true)
                  {
                      Seen = "Seen";
                  }
                  else
                  {
                      Seen = "Not Seen";
                  }
                  //var encodingMsg = $"{user} : {message} : {seen}";
                  var encodingMsg = $"{user} : {message}";
                  messages.Add(encodingMsg);
                  // Invoke our javascript to drop a user to bottom of page when click on send button
                  await _jsRuntime.InvokeAsync<string>("ScrollToBottom","chatContainer");
                  StateHasChanged();
              });
            // Retrieve 10 last messages 
            var Last10Message = await _chatMessageInterface.Get10LastMessage(FromUserId,toUserId);
            ChatMessages = Last10Message.ToList();
            //ChatMessages = await GetVirtualScrollDataAsync();
            // Recieve information from Chat hub
            _hubConnectionMessage.On<string, bool>("OnlineUser", (senderInfo, online) =>
            {
                Online = online;
                var onlineText = "";
                if (online == true)
                {
                    
                    onlineText = "Online";
                }
                else
                {
                    onlineText = "Offline";
                }
                var encodingMsg = $"{senderInfo} : {onlineText}";
                Typing = encodingMsg;
                StateHasChanged();
            });


        }

        // Dispose method
        // If you close your browser or disconnected with reciever user, this method will be run
        public async void Dispose()
        {

            (circuitHandler as CircuitHandlerService).CircuitsChanged -=
                      HandleCircuitsChanged;
            if (toUserId != null && FromUserId != null)
            {
                if (_hubConnectionMessage.State == HubConnectionState.Disconnected)
                {
                    await _hubConnectionMessage.StartAsync();
                }
                await _hubConnectionMessage.SendAsync("OnlineSender", FromUserId, false, toUserId);
            }
        }

        // Handle the life cycle of our component
        public void HandleCircuitsChanged(object sender, EventArgs args)
        {
            InvokeAsync(() => StateHasChanged());
        }

        // Send method is used for sending a message to ou Chat hub and then show this message for both users
        public async Task Send()
        {
            
            if (_message != null)
            {
                await _hubConnectionMessage.SendAsync("SendMessage", FromUserId, _message, toUserId);
            }
        }

        // This method refresh our page
        public async Task Reload()
        {
            NavigationManager.NavigateTo("/Chat/" + toUserId, true);

        }

        // This method load our chat messages with 10 value. Each time we click on load more button, 10 records will be loaded
        public async Task Loadmore()
        {
            Loading = true;
            
            var load = await _chatMessageInterface.LoadMore(FromUserId, toUserId, startIndex);
            
            startIndex += 10;
            Thread.Sleep(1000);
            Loading = false;
            ChatMessages = load.OrderBy(x=>x.Id).ToList();
            
        }

    }
}
