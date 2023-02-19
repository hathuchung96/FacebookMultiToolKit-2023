using CmCode;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using FBMultiToolKitDemo.Model;
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

namespace FBMultiToolKitDemo.UCControl
{
    /// <summary>
    /// Interaction logic for VideoSpy.xaml
    /// </summary>
    public partial class VideoSpy : UserControl
    {
        private SplashScreenManager manager = null;
        private CommonDC tCD = null;
        private List<Post> LPosts = new List<Post>();
        private List<Fanpage> LFanpages = new List<Fanpage>();
        private List<VideoJob> LVideoJobs = new List<VideoJob>();
        public VideoSpy(CommonDC sub)
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

            foreach (var item in tCD.Fanpages)
                LFanpages.Add(item);       

            GCListPost.ItemsSource = LPosts;
            GCFanpage.ItemsSource = LFanpages;
            GCVideoJob.ItemsSource = LVideoJobs;
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

        private void btnGetData_Click(object sender, RoutedEventArgs e)
        {
            HideUx();
            tCD.StartGetPost($"{txtIdFanpage.Text}/posts?fields=created_time,id,type,object_id,message,link,name&");
            LPosts = new List<Post>();
            foreach (var item in tCD.Posts)
                LPosts.Add(item);
            GCListPost.ItemsSource = LPosts;
            GCListPost.RefreshData();
            LVideoJobs.Clear();
            GCVideoJob.RefreshData();
            ShowUX();
        }

        private void btnAddFanpage_Click(object sender, RoutedEventArgs e)
        {
            HideUx();
            var c = GCFanpage.GetFocusedRow() as Fanpage;       
            if (LVideoJobs.FirstOrDefault(x => x.IdFanpageOwner == c.Id) == null && tCD.Posts.Count > 0)
            {
                VideoJob t = new VideoJob();
                t.IdFanpageTarget = txtIdFanpage.Text;
                t.IdFanpageOwner = c.Id;
                t.TotalVideo = GCListPost.VisibleRowCount.ToString();
                t.LastActive = "Hôm nay";
                t.AcessToken = c.AccessToken;
                LVideoJobs.Add(t);
                GCVideoJob.RefreshData();
            }
            ShowUX();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            HideUx();
            int time = txtTimeSelect.SelectedIndex == 0 ? 30 : 60;
            tCD.StartVideoJob(time, txtIdFanpage.Text, LVideoJobs);
            LVideoJobs.Clear();
            GCVideoJob.RefreshData();
            ShowUX();
            MessageBox.Show("Đã xử lý thành công.");
        }

        private void GCVideoJob_KeyDown(object sender, KeyEventArgs e)
        {
            HideUx();
            if (e.Key == System.Windows.Input.Key.Delete)
            {
                var c = GCVideoJob.GetFocusedRow() as VideoJob;
                LVideoJobs.Remove(c);
                GCVideoJob.RefreshData();
            }
            ShowUX();
        }
    }
}
