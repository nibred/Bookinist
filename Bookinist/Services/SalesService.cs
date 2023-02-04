using Bookinist.DAL.Entities;
using Bookinist.Interfaces;
using Bookinist.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookinist.Services;

public class SalesService : ISalesService
{
    private readonly IRepository<Book> _books;
    private readonly IRepository<Deal> _deals;

    public IEnumerable<Deal> Deals => _deals.Items;

    public SalesService(IRepository<Book> books, IRepository<Deal> deals)
    {
        _books = books;
        _deals = deals;
    }

    public async Task<Deal> MakeDeal(string bookName, Seller seller, Buyer buyer, decimal price)
    {
        var book = await _books.Items.FirstOrDefaultAsync(i => i.Name == bookName);
        if (book == null) { return null; }
        var deal = new Deal
        {
            Book = book,
            Seller = seller,
            Buyer = buyer,
            Price = price
        };
        return await _deals.AddAsync(deal);
    }
}
