using Bookinist.DAL.Entities;
using Bookinist.Services.Interfaces;
using Bookinist.ViewModels;
using Bookinist.Views.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookinist.Services;

public class UserDialogService : IUserDialog
{
    public Book Edit(Book book)
    {
        var bookEditorVM = new BookEditorViewModel(book);
        var bookEditorWindow = new BookEditorWindow { DataContext = bookEditorVM };

        bookEditorWindow.ShowDialog();

        book.Name = bookEditorVM.Name;

        return book;
    }
}
