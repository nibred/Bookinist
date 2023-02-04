using Bookinist.DAL.Entities;
using Bookinist.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookinist.DAL;

public static class RepositoryRegistrator
{
    public static IServiceCollection AddRepositories(this IServiceCollection services) => services
        .AddTransient<IRepository<Book>, BookRepository>()
        .AddTransient<IRepository<Seller>, DbRepository<Seller>>()
        .AddTransient<IRepository<Buyer>, DbRepository<Buyer>>()
        .AddTransient<IRepository<Category>, DbRepository<Category>>()
        .AddTransient<IRepository<Deal>, DealRepository>()
        ;
}
