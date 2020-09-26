using System;

namespace RecipeBook.SharedKernel.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository Repository { get; }

        void SaveAsync();
    }
}
