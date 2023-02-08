using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookinist.Service;

internal static class EnumerableExtensions
{
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> items) => new(items);
    public static ObservableCollection<T> ToObservableCollection<T>(this List<T> items) => new(items);
}
