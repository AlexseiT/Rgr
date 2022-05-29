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

namespace DBRegby.ViewModels
{
    public abstract class Handler
    {
        protected RequestManagerViewModel? RequestManager { get; set; } = null;   
        public Handler? next { get; set; }                      
        public abstract void Try();                              
    }
}