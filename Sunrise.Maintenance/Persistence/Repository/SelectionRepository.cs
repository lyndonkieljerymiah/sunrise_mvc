using Sunrise.Maintenance.Abstract;
using Sunrise.Maintenance.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities.GeneralRepository;

namespace Sunrise.Maintenance.Persistence.Repository
{
    public class SelectionRepository : BaseRepository<Selection, AppDbContext>, ISelectionRepository
    {
        public SelectionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Selection>> GetSelectionByType(string[] type)
        {
            var selections = await (from sel in _context.Selections
                                    where (type.Contains(sel.Type))
                                    select sel).ToListAsync();

            return selections;
        }

    }
}
