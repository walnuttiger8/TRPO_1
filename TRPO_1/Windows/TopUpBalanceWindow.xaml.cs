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
using System.Windows.Shapes;
using TRPO_1.Models;

namespace TRPO_1.Windows
{
    /// <summary>
    /// Логика взаимодействия для TopUpBalanceWindow.xaml
    /// </summary>
    public partial class TopUpBalanceWindow : Window
    {
        private List<MoneyUnit> _moneyUnits = new List<MoneyUnit>()
        {
            new MoneyUnit() {Quantity=500, Image=Resource.r500},
            new MoneyUnit() {Quantity=100, Image=Resource.r100},
            new MoneyUnit() {Quantity=50, Image=Resource.r50},
            new MoneyUnit() {Quantity=10, Image=Resource.r10},
            new MoneyUnit() {Quantity=5, Image=Resource.r5},
            new MoneyUnit() {Quantity=2, Image=Resource.r2},
            new MoneyUnit() {Quantity=1, Image=Resource.r1},
        };

        public MoneyUnit MoneyUnit { get; set; }

        public TopUpBalanceWindow()
        {
            InitializeComponent();

            moneyUnitsListView.ItemsSource = _moneyUnits;
        }

        private void topUpButton_Click(object sender, RoutedEventArgs e)
        {
            var unit = (MoneyUnit)moneyUnitsListView.SelectedItem;

            if (unit != null)
            {
                MoneyUnit = unit;
                DialogResult = true;
                Close();
            } else
            {
                MessageBox.Show("Выберите денежную единицу");
            }
        }
    }
}
