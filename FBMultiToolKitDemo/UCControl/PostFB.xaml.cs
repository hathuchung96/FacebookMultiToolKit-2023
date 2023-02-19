using CmCode;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using FBMultiToolKitDemo.Model;
using Newtonsoft.Json.Linq;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace FBMultiToolKitDemo.UCControl
{
    /// <summary>
    /// Interaction logic for PostFB.xaml
    /// </summary>
    public partial class PostFB : UserControl
    {
        private SplashScreenManager manager = null;
        private CommonDC tCD = null;
        private List<Fanpage> LFanpages = new List<Fanpage>();
        private List<FanpageJob> LFanpageJobs = new List<FanpageJob>();
        public PostFB(CommonDC sub)
        {
            InitializeComponent();
            manager = SplashScreenManager.CreateWaitIndicator(new DXSplashScreenViewModel
            {
                IsIndeterminate = true,
                Status = "Đang xử lý...",
            });
            this.tCD = sub;
            initFirstLoad();
        }
        private void initFirstLoad()
        {
            HideUx();
            txtTimeSelect.Items.Add("30 sec");
            txtTimeSelect.Items.Add("1 min");
            txtTimeSelect.SelectedIndex = 0;

            foreach(var item in tCD.Fanpages)
                LFanpages.Add(item);

            GCFanpage.ItemsSource = LFanpages;
            GCFanpageJob.ItemsSource = LFanpageJobs;
            ShowUX();
        }
        private void HideUx()
        {
            GridMain.IsEnabled = false;
            manager.Show();
        }
        private void ShowUX()
        {
            GridMain.IsEnabled = true;
            manager.Close();
        }
        private void btnAddFanpage_Click(object sender, RoutedEventArgs e)
        {
            HideUx();
            var c = GCFanpage.GetFocusedRow() as Fanpage;
            FanpageJob t = new FanpageJob();
            t.LinkShare = txtLink.Text;
            t.Message = txtTitle.Text;
            t.IdFanpageOwner = c.Id;
            t.AcessToken = c.AccessToken;
            LFanpageJobs.Add(t);
            GCFanpageJob.RefreshData();
            ShowUX();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            HideUx();
            if (String.IsNullOrEmpty(btnDateTime.EditValue?.ToString()))
            {
                ShowUX();
                MessageBox.Show("Vui lòng chọn ngày lên lịch.");
                return;
            }
            DateTime datetime = (DateTime)btnDateTime.EditValue;
            DateTime datenow = DateTime.Now;
            if (datetime <= datenow)
            {
                ShowUX();
                MessageBox.Show("Vui lòng không chọn ngày trong quá khứ.");
                return;
            }
            int time = txtTimeSelect.SelectedIndex == 0 ? 30 : 60;
            tCD.StartPostJob(time, datetime, LFanpageJobs);
            LFanpageJobs.Clear();
            GCFanpageJob.RefreshData();
            ShowUX();
            MessageBox.Show("Đã xử lý thành công.");
        }

        private void GCFanpageJob_KeyDown(object sender, KeyEventArgs e)
        {
            HideUx();
            if (e.Key == System.Windows.Input.Key.Delete)
            {
                var c = GCFanpageJob.GetFocusedRow() as FanpageJob;
                LFanpageJobs.Remove(c);
                GCFanpageJob.RefreshData();
            }
            ShowUX();
        }
    }
}
