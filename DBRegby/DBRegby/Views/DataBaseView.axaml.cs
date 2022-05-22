using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Interactivity;
using DBRegby.ViewModels;

namespace DBRegby.Views
{
    public partial class DataBaseView : UserControl
    {
        public DataBaseView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
