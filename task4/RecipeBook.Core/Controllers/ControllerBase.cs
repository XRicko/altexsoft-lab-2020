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
        protected readonly IUnitOfWork unitOfWork;

        public ControllerBase(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<List<T>> GetItemsAsync<T>() where T : BaseEntity
        {
            var items = await unitOfWork.Repository.GetAllAsync<T>();
            return items.ToList();
        }

        public async Task AddAsync<T>(T entity) where T : BaseEntity
        {
            var ing = await unitOfWork.Repository.GetAsync(entity);

            if (ing == null)
                await unitOfWork.Repository.AddAsync(entity);
        }

        public void Remove<T>(T entity) where T : BaseEntity
        {
            unitOfWork.Repository.Remove(entity);
            unitOfWork.SaveAsync();
        }

        public async Task RemoveAsync<T>(string name) where T : BaseEntity
        {
            StandardizeName(name);

            var entity = await unitOfWork.Repository.GetAsync<T>(name);
            unitOfWork.Repository.Remove(entity);

            unitOfWork.SaveAsync();
        }

        protected string StandardizeName(string name)
        {
            name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());
            name = name.Trim();

            return name;
        }
    }
}
