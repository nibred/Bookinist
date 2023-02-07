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
    private IRepository<Buyer> _buyerRepository;
    private IRepository<Book> _booksRepository;
    private IRepository<Deal> _dealsRepository;

    public ObservableCollection<Bestseller> Bestsellers { get; } = new();

    private ICommand _bestsellersCommand;
    public ICommand BestsellersCommand => _bestsellersCommand ??= new RelayCommandAsync(BestsellersCommandExecuteAsync);
    private async Task BestsellersCommandExecuteAsync()
    {
        var bestsellers = await _dealsRepository.Items.GroupBy(d => d.Book)
            .Select(deals => new { Book = deals.Key, Count = deals.Count() })
            .OrderByDescending(book => book.Count)
            .Take(5)
            .Select(k => new Bestseller { Name = k.Book.Name, Count= k.Count })
            .ToListAsync();

        Bestsellers.Clear();
        bestsellers.ForEach(i => Bestsellers.Add(i));
    }

    public StatisticViewModel(IRepository<Buyer> buyerRepository, IRepository<Book> booksRepository, IRepository<Deal> dealsRepository)
    {
        _buyerRepository = buyerRepository;
        _booksRepository = booksRepository;
        _dealsRepository = dealsRepository;

        BestsellersCommandExecuteAsync();
    }
}
