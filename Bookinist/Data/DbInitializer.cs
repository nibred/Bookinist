using Bookinist.DAL.Context;
using Bookinist.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookinist.Data;

internal class DbInitializer
{
    private readonly BookinistDB _db;
    private readonly ILogger<DbInitializer> _logger;

    public DbInitializer(BookinistDB db, ILogger<DbInitializer> logger)
    {
        _db = db;
        _logger = logger;
    }

    public async Task InitializeAsync()
    {
        var timer = Stopwatch.StartNew();

        _logger.LogInformation("Миграция БД");
        await _db.Database.MigrateAsync();
        _logger.LogInformation($"Миграция БД выполнена за {timer.ElapsedMilliseconds} мс");

        if (await _db.Books.AnyAsync()) { return; }

        await InitializeCategories();
        await InitializeBooks();
        await InitializeBuyers();
        await InitializeSellers();
        await InitializeDeals();

        _logger.LogInformation($"Инициализация БД выполнена за {timer.Elapsed.TotalSeconds} с");
    }

    private const int _categoriesCount = 10;
    private Category[] _categories;
    private async Task InitializeCategories()
    {
        _categories = new Category[_categoriesCount];
        for (int i = 0; i < _categoriesCount; i++)
        {
            _categories[i] = new Category { Name = $"Категория {i}" };
        }

        await _db.Categories.AddRangeAsync(_categories);
        await _db.SaveChangesAsync();
    }

    private const int _booksCount = 10;
    private Book[] _books;
    private async Task InitializeBooks()
    {
        var rnd = new Random();
        _books = Enumerable.Range(0, _booksCount)
            .Select(i => new Book
            {
                Name = $"Книга {i}",
                Category = rnd.NextItem(_categories)
            })
            .ToArray();
        await _db.Books.AddRangeAsync(_books);
        await _db.SaveChangesAsync();
    }

    private const int _sellersCount = 10;
    private Seller[] _sellers;
    private async Task InitializeSellers()
    {
        var rnd = new Random();
        _sellers = Enumerable.Range(0, _sellersCount)
            .Select(i => new Seller
            {
                Name = $"Продавец-Имя {i}",
                Surname = $"Продавец-Фамилия {i}",
                Patronymic = $"Продавец-Отчество {i}"
            })
            .ToArray();
        await _db.Sellers.AddRangeAsync(_sellers);
        await _db.SaveChangesAsync();
    }

    private const int _buyerCount = 10;
    private Buyer[] _buyer;
    private async Task InitializeBuyers()
    {
        var rnd = new Random();
        _buyer = Enumerable.Range(0, _buyerCount)
            .Select(i => new Buyer
            {
                Name = $"Покупатель-Имя {i}",
                Surname = $"Покупатель-Фамилия {i}",
                Patronymic = $"Покупатель-Отчество {i}"
            })
            .ToArray();
        await _db.Buyers.AddRangeAsync(_buyer);
        await _db.SaveChangesAsync();
    }

    private const int _dealsCount = 1000;
    private async Task InitializeDeals()
    {
        var rnd = new Random();
        var deals = Enumerable.Range(0, _dealsCount)
            .Select(i => new Deal
            {
                Book = rnd.NextItem(_books),
                Buyer = rnd.NextItem(_buyer),
                Seller = rnd.NextItem(_sellers),
                Price = (decimal)(rnd.NextDouble() * 4000 + 400)
            });

        await _db.Deals.AddRangeAsync(deals);
        await _db.SaveChangesAsync();
    }
}
