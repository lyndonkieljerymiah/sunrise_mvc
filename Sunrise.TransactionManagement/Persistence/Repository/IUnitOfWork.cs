using Sunrise.TransactionManagement.Persistence.Repository.Abstract;
using System;
using System.Threading.Tasks;

namespace Sunrise.TransactionManagement.Persistence.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IContractRepository Contracts { get; }
        IBillRepository Bills { get; }

        Task Commit();
    }
}
