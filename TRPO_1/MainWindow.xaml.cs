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
using TRPO_1.Services;
using TRPO_1.Windows;

namespace TRPO_1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DrinksService _drinksService;
        private List<Product> _availableProducts = new List<Product>()
        {
            new Product() { Name="Экспрессо", Price=40f },
            new Product() { Name="Капучино", Price=60f },
            new Product() { Name="Американо", Price=50f },
            new Product() { Name="Чай", Price=40f },
            new Product() { Name="Горчий компот", Price=19f },
        };
        public MainWindow()
        {
            InitializeComponent();

            availableProductsListView.ItemsSource = _availableProducts;

            _drinksService = new DrinksService();
            _drinksService.BalanceChanged += OnBalanceChanged;
        }

        private void TopUpBalance(MoneyUnit unit)
        {
            _drinksService.TopUp(unit);
        }

        private void BuyProduct(Product product)
        {
            var success = _drinksService.Buy(product);
            if (!success)
            {
                MessageBox.Show("Недостаточно средств");
            }
        }

        private void topUpButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new TopUpBalanceWindow();
            var result = window.ShowDialog();

            if(result == true)
            {
                var moneyUnit = window.MoneyUnit;
                TopUpBalance(moneyUnit);
            }
        }

        private void OnBalanceChanged(int prevValue, int value)
        {
            UpdateBalanceTextBlock();
        }

        private void UpdateBalanceTextBlock()
        {
            balanceTextBlock.Text = $"Баланс: {_drinksService.Balance} р.";
        }

        private async void buyButton_Click(object sender, RoutedEventArgs e)
        {
            var product = (Product)availableProductsListView.SelectedItem;
            if (product != null)
            {
                BuyProduct(product);
            }
            else
            {
                balanceTextBlock.Text = "Выберите продукт";
            } 
        }

        private void getChangeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
