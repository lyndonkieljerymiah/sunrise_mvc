using Sunrise.Client.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sunrise.Client.Persistence.Manager
{
    public class UserDataManager
    {
        private AppDbContext Context { get; }
        public UserDataManager()
        {
            Context = new AppDbContext();
        }

        public async Task<bool> CheckTransactionCode(string userId,string transactionCode)
        {
            var user = await Context.Users.SingleOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                return false;
            return user.IsAuthenticated(transactionCode);
        }
    }
}
