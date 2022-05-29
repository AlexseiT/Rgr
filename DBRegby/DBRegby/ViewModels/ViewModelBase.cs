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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
namespace DBRegby.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        public object Item { get; set; }
        public virtual object getThisTable() { return null; }
        public virtual List<Dictionary<string, object?>> getRowsThisTable()
        {
            return new List<Dictionary<string, object?>>();
        }
    }
}
