using Bookinist.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookinist.Services.Interfaces;

internal interface IUserDialog
{
    Book Edit(Book book, Category[] categories);
}
