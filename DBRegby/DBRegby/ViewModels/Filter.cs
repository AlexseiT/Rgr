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
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DBRegby.ViewModels
{
    public class Filter : INotifyPropertyChanged
    {
        private string? thisoperator;          
        private string example;          
        private bool ValueSupport; 
        public Filter(string BoolOper, ObservableCollection<string> Columns)
        {
            this.BoolOper = BoolOper;
            this.Columns = Columns;
            InputValueSupport = true;
            AllOperators = new ObservableCollection<string> {">", ">=", "=", "<>", "<", "<=","InRange", "NotInRange", "Contains", "NotContains","IsNull", "NotNull", "Belong", "NotBelong"};
            Operator = "";
            Column = "";
            Filtervalue = "";
            Example = "";
        }










        public string? Operator
        {
            get => thisoperator;
            set
            {
                thisoperator = value;
                switch (thisoperator)
                {
                    case ">":
                        Example = "Number";
                        InputValueSupport = true;
                        break;
                    case ">=":
                        Example = "Number";
                        InputValueSupport = true;
                        break;
                    case "=":
                        Example = "Number || String";
                        InputValueSupport = true;
                        break;
                    case "<>":
                        Example = "Number || String";
                        InputValueSupport = true;
                        break;
                    case "<":
                        Example = "Number";
                        InputValueSupport = true;
                        break;
                    case "<=":
                        Example = "Number";
                        InputValueSupport = true;
                        break;
                    case "In Range":
                        Example = "10..40";
                        InputValueSupport = true;
                        break;
                    case "Not In Range":
                        Example = "10..40";
                        InputValueSupport = true;
                        break;
                    case "Contains":
                        Example = "Substring";
                        InputValueSupport = true;
                        break;
                    case "Not Contains":
                        Example = "Substring";
                        InputValueSupport = true;
                        break;
                    case "Is Null":
                        Example = "";
                        InputValueSupport = false;
                        break;
                    case "Not Null":
                        Example = "";
                        InputValueSupport = false;
                        break;
                    case "Belong":
                        Example = "1, 2 || str1, str2";
                        InputValueSupport = true;
                        break;
                    case "Not Belong":
                        Example = "1, 2 || str1, str2";
                        InputValueSupport = true;
                        break;
                }
            }
        }
        public string Example
        {
            get => example;
            set
            {
                example = value;
                NotifyPropertyChanged();
            }
        }
        public bool InputValueSupport
        {
            get => ValueSupport;
            set
            {
                ValueSupport = value;
                NotifyPropertyChanged();
            }
        }
        public string? BoolOper { get; set; }                      
        public string Filtervalue { get; set; }                  
        public string? Column { get; set; }            
        


        public ObservableCollection<string> Columns { get; set; }  
        public ObservableCollection<string> AllOperators { get; set; }
        public event PropertyChangedEventHandler PropertyChanged; 

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
