using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookinist.ViewModels;

class MainWindowViewModel : ViewModelBase
{
    private string _title = "MainWindow";
    public string Title { get => _title; set => Set(ref _title, value); }
}
