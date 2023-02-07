using Bookinist.DAL.Entities;
using Bookinist.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookinist.ViewModels;

internal class BuyersViewModel : ViewModelBase
{
    private readonly IRepository<Buyer> _buyerRepository;

    public BuyersViewModel(IRepository<Buyer> buyerRepository)
    {
        _buyerRepository = buyerRepository;
    }
}
