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
    private readonly IRepository<Category> _categoryRepository;
    private readonly IRepository<Seller> _sellerRepository;
    private readonly IRepository<Buyer> _buyerRepository;
    private readonly IRepository<Deal> _dealsRepository;
    private readonly ISalesService _salesService;
    private readonly IUserDialog _userDialog;
    private string _title = "Main Window";
    public string Title { get => _title; set => Set(ref _title, value); }

    private ViewModelBase _currentVM;
    public ViewModelBase CurrentVM { get => _currentVM; private set => Set(ref _currentVM, value); }
    public ICommand ShowBooksViewCommand => new RelayCommand(OnShowBooksViewCommandExecuted);
    private void OnShowBooksViewCommandExecuted(object obj) => CurrentVM = new BooksViewModel(_booksRepository, _categoryRepository, _userDialog);
    public ICommand ShowBuyersViewCommand => new RelayCommand(OnShowBuyersViewCommandExecuted);
    private void OnShowBuyersViewCommandExecuted(object obj) => CurrentVM = new BuyersViewModel(_buyerRepository);
    public ICommand ShowStatisticViewCommand => new RelayCommand(OnShowStatisticViewCommandExecuted);
    //private void OnShowStatisticViewCommandExecuted(object obj) => CurrentVM = new StatisticViewModel(_buyerRepository, _booksRepository, _dealsRepository);
    private void OnShowStatisticViewCommandExecuted(object obj) => CurrentVM = App.Services.GetRequiredService<StatisticViewModel>();

    public MainWindowViewModel(IRepository<Book> booksRepository, IRepository<Category> categoryRepository, IRepository<Seller> sellersRepository, IRepository<Buyer> buyerRepository, IRepository<Deal> dealsRepository, ISalesService salesService, IUserDialog userDialog)
    {
        _booksRepository = booksRepository;
        _categoryRepository = categoryRepository;
        _sellerRepository = sellersRepository;
        _buyerRepository = buyerRepository;
        _dealsRepository = dealsRepository;
        _salesService = salesService;
        _userDialog = userDialog;
        OnShowBooksViewCommandExecuted(null);

        //var bestsellers = _dealsRepository.Items.GroupBy(d => d.Book)
        //    .Select(deals => new { Book = deals.Key, Count = deals.Count() })
        //    .OrderByDescending(book => book.Count)
        //    .Take(5)
        //    .ToArray();
    }
}
