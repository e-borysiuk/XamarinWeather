using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using XamarinWeather.Models;
using XamarinWeather.Views;

namespace XamarinWeather.ViewModels
{
    public class HistoryViewModel : BaseViewModel
    {
        public IEnumerable<Item> Items { get; set; }
        public Command LoadItemsCommand { get; set; }

        public HistoryViewModel()
        {
            Title = "History";
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            LoadItemsCommand.Execute(null);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                var items = await DataStore.GetItemsAsync(true);
                Items = new List<Item>(items);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}