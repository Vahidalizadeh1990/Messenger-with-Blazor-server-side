using Microsoft.EntityFrameworkCore;
using PrivateMessenger.Areas.Identity.Data;
using PrivateMessenger.Data;
using PrivateMessenger.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateMessenger.Models.Repository
{
    public class ConnectionRepository : ConnectionInterface
    {
        private readonly AppDbContext appDbContext;

        public ConnectionRepository(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        // Add a connection to our database table
        public async Task<Connection> Add(Connection connection)
        {
            await appDbContext.Connection.AddAsync(connection);
            appDbContext.SaveChanges();
            return connection;
        }

        // Retrieve data from connection table with specific sender and reciever user id
        public Connection GetBySenderAndRecieverId(string senderId,string recieverUserId)
        {
            return  appDbContext.Connection.Include(x=>x.RecieverUser).Include(y=>y.SenderUser).FirstOrDefault(x => x.SenderUserId == senderId && x.RecieverUserId==recieverUserId);
        }

        // List of connection based on a reciever id
        public async Task<List<Connection>> GetByRecieverId(string recieverId)
        {
            return await appDbContext.Connection.Include(x => x.RecieverUser).Include(y => y.SenderUser).Where(x => x.RecieverUserId == recieverId).ToListAsync();
        }


        // List of connection based on a sender id
        public async Task<List<Connection>> GetBySenderId(string senderId)
        {
            return await appDbContext.Connection.Include(x => x.RecieverUser).Include(y => y.SenderUser).Where(x => x.SenderUserId == senderId).ToListAsync();
        }

        // Update connection
        public async Task<Connection> Update(Connection connection)
        {
            var connections = appDbContext.Connection.Attach(connection);
            connections.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            await appDbContext.SaveChangesAsync();
            return connection;
        }

        // Retrieve a connection based on a reciever and sender user id
        public async Task<Connection> GetByRecieverAndSenderId(string recieverUserId, string senderId)
        {
            return await appDbContext.Connection.Include(x => x.RecieverUser).Include(y => y.SenderUser).FirstOrDefaultAsync(x => x.RecieverUserId == recieverUserId && x.SenderUserId == senderId);
        }
    }
}
