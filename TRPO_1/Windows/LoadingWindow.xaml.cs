using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TRPO_1.Windows
{
    /// <summary>
    /// Логика взаимодействия для LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window
    {
        private const int _maxStatusValue = 70055020;
        private const int _maxStatusPercent = 14;
        private const double _progressDisplayCoeff = 0.00122863d;
        public LoadingWindow()
        {
            InitializeComponent();

            statusMaxValueTextBlock.Text = _maxStatusValue.ToString();
            InitWorker();
        }

        private void continueButton_Click(object sender, RoutedEventArgs e)
        {
            var window = new MainWindow();
            window.Show();
            Close();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void InitWorker()
        {
            var worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += worker_DoWork;
            worker.ProgressChanged += worker_ProgressChanged;
            worker.RunWorkerCompleted += worker_Completed;

            worker.RunWorkerAsync();
        }

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            for (int i = 0; i <= _maxStatusPercent; i++)
            {
                (sender as BackgroundWorker).ReportProgress(i);
                Thread.Sleep(100);
            }
        }

        private void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            statusProgressBar.Value = e.ProgressPercentage;
            statusPercentTextBlock.Text = e.ProgressPercentage.ToString();
            var value = Convert.ToInt32(_maxStatusValue / 100 * _progressDisplayCoeff * e.ProgressPercentage);
            statusValueTextBlock.Text = value.ToString();
        }

        private void worker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            continueTextBlock.Visibility = Visibility.Visible;
            controlButtonStackPanel.Visibility = Visibility.Visible;
        }
    }
}
