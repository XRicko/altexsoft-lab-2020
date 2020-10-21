using RecipeBook.SharedKernel.Interfaces;
using System.Threading.Tasks;

namespace RecipeBook.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RecipeBookContext context;

        public IRepository Repository { get; }

        public UnitOfWork(RecipeBookContext context, IRepository repository)
        {
            this.context = context;
            Repository = repository;
        }

        public Task SaveAsync()
        {
            return context.SaveChangesAsync();
        }

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
