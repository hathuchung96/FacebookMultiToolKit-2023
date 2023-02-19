using CmCode;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
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

namespace FBMultiToolKitDemo.UCControl
{
    /// <summary>
    /// Interaction logic for License.xaml
    /// </summary>
    public partial class AccessToken : Window
    {
        private CommonDC t = new CommonDC();

        private SplashScreenManager manager = null;
        public AccessToken()
        {
            InitializeComponent();
            manager = SplashScreenManager.CreateWaitIndicator(new DXSplashScreenViewModel
            {
                IsIndeterminate = true,
                Status = "Đang xử lý...",
            });
        }
        private void HideUx()
        {
            this.IsEnabled = false;
            manager.Show();
        }
        private void ShowUX()
        {
            this.IsEnabled = true;
            manager.Close();
        }
        private async void btnLoginClick()
        {

        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            HideUx();
            txtOutput.Foreground = Brushes.Black;
            string result = t.CheckAccessTokenAsync(txtToken.Text);
            if (t.userAccessToken != "")
            {
                txtOutput.Text = "Đã đăng nhập thành công";
                txtOutput.Foreground = Brushes.Green;
                MainWindow mw = new MainWindow();
                mw.Show();
                this.Close();
            }
            else
            {
                txtOutput.Text = "Vui lòng nhập đúng AccessToken";
                txtOutput.Foreground = Brushes.Red;
            }
            ShowUX();
        }
    }
}
