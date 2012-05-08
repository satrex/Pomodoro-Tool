using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Media.Animation;
using System.Windows.Forms;

namespace PomodoroTool
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            //タスクバーに表示されないようにする
            ShowInTaskbar = false;

            //タスクトレイアイコンを初期化する
            notifyIcon = new NotifyIcon();
            notifyIcon.Text = "Pomodoro Tool";
            notifyIcon.Icon = Properties.Resources.pomodoro;

            //タスクトレイに表示する
            this.Hidden = true;
            
            //タスクトレイアイコンのクリックイベントハンドラを登録する
            notifyIcon.MouseClick += (sender, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    //ウィンドウを可視化
                    this.Hidden = false;
                }
            };
            this.IntervalMinutes = 40;
            this.IntervalSeconds = 0;
        }

        public int IntervalMinutes { get; set; }
        public int IntervalSeconds { get; set; }

        private void SetMenues()
        {
            ContextMenuStrip menuStrip = new ContextMenuStrip();

            //アイコンにコンテキストメニューを追加する
            ToolStripMenuItem settingItem = new ToolStripMenuItem();
            settingItem.Text = "設定";
            menuStrip.Items.Add(settingItem);
            settingItem.Click += new EventHandler(settingItem_Click);

            ToolStripMenuItem exitItem = new ToolStripMenuItem();
            exitItem.Text = "終了";
            menuStrip.Items.Add(exitItem);
            exitItem.Click += new EventHandler(exitItem_Click);

            notifyIcon.ContextMenuStrip = menuStrip;
        }

        void settingItem_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private bool Hidden
        {
            set
            {
                Visibility = value ?  System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible;
                //WindowState = value ?  System.Windows.WindowState.Minimized : System.Windows.WindowState.Normal;
                notifyIcon.Visible = value ;
            }
        }

        //ウィンドウが閉じられる前に発生するイベントのハンドラ
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                //閉じるのをキャンセルする
                e.Cancel = true;

                //ウィンドウを非可視にする
                Visibility = System.Windows.Visibility.Collapsed;
            }
            catch { }
        }

        //終了メニューのイベントハンドラ
        private void exitItem_Click(object sender, EventArgs e)
        {
            try
            {
                notifyIcon.Dispose();
                System.Windows.Application.Current.Shutdown();
            }
            catch { }
        }

        private System.Windows.Forms.NotifyIcon notifyIcon;

        BackgroundWorker worker = new BackgroundWorker()
        {
             WorkerSupportsCancellation = true,
             WorkerReportsProgress = true
        };

        private void ResetTimer()
        {
#if DEBUG
            this.RestTime = new TimeSpan(0, 0, 3);
#else
            this.RestTime = new TimeSpan(0, IntervalMinutes, IntervalSeconds);
#endif
        }

        TimeSpan restTime;
        public TimeSpan RestTime
        {
            get { return restTime; }
            set
            {
                restTime = value;
                this.label1.Content = this.GetRestTimeString();
            }

        }

        /// <summary>
        /// ロード時イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.LocateWindowRightDown();
            this.ResetTimer();            
        }

        private void StartTimer()
        {
            this.worker.DoWork += (p, q) =>
            {
                while(!this.IsEnd)
                {
                    if (this.worker.CancellationPending)
                    {
                        q.Cancel = true;
                        return;     
                    }
                    System.Threading.Thread.Sleep(1000);
                    this.worker.ReportProgress(0);
                }
            };

            this.worker.ProgressChanged += (p, q) =>
            {
                this.RestTime = this.RestTime.Subtract(TimeSpan.FromSeconds(1));
            };

            this.worker.RunWorkerCompleted += (p, q) =>
            {
                if (q.Cancelled) return;

                this.Topmost = true;
                this.Alert();
            };

            this.worker.RunWorkerAsync();
        }

        /// <summary>
        /// 画面を、ディスプレイの右下に表示します。
        /// </summary>
        private void LocateWindowRightDown()
        {
            Screen[] screens = Screen.AllScreens;
            this.Top = screens[0].WorkingArea.Height - this.ActualHeight;
            this.Left = screens[0].WorkingArea.Width - this.ActualWidth;
        }

        /// <summary>
        /// 画面を、ディスプレイの中央に表示します。
        /// </summary>
        private void LocateWindowCenter()
        {
            Screen[] screens = Screen.AllScreens;
            this.Top = screens[0].WorkingArea.Height / 2 - this.ActualHeight / 2;
            this.Left = screens[0].WorkingArea.Width / 2 - this.ActualWidth / 2;
        }

        
        /// <summary>
        /// １ポモドーロが終わった際に、ユーザーに通知するための画面処理を行います。
        /// </summary>
        private void Alert()
        {
            this.LocateWindowCenter();
            this.label1.Content = "End";
            this.label1.Margin = new Thickness(40, 20, 40, 20);
            this.label2.Margin = new Thickness(40, 20, 40, 20);

            // Create and animate a Brush to set the button's Background.
            SolidColorBrush myBrush = new SolidColorBrush();
            myBrush.Color = Colors.White;

            ColorAnimation myBackColorAnimation = new ColorAnimation();
            myBackColorAnimation.From = Colors.Black;
            myBackColorAnimation.To = Colors.Yellow;
            myBackColorAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(20));
            myBackColorAnimation.AutoReverse = true;
            myBackColorAnimation.RepeatBehavior = RepeatBehavior.Forever;

            // Apply the animation to the brush's Color property.
            myBrush.BeginAnimation(SolidColorBrush.ColorProperty, myBackColorAnimation);
            this.label1.Foreground = myBrush;
        }

        /// <summary>
        /// タイマーが終了している場合はtrue,それ以外はfalseを返却します。
        /// </summary>
        private bool IsEnd { get { return this.RestTime <= TimeSpan.Zero; } }

        /// <summary>
        /// １ポモドーロの残り時間を、文字列で返却します。
        /// </summary>
        /// <returns></returns>
        private string GetRestTimeString()
        {
            return this.IsEnd ? "00:00" : this.RestTime.Minutes.ToString("00") + ":" + this.RestTime.Seconds.ToString("00");
        }

        /// <summary>
        /// ダブルクリック時のイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            this.Hidden = true;

            //if (!this.IsEnd)
            //{
            //    var result = System.Windows.MessageBox.Show("終了しますか？", "Pomodoro", MessageBoxButton.OKCancel);
            //    if (result == MessageBoxResult.OK)
            //    {
            //        this.Hidden
            //    }
            //}
            //else
            //{
            //    this.Hidden = true;
            //}
        }

        /// <summary>
        /// マウス押下時のイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void MainWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!this.worker.IsBusy)
            {
                this.StartTimer();
            }

        }

        private void SetTimeButton_Click(object sender, RoutedEventArgs e)
        {
            this.StopTimer();
            this.worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted2);
        }

        void worker_RunWorkerCompleted2(object sender, RunWorkerCompletedEventArgs e)
        {
            ResetTimer();
            this.worker.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted2);
        }

        private void StopTimer()
        {
            this.worker.CancelAsync();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            var result = System.Windows.MessageBox.Show("終了しますか？", "Pomodoro", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK) 
            {
                this.Close();
            }
        }

    }
}
