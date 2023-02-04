using Bookinist.DAL.Entities;
using Bookinist.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookinist.ViewModels;

class MainWindowViewModel : ViewModelBase
{
    private string _title = "MainWindow";
    private readonly IRepository<Book> _booksRepository;

    public string Title { get => _title; set => Set(ref _title, value); }

    public MainWindowViewModel(IRepository<Book> booksRepository)
    {
        _booksRepository = booksRepository;

        var books = _booksRepository.Items.Take(10).ToArray();
    }
}
