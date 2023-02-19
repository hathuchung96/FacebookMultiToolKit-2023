using CmCode;
using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using DevExpress.XtraBars.Docking;
using FBMultiToolKitDemo.UCControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MessageBox = System.Windows.MessageBox;

namespace FBMultiToolKitDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ThemedWindow
    {
        private PostFB tabFacebook = null;
        private VideoSpy tabVideo = null;
        private SplashScreenManager manager = null;
        private CommonDC t = new CommonDC();
        public MainWindow()
        {          
            InitializeComponent();
            manager = SplashScreenManager.CreateWaitIndicator(new DXSplashScreenViewModel
            {
                IsIndeterminate = true,
                Status = "Đang xử lý...",
            });
            t.InitCommonDC();
            if (t.License != "")
            {
                LicenseCode.Content = t.License;
                LicenseCode.Foreground = Brushes.Green;
            } else
            {
                License frmLicense = new License();
                frmLicense.Show();
                this.Close();
            }
            if (t.userAccessToken != "")
            {
                //AccessTokenCode.Content =t.userAccessToken;
            }
            else
            {
                AccessToken frmAccessToken = new AccessToken();
                frmAccessToken.Show();
                this.Close();
            }
        }
        private void MainControl_SelectedPageChanged(object sender, DevExpress.Xpf.Ribbon.RibbonPropertyChangedEventArgs e)
        {
            try
            {
                if (DocView != null)
                {
                    manager.Show();
                    DocView.Children.Clear();
                    if (MainControl.SelectedPage == SpyTab)
                    {
                        if (tabVideo == null)
                        {
                            tabVideo = new VideoSpy(t);
                        }
                        DocView.Children.Add(tabVideo);
                        tabVideo.UpdateLayout();
                    }
                    else if (MainControl.SelectedPage == PostTab)
                    {

                        if (tabFacebook == null)
                        {
                            tabFacebook = new PostFB(t);
                        }
                        DocView.Children.Add(tabFacebook);
                    }
                    manager.Close();
                }
            }
            catch (Exception ae)
            {
                MessageBox.Show(ae.Message);
            }
        }

        private void MainPanel_Loaded(object sender, RoutedEventArgs e)
        {
            PostTab.IsSelected = true;
            SpyTab.IsSelected = true;
        }
    }
}
