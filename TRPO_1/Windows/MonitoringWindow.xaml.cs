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
using TRPO_1.Services;
using static TRPO_1.Services.DrinksService;

namespace TRPO_1.Windows
{
    /// <summary>
    /// Логика взаимодействия для MonitoringWindow.xaml
    /// </summary>
    public partial class MonitoringWindow : Window
    {
        private DrinksService drinksService;
        public MonitoringWindow(DrinksService drinksService)
        {
            InitializeComponent();
            this.drinksService = drinksService;
            this.drinksService.VolumeChanged += OnVolumeChanged;
            this.drinksService.BalanceChanged += OnBakanceChanged;

            volumeProgressBar.Maximum = drinksService.MaxVolume;
            volumeProgressBar.Minimum = drinksService.MinVolume;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            Update();
        }

        private void Update()
        {
            userBalanceTextBlock.Text = drinksService.Balance.ToString();
            volumeTextBlock.Text = drinksService.Volume.ToString();
            serviceBalanceTextBlock.Text = drinksService.ServiceBalance.ToString();
            volumeProgressBar.Value = drinksService.Volume;
        }

        private void OnVolumeChanged(float prevValue, float currentValue)
        {
            Update();
        }
        private void OnBakanceChanged(int prevValue, int currentValue)
        {
            Update();
        }
    }
}
