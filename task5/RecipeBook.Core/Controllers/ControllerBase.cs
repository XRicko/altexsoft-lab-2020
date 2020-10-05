using RecipeBook.SharedKernel;
using RecipeBook.SharedKernel.Interfaces;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

        public async Task AddItemAsync<T>(T entity) where T : BaseEntity
        {
            var item = await UnitOfWork.Repository.GetAsync(entity);

            if (item == null)
            {
                var items = await UnitOfWork.Repository.GetAllAsync<T>();

                if (items != null && items.Any())
                    entity.Id = items.Last().Id + 1;

                await UnitOfWork.Repository.AddAsync(entity);
            }
        }

        protected string StandardizeName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower()).Trim();
        }
    }
}
