using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Sunrise.Client.Domains.DTO;
using Sunrise.Client.Persistence.Abstract;
using Sunrise.Client.Persistence.Manager;
using Sunrise.Client.Persistence.Repositories;

namespace AppUnitTest.DataManager
{
    public class VillaDataManagerTest
    {

        private VillaDataManager _manager;
        private VillaDTO _dto;
       
        public void Initialize()
        {

            IUnitOfWork work = new UnitOfWork();
            _manager = new VillaDataManager(work);

        }












    }
}
