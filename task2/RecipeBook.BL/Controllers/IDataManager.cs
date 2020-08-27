using System.Collections.Generic;

namespace RecipeBook.BL.Controllers
{
    public interface IDataManager
    {
        void Save<T>(List<T> items);
        List<T> Load<T>();
    }
}
