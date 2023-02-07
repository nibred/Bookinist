using Bookinist.DAL.Entities;
using Bookinist.Infrastructure.Commands;
using Bookinist.Interfaces;
using Bookinist.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

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

    private ViewModelBase _currentVM;
    public ViewModelBase CurrentVM { get => _currentVM; private set => Set(ref _currentVM, value); }

    private ICommand _showBooksViewCommand;
    public ICommand ShowBooksViewCommand => _showBooksViewCommand ??= new RelayCommand(OnShowBooksViewCommandExecuted);
    private void OnShowBooksViewCommandExecuted(object obj) => CurrentVM = new BooksViewModel(_booksRepository);

    private ICommand _showBuyersViewCommand;
    public ICommand ShowBuyersViewCommand => _showBuyersViewCommand ??= new RelayCommand(OnShowBuyersViewCommandExecuted);
    private void OnShowBuyersViewCommandExecuted(object obj) => CurrentVM = new BuyersViewModel(_buyerRepository);

    private ICommand _showStatisticViewCommand;
    public ICommand ShowStatisticViewCommand => _showStatisticViewCommand ??= new RelayCommand(OnShowStatisticViewCommandExecuted);
    //private void OnShowStatisticViewCommandExecuted(object obj) => CurrentVM = new StatisticViewModel(_buyerRepository, _booksRepository, _dealsRepository);
    private void OnShowStatisticViewCommandExecuted(object obj) => CurrentVM = App.Services.GetRequiredService<StatisticViewModel>();

    public MainWindowViewModel(IRepository<Book> booksRepository, IRepository<Seller> sellersRepository, IRepository<Buyer> buyerRepository, IRepository<Deal> dealsRepository, ISalesService salesService)
    {
        _booksRepository = booksRepository;
        _sellerRepository = sellersRepository;
        _buyerRepository = buyerRepository;
        _dealsRepository = dealsRepository;
        _salesService = salesService;

        OnShowBooksViewCommandExecuted(null);


        //var bestsellers = _dealsRepository.Items.GroupBy(d => d.Book)
        //    .Select(deals => new { Book = deals.Key, Count = deals.Count() })
        //    .OrderByDescending(book => book.Count)
        //    .Take(5)
        //    .ToArray();
    }
}
