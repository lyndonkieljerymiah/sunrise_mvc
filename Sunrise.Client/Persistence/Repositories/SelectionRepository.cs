using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.Client.Domains.Models;
using Sunrise.Client.Persistence.Abstract;
using Sunrise.Client.Persistence.Context;

namespace Sunrise.Client.Persistence.Repositories
{
    public class SelectionRepository : BaseRepository<Selection>, ISelectionRepository
    {
        public SelectionRepository(AppDbContext context, ReferenceDbContext referenceDb) : base(context, referenceDb)
        {
        }

        public async Task<ICollection<Selection>> GetSelections(string[] keys)
        {
            var selections = await (from sel in _context.Selections
                where (keys.Contains(sel.Type))
                select sel).ToListAsync();

            return selections;
        }
    }
}
