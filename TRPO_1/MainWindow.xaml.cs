using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using TRPO_1.Models;
using TRPO_1.Services;
using TRPO_1.Windows;
using Task = System.Threading.Tasks.Task;
using Window = System.Windows.Window;

namespace TRPO_1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DrinksService _drinksService;
        private MoneyPrinterService _moneyPrinterService = new MoneyPrinterService();
        

        #region Параметры системы
        private const float _discountPercent = 15;
        private const int _flushDelay = 1000;

        private readonly List<Product> _availableProducts = new List<Product>()
        {
            new Product() { Name="Экспрессо", Price=40f, Image = Resource.espresso},
            new Product() { Name="Капучино", Price=60f, Image = Resource.cappuccino},
            new Product() { Name="Американо", Price=50f, Image = Resource.americano},
            new Product() { Name="Чай", Price=40f, Image = Resource.tea},
            new Product() { Name="Горчий компот", Price=19f, Image = Resource.cumpot},
        };
        #endregion

        #region Состояние
        private bool _discountApplied = false;
        #endregion
        public MainWindow()
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            
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

        private async void ApplyDiscount()
        {
            if (_discountApplied)
            {
                await FlushMessage("Скидка уже использована");
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

        private async void BuyProduct(Product product)
        {
            try
            {
                _drinksService.Buy(product);
            }
            catch (AfricanException)
            {
                await FlushMessage("Недостаточно воды");
                return;
            }
            catch (RussianException)
            {
                await FlushMessage("Недостаточно средств");
                return;
            }

            var thankYou = new ThankYouWindow(this, product);

            IsEnabled = false;
            thankYou.Show();
            
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
                PrintMoney("Ваша сдача", "Спасибо за покупку!", change);
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
            _drinksService.Buy(new Product()
            {
                Price=_drinksService.Balance
            });
        }

        private void discountButton_Click(object sender, RoutedEventArgs e)
        {
            ApplyDiscount();
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

        private void managerKeyButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new MonitoringWindow(_drinksService);
            window.Show();
        }
        
        private void PrintMoney(string title, string caption, List<MoneyUnit> moneyUnits)
        {
            var document = _moneyPrinterService.PrintMoneyReport(title, caption, moneyUnits);

            document.Visible = true;
        }
    }
}
