using Bookinist.DAL.Entities;
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

    public BooksViewModel(IRepository<Book> booksRepository)
    {
        _booksRepository = booksRepository;
    }
}
