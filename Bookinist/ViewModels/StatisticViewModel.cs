using Bookinist.DAL.Entities;
using Bookinist.Infrastructure.Commands;
using Bookinist.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bookinist.ViewModels;

internal class StatisticViewModel : ViewModelBase
{
    private IRepository<Buyer> _buyerRepository;
    private IRepository<Book> _booksRepository;
    private IRepository<Deal> _dealsRepository;

    private Dictionary<Book, int> _bestsellers;
    public Dictionary<Book, int> Bestsellers { get => _bestsellers; set => Set(ref _bestsellers, value); }

    private ICommand _bestsellersCommand;
    public ICommand BestsellersCommand => _bestsellersCommand ??= new RelayCommandAsync(BestsellersCommandExecuteAsync);
    private async Task BestsellersCommandExecuteAsync()
    {
        Bestsellers = _dealsRepository.Items.GroupBy(d => d.Book)
            .Select(deals => new { Book = deals.Key, Count = deals.Count() })
            .OrderByDescending(book => book.Count)
            .Take(5)
            .ToDictionary(k => k.Book, k => k.Count);
    }

    public StatisticViewModel(IRepository<Buyer> buyerRepository, IRepository<Book> booksRepository, IRepository<Deal> dealsRepository)
    {
        _buyerRepository = buyerRepository;
        _booksRepository = booksRepository;
        _dealsRepository = dealsRepository;
    }
}
