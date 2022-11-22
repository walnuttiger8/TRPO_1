using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        #region Параметры системы
        private const float _discountPercent = 15;
        private const int _flushDelay = 1000;

        private readonly List<Product> _availableProducts = new List<Product>()
        {
            new Product() { Name="Экспрессо", Price=40f },
            new Product() { Name="Капучино", Price=60f },
            new Product() { Name="Американо", Price=50f },
            new Product() { Name="Чай", Price=40f },
            new Product() { Name="Горчий компот", Price=19f },
        };
        #endregion

        #region Состояние
        private bool _discountApplied = false;
        #endregion
        public MainWindow()
        {
            InitializeComponent();
            InitializeProducts();

            _drinksService = new DrinksService();
            _drinksService.BalanceChanged += OnBalanceChanged;

            UpdateBalanceTextBlock();
        }

        private void InitializeProducts(ObservableCollection<Product> products)
        {
            availableProductsListView.ItemsSource = products;
        }

        private void InitializeProducts()
        {
            InitializeProducts(new ObservableCollection<Product>(_availableProducts));
        }

        private void ApplyDiscount()
        {
            if (_discountApplied)
            {
                MessageBox.Show("Скидка уже использована");
                return;
            }
            var productsWithDiscountedPrice = GetProducts();
            productsWithDiscountedPrice.ForEach(x => x.Price = CalculatePriceWithDiscount(x.Price, _discountPercent));

            var products = new ObservableCollection<Product>(productsWithDiscountedPrice);
            InitializeProducts(products);
            _discountApplied = true;
        }

        private static float CalculatePriceWithDiscount(float price, float discount)
        {
            return Convert.ToInt32(price - (price * (discount / 100)));
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
                return;
            }
            InitializeProducts();
            _discountApplied = false;
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
                await FlushMessage("Выберите продукт");
            }
        }

        private async Task FlushMessage(string message, int delay=_flushDelay)
        {
            balanceTextBlock.Text = message;
            await Task.Delay(delay);
            UpdateBalanceTextBlock();
        }

        private void GetChange()
        {
            var change = _drinksService.GetChange();

            if (change != null)
            {
                foreach (var unit in change)
                {
                    MessageBox.Show(unit.Quantity.ToString());
                }
            }
            else
            {
                MessageBox.Show("Нет возможности выдать сдачу наличными, введите данные карты для перевода");
            }
        }

        private void getChangeButton_Click(object sender, RoutedEventArgs e)
        {
            GetChange();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            GetChange();
        }

        private void discountButton_Click(object sender, RoutedEventArgs e)
        {
            ApplyDiscount();
        }

        private void DEBUG_ShowAvailableProductPrices()
        {
            foreach (var product in _availableProducts)
            {
                MessageBox.Show(product.Price.ToString());
            }
        }

        private List<Product> GetProducts()
        {
            var result = new List<Product>();
            foreach (var product in _availableProducts)
            {
                result.Add(product.Clone());
            }
            return result;
        }
    }
}
