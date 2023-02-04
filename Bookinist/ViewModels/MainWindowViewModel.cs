using Bookinist.DAL.Entities;
using Bookinist.Interfaces;
using Bookinist.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookinist.ViewModels;

class MainWindowViewModel : ViewModelBase
{
    private readonly IRepository<Book> _booksRepository;
    private readonly IRepository<Seller> _sellerRepository;
    private readonly IRepository<Buyer> _buyerRepository;
    private readonly IRepository<Deal> _dealsRepository;
    private readonly ISalesService _salesService;

    private string _title = "MainWindow";
    public string Title { get => _title; set => Set(ref _title, value); }

    public MainWindowViewModel(IRepository<Book> booksRepository, IRepository<Seller> sellersRepository, IRepository<Buyer> buyerRepository, IRepository<Deal> dealsRepository, ISalesService salesService)
    {
        _booksRepository = booksRepository;
        _sellerRepository = sellersRepository;
        _buyerRepository = buyerRepository;
        _dealsRepository = dealsRepository;
        _salesService = salesService;

        var bestsellers = _dealsRepository.Items.GroupBy(d => d.Book)
            .Select(deals => new { Book = deals.Key, Count = deals.Count() })
            .OrderByDescending(book => book.Count)
            .Take(5)
            .ToArray();
    }
}
