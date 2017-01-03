using System.Collections.Generic;
using System.Threading.Tasks;
using Sunrise.Maintenance.Abstract;
using Sunrise.Maintenance.Model;

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
            var selections = await _unitOfWork.Selections.GetSelectionByType(parent);
            return selections;
        } 
    }
}
