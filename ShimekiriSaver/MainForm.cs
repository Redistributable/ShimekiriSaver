using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShimekiriSaver
{
    /// <summary>
    /// アプリケーションのメインウィンドウを定義します．
    /// </summary>
    public partial class MainForm : Form
    {
        // 非公開フィールド
        private Timer updateTimer;


        // コンストラクタ

        /// <summary>
        /// 新しい MainForm クラスのインスタンスを初期化します．
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            this.updateTimer = new Timer();
            this.updateTimer.Interval = 500;
            this.updateTimer.Tick += UpdateTimer_Tick;

            this.DoubleBuffered = true;
        }


        // 非公開メソッド

        /// <summary>
        /// ウィンドウの更新処理を定義します．
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            this.label1.Text = String.Join("\n\n", MainClass.TimerItems.Select(item => item.GetString()));
        }

        protected override void OnShown(EventArgs e)
        {
            // 基底処理
            base.OnShown(e);

            // タイマー開始
            if (this.updateTimer.Enabled == false)
                this.updateTimer.Start();
        }

        /// <summary>
        /// サイズが変更されたときの処理を定義します．
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSizeChanged(EventArgs e)
        {
            // 基底処理
            base.OnSizeChanged(e);

            // mainPanelを中央へ配置する処理
            int centerPointX = this.ClientSize.Width / 2;
            int centerPointY = this.ClientSize.Height / 2;

            int mainPanelWidthHalf = this.mainPanel.Width / 2;
            int mainPanelHeightHalf = this.mainPanel.Height / 2;

            this.mainPanel.Left = centerPointX - mainPanelWidthHalf;
            this.mainPanel.Top = centerPointY - mainPanelHeightHalf;
        }


        // 公開メソッド

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        public void SetScreen(Screen target)
        {
            var bounds = target.Bounds;

            this.StartPosition = FormStartPosition.Manual;
            this.Location = bounds.Location;
            this.ClientSize = bounds.Size;
        }
    }
}
