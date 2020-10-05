using System;
using System.Threading.Tasks;

namespace RecipeBook.SharedKernel.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository Repository { get; }

        Task SaveAsync();
    }
}
