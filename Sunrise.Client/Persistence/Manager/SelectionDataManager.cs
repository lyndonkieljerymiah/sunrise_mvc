using System.Collections.Generic;
using System.Threading.Tasks;
using Sunrise.Maintenance.Abstract;
using Sunrise.Maintenance.Model;
using System.Linq;
using System;
using Sunrise.Maintenance.Data.Factory;

namespace Sunrise.Client.Persistence.Manager
{

    public class SelectionDataManager : IDisposable
    {
        
        private IMasterFileFactory Factory { get; set; }

        public SelectionDataManager(IMasterFileFactory factory)
        {
            this.Factory = factory;
        }

        public async Task<IEnumerable<Selection>> GetLookup(string[] parent)
        {
            var selections = await Factory.Selections.GetLookup(parent);
            return selections;
        }

        public void Dispose()
        {
            Factory.Dispose();
        }
    }
}
