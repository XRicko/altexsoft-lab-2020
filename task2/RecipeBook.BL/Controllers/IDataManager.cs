using System.Collections.Generic;

namespace RecipeBook.BL.Controllers
{
    interface IDataManager
    {
        void Save<T>(List<T> items);
        List<T> Load<T>();
    }
}
