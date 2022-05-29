// GroupHandler
// Звено, реализующее группировку результирующего массива записей

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
    public class HandlerGroup : Handler
    {
        public HandlerGroup(RequestManagerViewModel RequestManager)
        {
            this.RequestManager = RequestManager;
        }

        public override void Try()
        {
            if (RequestManager != null)
            {
                if (RequestManager.GroupingColumn != null)
                {
                    try
                    {
                        var result = RequestManager.ResultTable.GroupBy(item => item[RequestManager.GroupingColumn]).ToList();
                        RequestManager.ResultTable.Clear();
                        foreach (var group in result)
                        {
                            foreach (Dictionary<string, object?> row in group)
                            {
                                RequestManager.ResultTable.Add(row);
                            }
                        }

                        if (RequestManager.ResultTable.Count != 0)
                        {
                            RequestManager.IsRequestSuccess = true;

                            if (next != null)
                            {
                                next.Try();
                            }
                        }

                        else
                        {
                            RequestManager.IsRequestSuccess = false;
                            return;
                        }
                    }
                    catch
                    {
                        RequestManager.IsRequestSuccess = false;
                        return;
                    }
                }
                else if (RequestManager.GroupFilters.Count == 1 && RequestManager.GroupFilters[0].Filtervalue == ""
                        && RequestManager.GroupFilters[0].Operator == "" && RequestManager.GroupFilters[0].Column == "")
                {
                    RequestManager.IsRequestSuccess = true;
                    return;
                }
                else
                {
                    RequestManager.IsRequestSuccess = false;
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }
}
