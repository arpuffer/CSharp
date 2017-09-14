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

namespace AsyncWithProgress
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Threading.CancellationTokenSource tokenSource = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            tokenSource = new System.Threading.CancellationTokenSource();
            System.Threading.CancellationToken token = tokenSource.Token;
            btn_Start.IsEnabled = false;
            btn_Cancel.IsEnabled = true;
            var progress = new Progress<int>(percent =>
            {
                if (percent != null)
                {
                    tb_Progress.Text = percent + "%";
                    pb_Progress.Value = percent;
                }
                else
                {
                    tb_Progress.Text = "Canceled";
                }
            });
            Task first = Task.Run(() => DoProcessing(progress, token));
            try
            {
                await first;
            }
            catch
            {
                tb_Progress.Text = "CANCELED";
                btn_Cancel.IsEnabled = false;
                btn_Start.IsEnabled = true;
                return;
            }

            if (first.Status == TaskStatus.Canceled)
            {
                tb_Progress.Text = "CANCELED";
            }
            else if (first.Status == TaskStatus.Faulted)
            {
                tb_Progress.Text = "ERROR";
            }
            else { tb_Progress.Text = "DONE"; }
            btn_Start.IsEnabled = true;
        }

        public void DoProcessing(IProgress<int> progress, System.Threading.CancellationToken ct)
        {
            int min = 0;
            int max = 100;
            for (int i = min; i <= max; ++i)
            {
                System.Threading.Thread.Sleep(100);
                if ( ct.IsCancellationRequested)
                {
                    progress.Report(0);
                    ct.ThrowIfCancellationRequested();
                }
                if (progress != null)
                    progress.Report(i);
            }
        }

        private void btn_Cancel_click(object sender, RoutedEventArgs e)
        {
            if (tokenSource != null)
            {
                tokenSource.Cancel();
            }
        }
    }
}