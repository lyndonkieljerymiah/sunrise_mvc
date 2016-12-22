using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.Client.Domains.Models;

namespace Sunrise.Client.Persistence.Abstract
{
    public interface IVillaRepository : IBaseRepository<Villa>
    {
        Task<Villa> GetVilla(string villaNo);
        Task<IEnumerable<Villa>> GetAvailable();
        Task<IEnumerable<Villa>> GetAllVilla();
    }
}
