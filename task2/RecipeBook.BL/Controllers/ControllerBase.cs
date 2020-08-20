using RecipeBook.BL.Models;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace RecipeBook.BL.Controllers
{
    public abstract class ControllerBase
    {
        private readonly IDataManager manager = new JSONDataManager();

        protected void Save<T>(List<T> items)
        {
            manager.Save(items);
        }
        protected List<T> Load<T>()
        {
            return manager.Load<T>();
        }
        protected T GetItem<T>(List<T> models, T model) where T : ModelBase
        {
            var item = models.SingleOrDefault(x => x.Name == model.Name);
            return item;
        }
        protected T GetItem<T>(List<T> models, ref string name) where T : ModelBase
        {
            name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());
            name = name.Trim();

            var tempName = name;
            var item = models.SingleOrDefault(x => x.Name == tempName);

            return item;
        }
        protected bool AddItem<T>(List<T> models, T model) where T : ModelBase
        {
            var item = GetItem(models, model);

            if (item == null)
            {
                if (models.Count != 0)
                    model.Id = models.Last().Id + 1;

                models.Add(model);

                return true;
            }

            return false;
        }
    }
}
