using Bookinist.DAL.Entities;
using Bookinist.Infrastructure.Commands;
using Bookinist.Infrastructure.DebugService;
using Bookinist.Interfaces;
using Bookinist.Services;
using Bookinist.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace Bookinist.ViewModels;
internal class BooksViewModel : ViewModelBase
{
    private readonly IRepository<Book> _booksRepository;
    private readonly IRepository<Category> _categoryRepository;
    private readonly IUserDialog _userDialog;
    private readonly CollectionViewSource _booksViewSource;

    private ObservableCollection<Book> _books;
    public ObservableCollection<Book> Books
    {
        get => _books;
        set
        {
            if (Set(ref _books, value))
                _booksViewSource.Source = value;
        }
    }
    public ICollectionView BooksView => _booksViewSource.View;
    public string ButtonAddEdit => SelectedBook is null ? "Добавить" : "Редактир.";

    private string _booksFilter;
    public string BooksFilter
    {
        get => _booksFilter;
        set
        {
            if (Set(ref _booksFilter, value))
                _booksViewSource.View.Refresh();
        }
    }

    private Book _selectedBook;
    public Book SelectedBook
    {
        get => _selectedBook;
        set
        {
            Set(ref _selectedBook, value);
            OnPropertyChanged(nameof(ButtonAddEdit));
        }
    }

    public ICommand LoadDataCommand => new RelayCommandAsync(LoadDataCommandExecuted);
    private async Task LoadDataCommandExecuted()
    {
        Books = new ObservableCollection<Book>(await _booksRepository.Items.ToArrayAsync());
    }
    public ICommand AddBookCommand => new RelayCommandAsync(AddBookCommandExecuted);
    private async Task AddBookCommandExecuted()
    {
        var book = new Book();
        if (SelectedBook is null)
        {
            if (!_userDialog.Edit(book, _categoryRepository.Items.ToArray())) return;
            Books.Add(_booksRepository.Add(book));
            return;
        }
        if (!_userDialog.Edit(SelectedBook, _categoryRepository.Items.ToArray())) return;
        _booksRepository.Update(SelectedBook);
        _booksViewSource.View.Refresh();
    }
    public ICommand RemoveBookCommand => new RelayCommandAsync(RemoveBookCommandExecuted, () => SelectedBook is not null);
    private async Task RemoveBookCommandExecuted(object obj)
    {
        var book = obj as Book;
        await _booksRepository.RemoveAsync(book.Id);
        Books.Remove(book);
        if (ReferenceEquals(SelectedBook, book))
            SelectedBook = null;
    }

    //public BooksViewModel() : this(new DebugBookRepository(), new DebugBookRepository(), new UserDialogService())
    //{
    //    if (!App.IsDesignTime) throw new InvalidOperationException("Constructor for design time only!");
    //    _ = LoadDataCommandExecuted();
    //}

    public BooksViewModel(IRepository<Book> booksRepository, IRepository<Category> categoryRepository, IUserDialog userDialog)
    {
        _booksRepository = booksRepository;
        _categoryRepository = categoryRepository;
        _userDialog = userDialog;
        _booksViewSource = new CollectionViewSource();
        _booksViewSource.Filter += OnBooksFilter;
        _ = LoadDataCommandExecuted();
    }

    private void OnBooksFilter(object sender, FilterEventArgs e)
    {
        if (string.IsNullOrEmpty(BooksFilter)) return;

        e.Accepted = e.Item is Book book &&
            e is not null &&
            book.Name.Contains(BooksFilter, StringComparison.OrdinalIgnoreCase);

    }
}
