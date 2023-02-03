using Bookinist.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookinist.Data;

static class DbRegistrator
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration) => services
        .AddDbContext<BookinistDB>(options =>
        {
            options.UseSqlite(configuration.GetConnectionString("SQLite"));
        })
        ;
}
