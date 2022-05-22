using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Reactive;
using DBRegby.Models;
using Microsoft.Data.Sqlite;
using System.IO;

namespace DBRegby.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        public virtual object getThisTable() { return null; }
    }
}
