using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sunrise.Client.Domains.Models;
using Sunrise.Client.Persistence.Abstract;

namespace Sunrise.Client.Persistence.Manager
{
    
    public class SelectionDataManager
    {
        private readonly IUnitOfWork _unitOfWork;

        public SelectionDataManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Selection>> GetLookup(string[] parent)
        {
            var selections = await _unitOfWork.Selections.GetSelections(parent);
            return selections;
        } 
    }
}
