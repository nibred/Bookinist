using Bookinist.DAL.Entities;
using Bookinist.Infrastructure.DebugService;
using Bookinist.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookinist.ViewModels;
internal class BooksViewModel : ViewModelBase
{
    private readonly IRepository<Book> _booksRepository;

    public IEnumerable<Book> Books => _booksRepository.Items;

    public BooksViewModel()
    {
        if (!App.IsDesignTime) throw new InvalidOperationException("Constructor for design time only!");
        _booksRepository = new DebugBookRepository();
    }

    public BooksViewModel(IRepository<Book> booksRepository)
    {
        _booksRepository = booksRepository;
    }
}
