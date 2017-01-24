using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.Maintenance.Model;
using Sunrise.Maintenance.Persistence;
using System.Data.Entity;

namespace Sunrise.Maintenance.Data.MasterFile
{
    public class SelectionDataService : ISelectionDataService
    {
        public AppDbContext Context { get; set; }

        public SelectionDataService(AppDbContext context)
        {
            Context = context;
        }

        public async Task<IEnumerable<Selection>> GetLookup(string[] categoryCode)
        {
            return await Context.Selections
                .Where(s => categoryCode.Contains(s.Type)).ToListAsync();
        }
    }
}
