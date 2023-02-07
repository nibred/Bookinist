using Bookinist.DAL.Entities;
using Bookinist.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookinist.ViewModels;

internal class StatisticViewModel : ViewModelBase
{
    private IRepository<Buyer> _buyerRepository;
    private IRepository<Book> _booksRepository;
    private IRepository<Deal> _dealsRepository;

    public StatisticViewModel(IRepository<Buyer> buyerRepository, IRepository<Book> booksRepository, IRepository<Deal> dealsRepository)
    {
        _buyerRepository = buyerRepository;
        _booksRepository = booksRepository;
        _dealsRepository = dealsRepository;
    }
}
