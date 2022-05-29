using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using ReactiveUI;
using System.Reactive;
using DBRegby.Models;
using Microsoft.Data.Sqlite;
using System.IO;
using System;
using System.Reactive.Linq;

namespace DBRegby.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ViewModelBase page;
        private DataBaseViewModel ViewPage;
        private RequestManagerViewModel requestManager;
        public ViewModelBase Page
        {
            set => this.RaiseAndSetIfChanged(ref page, value);
            get => page;
        }
        public MainWindowViewModel()
        {
            ViewPage = new DataBaseViewModel();
            requestManager = new RequestManagerViewModel(ViewPage);
            Page = ViewPage;
        }

        public void OpenRequest()
        {
            Page = requestManager;
        }

        public void OpenDB()
        {
            Page = ViewPage;
        }
    }
}
