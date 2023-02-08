using Bookinist.DAL.Entities;
using Bookinist.Infrastructure.Commands;
using Bookinist.Interfaces;
using Bookinist.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Bookinist.ViewModels;

internal class StatisticViewModel : ViewModelBase
{
    private readonly IRepository<Buyer> _buyerRepository;
    private readonly IRepository<Book> _booksRepository;
    private readonly IRepository<Deal> _dealsRepository;

    public ObservableCollection<Bestseller> Bestsellers { get; } = new();

    private ICommand _bestsellersCommand;
    public ICommand BestsellersCommand => _bestsellersCommand ??= new RelayCommandAsync(BestsellersCommandExecuteAsync);
    private async Task BestsellersCommandExecuteAsync()
    {
        var bestsellers = await _dealsRepository.Items //TODO check 'sum' function
            .GroupBy(d => d.Book)
            .Select(deals => new
            {
                Book = deals.Key,
                Count = deals.Count()
            })
            .Take(5)
            .OrderByDescending(d => d.Count)
            .Select(i => new Bestseller
            {
                Name = i.Book.Name,
                Count = i.Count
            })
            .ToArrayAsync();

        Bestsellers.AddWithClear(bestsellers);
    }

    public StatisticViewModel(IRepository<Buyer> buyerRepository, IRepository<Book> booksRepository, IRepository<Deal> dealsRepository)
    {
        _buyerRepository = buyerRepository;
        _booksRepository = booksRepository;
        _dealsRepository = dealsRepository;

        BestsellersCommandExecuteAsync();
    }
}
