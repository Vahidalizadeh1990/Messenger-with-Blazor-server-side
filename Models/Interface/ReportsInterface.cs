using PrivateMessenger.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateMessenger.Models.Interface
{
    /// <summary>
    /// This interface will be injected in our components
    /// We just add a new report to our table with this interface
    /// </summary>
    public interface ReportsInterface
    {
        Task<Reports> Add(Reports reports);
    }
}
