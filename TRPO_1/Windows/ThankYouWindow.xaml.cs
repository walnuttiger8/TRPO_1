using System.Drawing;
using System.Windows;
using System.Windows.Media;
using TRPO_1.Extensions;
using TRPO_1.Models;

namespace TRPO_1.Windows
{
    public partial class ThankYouWindow : Window
    {
        private readonly Window _parent;
        private readonly Product _product;
        
        public ThankYouWindow(Window parent, Product product)
        {
            _parent = parent;
            _product = product;

            WindowStyle = WindowStyle.None;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
            
            ProductImage.Source = _product.ImageSource;
        }

        private void YesButton_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Приятного распития!", "Пожелание", MessageBoxButton.OK, MessageBoxImage.Information);
            
            CloseThis();
        }

        private void NoButton_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ок.", "Ладно", MessageBoxButton.OK, MessageBoxImage.Information);
            
            CloseThis();
        }

        private void CloseThis()
        {
            _parent.IsEnabled = true;
            
            Close();
        }
    }
}