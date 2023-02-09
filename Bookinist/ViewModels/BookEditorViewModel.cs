using Bookinist.DAL.Entities;
using Bookinist.Infrastructure.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace Bookinist.ViewModels;

internal class BookEditorViewModel : ViewModelBase
{
    private string _name;
    public string Name { get => _name; set => Set(ref _name, value); }

    public int BookID { get; }

    public ICommand EditBookCommand => new DialogCommand(BookEditCommandExecuted);

    private void BookEditCommandExecuted(object obj)
    {
        var book = obj as Book;
        book.Name = Name;
    }

    public BookEditorViewModel() : this(new Book { Id = 1, Name = "Тестовая книга" })
    {
        if (!App.IsDesignTime) throw new InvalidOperationException("Design time only!");
    }

    public BookEditorViewModel(Book book)
    {
        BookID = book.Id;
        Name = book.Name;
    }
}
