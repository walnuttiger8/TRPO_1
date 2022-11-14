using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TRPO_1.Models;
using TRPO_1.Windows;

namespace TRPO_1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Product> _availableProducts = new List<Product>()
        {
            new Product() { Name="Экспрессо", Price=40f },
            new Product() { Name="Капучино", Price=60f },
            new Product() { Name="Американо", Price=50f },
            new Product() { Name="Чай", Price=40f },
            new Product() { Name="Горчий компот", Price=19f },
        };

        private float _balance = 0.0f;
        public MainWindow()
        {
            InitializeComponent();

            availableProductsListView.ItemsSource = _availableProducts;

            UpdateBalanceTextBlock();
        }

        private void TopUpBalance(MoneyUnit unit)
        {
            _balance += unit.Quantity;
        }

        private void topUpButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new TopUpBalanceWindow();
            var result = window.ShowDialog();

            if(result == true)
            {
                var moneyUnit = window.MoneyUnit;
                TopUpBalance(moneyUnit);

                UpdateBalanceTextBlock();
            }
        }

        private void UpdateBalanceTextBlock()
        {
            balanceTextBlock.Text = $"Баланс: {_balance} р.";
        }
    }
}
