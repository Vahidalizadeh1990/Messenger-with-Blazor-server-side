using PrivateMessenger.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateMessenger.Models.Interface
{
    /// <summary>
    /// We inject this interface in our component to have an access to Connection table
    /// We have GetBySenderAndRecieverId() and GetByRecieverAndSenderId, because we need to check the both side of this table to show
    /// this information for a sender and reciever. 
    /// For example if a user with id 123 is connected, we should show to user with id 456 that a user with id 123 is connected and vice versa
    /// </summary>
    public interface ConnectionInterface
    {
        // Add a new conenction with other information about a user such as an example, we store userId, Date, is connected or not, etc.
        Task<Connection> Add(Connection connection);
        // Retrieve a row by sender and reciever user id
        Connection GetBySenderAndRecieverId(string senderId, string recieverUserId);
        // Retrieve a row by reciever and sender user id
        Task<Connection> GetByRecieverAndSenderId(string recieverUserId, string senderId);
        // Retrieve a row by a sender user id
        Task<List<Connection>> GetBySenderId(string senderId);
        // Retrieve a row by a reciever user id
        Task<List<Connection>> GetByRecieverId(string recieverId);
        // Update our connection
        // We can figure out which user is online with update method.
        // When a user open chat componet, The connection will be updated.
        Task<Connection> Update(Connection connection);
    }
}
