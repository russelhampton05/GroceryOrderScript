using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroceryOrderScript
{
    public interface IGroceryOrder<T>
    {
        Task PlaceOrder(List<T> items);
    }
}
