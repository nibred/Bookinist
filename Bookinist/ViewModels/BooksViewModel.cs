using Bookinist.DAL.Entities;
using Bookinist.Infrastructure.DebugService;
using Bookinist.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Bookinist.ViewModels;
internal class BooksViewModel : ViewModelBase
{
    private readonly IRepository<Book> _booksRepository;
    private readonly CollectionViewSource _booksViewSource;
    public ICollectionView BooksView => _booksViewSource.View;
    public IEnumerable<Book> Books => _booksRepository.Items;

    private string _booksFilter;
    public string BooksFilter
    {
        get => _booksFilter;
        set
        {
            if (Set(ref _booksFilter, value))
                _booksViewSource.View.Refresh();
        }
    }
    public BooksViewModel() : this(new DebugBookRepository())
    {
        if (!App.IsDesignTime) throw new InvalidOperationException("Constructor for design time only!");
    }

    public BooksViewModel(IRepository<Book> booksRepository)
    {
        _booksRepository = booksRepository;
        _booksViewSource = new CollectionViewSource
        {
            Source = Books,
            SortDescriptions =
            {
                new SortDescription(nameof(Book.Name), ListSortDirection.Ascending)
            }
        };
        _booksViewSource.Filter += OnBooksFilter;
    }

    private void OnBooksFilter(object sender, FilterEventArgs e)
    {
        if (string.IsNullOrEmpty(BooksFilter)) return;

        e.Accepted = e.Item is Book book &&
            e is not null &&
            book.Name.Contains(BooksFilter, StringComparison.OrdinalIgnoreCase);

    }
}
