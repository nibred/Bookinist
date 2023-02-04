using Bookinist.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookinist.Services.Interfaces;

public interface ISalesService
{
    IEnumerable<Deal> Deals { get; }

    Task<Deal> MakeDeal(string bookName, Seller seller, Buyer buyer, decimal price);
}
