using Sunrise.TransactionManagement.Data.Contract;
using System;

namespace Sunrise.TransactionManagement.Data.Factory
{
    public interface IContractDataFactory : IDisposable
    {
        IContractDataService Contracts { get; }
    }
}
