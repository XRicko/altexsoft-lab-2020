using RecipeBook.SharedKernel;
using RecipeBook.SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace RecipeBook.Core.Controllers
{
    public abstract class ControllerBase
    {
        protected IUnitOfWork UnitOfWork { get; }

        public ControllerBase(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public Task<IEnumerable<T>> GetItemsAsync<T>() where T : BaseEntity
        {
            return UnitOfWork.Repository.GetAllAsync<T>();
        }

        public async Task AddAsync<T>(T entity) where T : BaseEntity
        {
            var item = await UnitOfWork.Repository.GetAsync(entity);

            if (item == null)
                await UnitOfWork.Repository.AddAsync(entity);
        }

        public async Task RemoveAsync<T>(T entity) where T : BaseEntity
        {
            UnitOfWork.Repository.Remove(entity);
            await UnitOfWork.SaveAsync();
        }

        public async Task RemoveAsync<T>(string name) where T : BaseEntity
        {
            var entity = await UnitOfWork.Repository.GetAsync<T>(StandardizeName(name));
            UnitOfWork.Repository.Remove(entity);

            await UnitOfWork.SaveAsync();
        }

        protected string StandardizeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower()).Trim();
        }
    }
}
