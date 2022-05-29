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
    public class HandlerFilter : Handler
    {
        private ObservableCollection<Filter> Filters { get; set; }          
        private List<Dictionary<string, object?>> ResultTable { get; set; } 
        public HandlerFilter(RequestManagerViewModel _QueryManager, string collection)
        {
            RequestManager = _QueryManager;
            ResultTable = new List<Dictionary<string, object?>>();
            if (collection == "Filters")
                Filters = RequestManager.Filters;
            else
                Filters = RequestManager.GroupFilters;
        }

        private bool AND(Filter filter)
        {
            try
            {
                switch (filter.Operator)
                {
                    case ">":
                        {
                            RequestManager.ResultTable = RequestManager.ResultTable.Where(item =>
                            double.Parse(item[filter.Column].ToString()) > double.Parse(filter.Filtervalue)
                            ).ToList();
                            return true;
                        }
                    case "<":
                        {
                            RequestManager.ResultTable = RequestManager.ResultTable.Where(item =>
                            double.Parse(item[filter.Column].ToString()) < double.Parse(filter.Filtervalue)).ToList();
                            return true;
                        }
                    case "=":
                        {
                            RequestManager.ResultTable = RequestManager.ResultTable.Where(item =>
                            item[filter.Column].ToString() == filter.Filtervalue).ToList();
                            return true;
                        }
                    case ">=":
                        {
                            RequestManager.ResultTable = RequestManager.ResultTable.Where(item =>
                            double.Parse(item[filter.Column].ToString()) >= double.Parse(filter.Filtervalue)).ToList();
                            return true;
                        }
                    case "<>":
                        {
                            RequestManager.ResultTable = RequestManager.ResultTable.Where(item =>
                            item[filter.Column].ToString() != filter.Filtervalue).ToList();
                            return true;
                        }
                    case "<=":
                        {
                            RequestManager.ResultTable = RequestManager.ResultTable.Where(item =>
                            double.Parse(item[filter.Column].ToString()) <= double.Parse(filter.Filtervalue)).ToList();
                            return true;
                        }
                    case "In Range":
                        {
                            string separator = "..";
                            string[] range = filter.Filtervalue.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            if (range.Count() != 2)
                                return false;

                            RequestManager.ResultTable = RequestManager.ResultTable.Where(item =>
                            {
                                double number = double.Parse(item[filter.Column].ToString());
                                if (number >= double.Parse(range[0]) && number <= double.Parse(range[1]))
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList();
                            return true;
                        }
                    case "Not In Range":
                        {
                            string separator = "..";
                            string[] range = filter.Filtervalue.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            if (range.Count() != 2)
                                return false;

                            RequestManager.ResultTable = RequestManager.ResultTable.Where(item =>
                            {
                                double number = double.Parse(item[filter.Column].ToString());
                                if (number < double.Parse(range[0]) || number > double.Parse(range[1]))
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList();
                            return true;
                        }
                    case "Contains":
                        {
                            RequestManager.ResultTable = RequestManager.ResultTable.Where(item =>
                            {
                                string value = item[filter.Column].ToString().ToUpper().Replace(" ", ""); ;
                                string filterVal = filter.Filtervalue.ToUpper().Replace(" ", "");
                                if (value.IndexOf(filterVal) != -1)
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList();
                            return true;
                        }
                    case "Not Contains":
                        {
                            RequestManager.ResultTable = RequestManager.ResultTable.Where(item =>
                            {
                                string value = item[filter.Column].ToString().ToUpper().Replace(" ", ""); ;
                                string filterVal = filter.Filtervalue.ToUpper().Replace(" ", "");
                                if (value.IndexOf(filterVal) == -1)
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList();
                            return true;
                        }
                    case "Is Null":
                        {
                            RequestManager.ResultTable = RequestManager.ResultTable.Where(item =>
                            {
                                string value = item[filter.Column].ToString();
                                if (value == "None" || value == "0" || value == "0000-00-00 00:00:00"
                                   || value == "00:00" || value.Replace(" ", "") == "" || value.ToUpper() == "NULL"
                                  )
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList();
                            return true;
                        }
                    case "Not Null":
                        {
                            RequestManager.ResultTable = RequestManager.ResultTable.Where(item =>
                            {
                                string value = item[filter.Column].ToString();
                                if (value != "None" && value != "0" && value != "0000-00-00 00:00:00"
                                   && value != "00:00" && value.Replace(" ", "") != "" && value.ToUpper() != "NULL"
                                  )
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList();
                            return true;
                        }
                    case "Belong":
                        {
                            string separator = ",";
                            string[] set = filter.Filtervalue.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            if (set.Count() == 0)
                                return false;

                            for (int i = 0; i < set.Count(); i++)
                            {
                                set[i] = set[i].ToUpper().Replace(" ", "");
                            }

                            RequestManager.ResultTable = RequestManager.ResultTable.Where(item =>
                            {
                                if (set.Contains(item[filter.Column].ToString().ToUpper()))
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList();
                            return true;
                        }
                    case "Not Belong":
                        {
                            string separator = ",";
                            string[] set = filter.Filtervalue.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            if (set.Count() == 0)
                                return false;

                            for (int i = 0; i < set.Count(); i++)
                            {
                                set[i] = set[i].ToUpper().Replace(" ", "");
                            }

                            RequestManager.ResultTable = RequestManager.ResultTable.Where(item =>
                            {
                                if (!set.Contains(item[filter.Column].ToString().ToUpper()))
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList();
                            return true;
                        }
                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private bool OR(Filter filter)
        {
            try
            {
                switch (filter.Operator)
                {
                    case ">":
                        {
                            ResultTable = ResultTable.Union(RequestManager.ResultTable.Where(item =>
                            double.Parse(item[filter.Column].ToString()) > double.Parse(filter.Filtervalue)).ToList()).ToList();
                            return true;
                        }
                    case "<":
                        {
                            ResultTable = ResultTable.Union(RequestManager.ResultTable.Where(item =>
                            double.Parse(item[filter.Column].ToString()) < double.Parse(filter.Filtervalue)).ToList()).ToList();
                            return true;
                        }
                    case "=":
                        {
                            ResultTable = ResultTable.Union(RequestManager.ResultTable.Where(item =>
                            item[filter.Column].ToString() == filter.Filtervalue).ToList()).ToList();
                            return true;
                        }
                    case ">=":
                        {
                            ResultTable = ResultTable.Union(RequestManager.ResultTable.Where(item =>
                            double.Parse(item[filter.Column].ToString()) >= double.Parse(filter.Filtervalue)).ToList()).ToList();
                            return true;
                        }
                    case "<>":
                        {
                            ResultTable = ResultTable.Union(RequestManager.ResultTable.Where(item =>
                            item[filter.Column].ToString() != filter.Filtervalue).ToList()).ToList();
                            return true;
                        }
                    case "<=":
                        {
                            ResultTable = ResultTable.Union(RequestManager.ResultTable.Where(item =>
                            double.Parse(item[filter.Column].ToString()) <= double.Parse(filter.Filtervalue)).ToList()).ToList();
                            return true;
                        }
                    case "In Range":
                        {
                            string separator = "..";
                            string[] range = filter.Filtervalue.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            if (range.Count() != 2)
                                return false;

                            ResultTable = ResultTable.Union(RequestManager.ResultTable.Where(item =>
                            {
                                double number = double.Parse(item[filter.Column].ToString());
                                if (number >= double.Parse(range[0]) && number <= double.Parse(range[1]))
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList()).ToList();
                            return true;
                        }
                    case "Not In Range":
                        {
                            string separator = "..";
                            string[] range = filter.Filtervalue.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            if (range.Count() != 2)
                                return false;

                            ResultTable = ResultTable.Union(RequestManager.ResultTable.Where(item =>
                            {
                                double number = double.Parse(item[filter.Column].ToString());
                                if (number < double.Parse(range[0]) || number > double.Parse(range[1]))
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList()).ToList();
                            return true;
                        }
                    case "Contains":
                        {
                            ResultTable = ResultTable.Union(RequestManager.ResultTable.Where(item =>
                            {
                                string value = item[filter.Column].ToString().ToUpper().Replace(" ", ""); ;
                                string filterVal = filter.Filtervalue.ToUpper().Replace(" ", "");
                                if (value.IndexOf(filterVal) != -1)
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList()).ToList();
                            return true;
                        }
                    case "Not Contains":
                        {
                            ResultTable = ResultTable.Union(RequestManager.ResultTable.Where(item =>
                            {
                                string value = item[filter.Column].ToString().ToUpper().Replace(" ", ""); ;
                                string filterVal = filter.Filtervalue.ToUpper().Replace(" ", "");
                                if (value.IndexOf(filterVal) == -1 )
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList()).ToList();
                            return true;
                        }
                    case "Is Null":
                        {
                            ResultTable = ResultTable.Union(RequestManager.ResultTable.Where(item =>
                            {
                                string value = item[filter.Column].ToString();
                                if (value == "None" || value == "0" || value == "0000-00-00 00:00:00"
                                   || value == "00:00" || value.Replace(" ", "") == "" || value.ToUpper() == "NULL"
                                  )
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList()).ToList();
                            return true;
                        }
                    case "Not Null":
                        {
                            ResultTable = ResultTable.Union(RequestManager.ResultTable.Where(item =>
                            {
                                string value = item[filter.Column].ToString();
                                if (value != "None" && value != "0" && value != "0000-00-00 00:00:00"
                                   && value != "00:00" && value.Replace(" ", "") != "" && value.ToUpper() != "NULL"
                                  )
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList()).ToList();
                            return true;
                        }
                    case "Belong":
                        {
                            string separator = ",";
                            string[] set = filter.Filtervalue.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            if (set.Count() == 0)
                                return false;

                            for (int i = 0; i < set.Count(); i++)
                            {
                                set[i] = set[i].ToUpper().Replace(" ", "");
                            }

                            ResultTable = ResultTable.Union(RequestManager.ResultTable.Where(item =>
                            {
                                if (set.Contains(item[filter.Column].ToString().ToUpper()))
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList()).ToList();
                            return true;
                        }
                    case "Not Belong":
                        {
                            string separator = ",";
                            string[] set = filter.Filtervalue.Split(separator, StringSplitOptions.RemoveEmptyEntries);
                            if (set.Count() == 0)
                                return false;

                            for (int i = 0; i < set.Count(); i++)
                            {
                                set[i] = set[i].ToUpper().Replace(" ", "");
                            }

                            ResultTable = ResultTable.Union(RequestManager.ResultTable.Where(item =>
                            {
                                if (!set.Contains(item[filter.Column].ToString().ToUpper()))
                                    return true;
                                else
                                    return false;
                            }
                            ).ToList()).ToList();
                            return true;
                        }
                    default:
                        return false;
                }
            }
            catch
            {
                return false;
            }
        }

        private void ResultByUnion()
        {
            foreach (Filter filter in Filters)
            {
     
                if (filter.Filtervalue.Replace(" ", "") == "" && filter.Operator != "Is Null" && filter.Operator != "Not Null")
                {
                    RequestManager.IsRequestSuccess = false;
                    return;
                }
                else
                {
                    if (!OR(filter))
                    {
                        RequestManager.IsRequestSuccess = false;
                        return;
                    }
                    else
                    {
                        RequestManager.IsRequestSuccess = true;
                    }
                }
            }
            RequestManager.ResultTable = ResultTable;
        }


        private void ResultByChain()
        {
            if (Filters.Count == 1 && Filters[0].Filtervalue == "" && Filters[0].Operator == "" && Filters[0].Column == "")
            {
                RequestManager.IsRequestSuccess = true;
                return;
            }

            foreach (Filter filter in Filters)
            {
                if (filter.Filtervalue.Replace(" ", "") == "" && filter.Operator != "Is Null" && filter.Operator != "Not Null")
                {
                    RequestManager.IsRequestSuccess = false;
                    return;
                }
                else
                {

                    if (!AND(filter))
                    {
                        RequestManager.IsRequestSuccess = false;
                        return;
                    }
                    else
                    {
                        RequestManager.IsRequestSuccess = true;
                    }
                }
            }
        }

        public override void Try()
        {
            if (RequestManager != null)
            {
                if (Filters.Count != 0)
                {
                    if (Filters.Count > 1)
                    {
                        if (Filters[1].BoolOper == "AND")
                            ResultByChain();
                        else
                            ResultByUnion();
                    }
                    else
                    {
                        ResultByChain();
                    }
                    if (next != null && RequestManager.IsRequestSuccess != false)
                    {
                        next.Try();
                    }
                }
                else if (next != null && RequestManager.SelectedColumns.Count != 0)
                {
                    next.Try();
                }
                else
                    RequestManager.IsRequestSuccess = true;
            }
            else
            {
                return;
            }
        }
    }
}
