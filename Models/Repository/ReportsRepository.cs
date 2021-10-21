using PrivateMessenger.Areas.Identity.Data;
using PrivateMessenger.Data;
using PrivateMessenger.Models.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrivateMessenger.Models.Repository
{
    public class ReportsRepository : ReportsInterface
    {
        private readonly AppDbContext db;
        public ReportsRepository(AppDbContext db)
        {
            this.db = db;
        }
        // Add new record to reports table
        public async Task<Reports> Add(Reports reports)
        {
            await db.Reports.AddAsync(reports);
            await db.SaveChangesAsync();
            return reports;
        }
    }
}
