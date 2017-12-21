using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ShimekiriSaver
{
    public class TimerItem
    {
        // 非公開フィールド
        private string name;
        private DateTime targetTime;


        // 公開プロパティ

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get => this.name;
            set => this.name = value;
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime TargetTime
        {
            get => this.targetTime;
            set => this.targetTime = value;
        }


        // コンストラクタ

        /// <summary>
        /// 新しい TimerItem クラスのインスタンスを初期化します．
        /// </summary>
        public TimerItem()
        {
            this.name = "untitled";
            this.targetTime = DateTime.Now;
        }


        // 公開メソッド

        /// <summary>
        /// 文字列化したものを取得します．
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            var diff = (DateTime.Now - this.targetTime).Negate();
            if (diff.TotalSeconds < 0)
                return this.name + " は終了しました";
            else
                return this.name + " まで " + diff.TotalDays.ToString("00") + "日" + diff.Hours.ToString("00") + "時間" + diff.Minutes.ToString("00") + "分" + diff.Seconds.ToString("00") + "秒";
        }


        // 公開静的メソッド

        /// <summary>
        /// ファイルからロードします．
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static TimerItem[] LoadFromFile(string path)
        {
            string rawData = null;
            try
            {
                rawData = File.ReadAllText(path);
            }
            catch (Exception ex)
            {
                throw new TimerItemException("ファイルの読み込みに失敗しました．", ex);
            }

            rawData = rawData.Replace("\r\n", "\n");
            var items = new List<TimerItem>();
            foreach (var line in rawData.Split('\n').Select(str => str.Split(',')))
            {
                DateTime dt = DateTime.MinValue;
                try
                {
                    var dtTemp = line[0].Split('-');

                    int ye = Int32.Parse(dtTemp[0]);
                    int mo = Int32.Parse(dtTemp[1]);
                    int da = Int32.Parse(dtTemp[2]);
                    int ho = Int32.Parse(dtTemp[3]);
                    int mi = Int32.Parse(dtTemp[4]);
                    int se = Int32.Parse(dtTemp[5]);

                    dt = new DateTime(ye, mo, da, ho, mi, se);
                }
                catch (Exception ex)
                {
                    throw new TimerItemException("日付データの読み取りに失敗しました．", ex);
                }

                string name = null;
                try
                {
                    name = line[1];
                }
                catch (Exception ex)
                {
                    throw new TimerItemException("名前データの読み取りに失敗しました．", ex);
                }

                items.Add(new TimerItem() { Name = name, TargetTime = dt });
            }

            return items.ToArray();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class TimerItemException : Exception
    {
        /// <summary>
        /// 使用しないでください
        /// </summary>
        private TimerItemException()
        {
            // 実装無し
        }

        /// <summary>
        /// メッセージを指定して，新しい TimerItemException クラスのインスタンスを初期化します．
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public TimerItemException(string message, Exception innerException)
            : base(message, innerException)
        {
            // 実装無し
        }
    }
}
