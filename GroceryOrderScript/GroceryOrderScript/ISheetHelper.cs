using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GroceryOrderScript
{
    public interface ISheetHelper
    {
        Task<ValueRange> GetRange(string spreadSheetId, string range);
        Task Append<T>(IEnumerable<T> models, string spreadSheetId, string range) where T : IToValueList;
        Task AppendOnlyDistinct<T>(IEnumerable<T> models, string spreadSheetId, string range) where T : IToValueList, IEquatable<T>;

    }
}
