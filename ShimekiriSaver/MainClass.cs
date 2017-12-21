using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace ShimekiriSaver
{
    using self = MainClass;

    /// <summary>
    /// 
    /// </summary>
    public static class MainClass
    {
        // 非公開静的フィールド
        private static Point latestMouseLocation;
        private static TimerItem[] timerItems;


        // 公開プロパティ

        /// <summary>
        /// 
        /// </summary>
        public static TimerItem[] TimerItems
        {
            get => self.timerItems;
        }
        

        // 静的コンストラクタ

        /// <summary>
        /// <see cref="MainClass"/> を初期化します．
        /// </summary>
        static MainClass()
        {
            self.latestMouseLocation = Point.Empty;
        }


        // 非公開静的メソッド

        /// <summary>
        /// マウス挙動監視用のメソッド
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Timer_Tick(object sender, EventArgs e)
        {
            if (self.latestMouseLocation == Point.Empty)
                self.latestMouseLocation = Cursor.Position;
            else
            {
                int dx = Math.Abs(self.latestMouseLocation.X - Cursor.Position.X);
                int dy = Math.Abs(self.latestMouseLocation.Y - Cursor.Position.Y);

                if (dx + dy > 5)
                    Environment.Exit(0);
            }
        }

        /// <summary>
        /// スクリーンセーバーを開始します．
        /// </summary>
        private static void startScreenSaver()
        {
            ApplicationContext context = new ApplicationContext();

            var asmPath = Assembly.GetEntryAssembly().Location;
            self.timerItems = TimerItem.LoadFromFile(Path.GetDirectoryName(asmPath) + "\\" + Path.GetFileNameWithoutExtension(asmPath) + ".txt");

            Timer timer = new Timer();
            timer.Interval = 500;
            timer.Tick += Timer_Tick;
            timer.Start();

            foreach (var s in Screen.AllScreens)
            {
                var form = new MainForm();
                form.SetScreen(s);
                form.TopMost = true;
                form.Show();
            }

            Application.Run();
        }


        // 公開静的メソッド

        /// <summary>
        /// アプリケーションのメイン エントリ ポイントを定義します．
        /// </summary>
        /// <param name="args"></param>
        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 引数処理
            Dictionary<string, string> arguments = new Dictionary<string, string>();
            foreach (var arg in args)
            {
                var kv = arg.Split(':');
                string key = kv[0];
                string val = null;
                if (kv.Length >= 2)
                    val = kv[1];

                arguments[key] = val;
            }

            if (arguments.Count() == 0)
                self.startScreenSaver();
            else
                switch (arguments.First().Key)
                {
                    case "/c":
                        MessageBox.Show("このスクリーンセーバーには設定項目がございません．");
                        break;
                    case "/p":
                        break;
                    default:
                        self.startScreenSaver();
                        break;
                }
        }
    }
}
