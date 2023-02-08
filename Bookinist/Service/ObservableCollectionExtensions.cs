using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections.ObjectModel;

public static class ObservableCollectionExtensions
{
    public static void AddWithClear<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
    {
        collection.Clear();
        collection.Add(items);
    }
    public static void Add<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
    {
        foreach (T item in items)
        {
            collection.Add(item);
        }
    }
}
