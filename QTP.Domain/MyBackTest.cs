using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using QTP.TAlib;
using GMSDK;

namespace QTP.Domain
{
    public class MyBackTest
    {
        private Stopwatch watch = new Stopwatch();
        private MyStrategy strategy;
        private List<int> periods;
        private bool pause = false;
       
        // get from parameters
        private bool useLocal = true;
        private DateTime startTime, endTime;
        private int interval = 20;
        private bool save = false;
        

        // committe

        // Bars Store
        private Dictionary<string , List<DailyBar>> dictDailyBars = new Dictionary<string,List<DailyBar>>();
        private Dictionary<string, List<Bar>> dictBars = new Dictionary<string, List<Bar>>();

        // KLine Store
        private Dictionary<string , List<KLine>> dictDailyKs;
        private Dictionary<string, List<KLine>> dictKs;

        #region public methods
        public MyBackTest(MyStrategy strategy, Dictionary<string, string> parameters, List<int> periods)
        {
            this.strategy = strategy;
            this.periods = periods;

            if (parameters.ContainsKey("UseLocal")) useLocal = parameters["UseLocal"] == "1";

            startTime = Convert.ToDateTime(parameters["Start"]);
            endTime = Convert.ToDateTime(parameters["End"]);

            if (parameters.ContainsKey("Interval")) interval = Convert.ToInt32(parameters["Interval"]);
            if (parameters.ContainsKey("Save")) save = parameters["Save"] == "1";
        }

        public void Run()
        {
            Task taskData = new Task(GetData);
            taskData.Start();
            
            // Producer
            Task taskProducer = taskData.ContinueWith(Producer);
        }

        public void Continue()
        {
            pause = false;
        }

        // GetBars()
        public void GetData()
        {
            Pause(string.Format("将要取数据(Local={0})(Save={1})", useLocal, save));

            watch.Reset(); watch.Start();
            if (useLocal)
            {
                // read files to  Dictionary of KLine
                Stream fStream = new FileStream(@"Data\Daily.dat", FileMode.Open);
                BinaryFormatter binFormat = new BinaryFormatter();//创建二进制序列化器
                dictDailyKs = (Dictionary<string, List<KLine>>)binFormat.Deserialize(fStream);

                fStream.Close();
                fStream = new FileStream(@"Data\Bars.dat", FileMode.Open);
                dictKs = (Dictionary<string, List<KLine>>)binFormat.Deserialize(fStream);
            }
            else
            {
                GetDailyBars();
                GetBars();

                if (save == true)
                    SaveToFile();
            }

            watch.Stop();
            strategy.WriteMDLog(string.Format("获取数据用时({0}秒)", watch.ElapsedMilliseconds / 1000));

        }


        public void Producer(Task t)
        {
            Pause(string.Format("将要开始生产数据..."));
            watch.Reset(); watch.Start();

            if (useLocal)
                ProducerFromKLines();
            else
                ProducerFromBars();

            watch.Stop();
            strategy.WriteMDLog(string.Format("生产数据完成。用时({0})", watch.ElapsedMilliseconds));
        }

        #endregion

        #region private methods during producer

        private void ProducerFromKLines()
        {
            int count = 0;
            foreach (string dtString in dictDailyKs.Keys)
            {
                List<KLine> ks = dictKs[dtString];

                foreach (KLine k in ks)
                {
                    Monitor m = strategy.GetMonitor(k.Symbol);

                    m.TA.PushKLine(1, k);
                }

                List<KLine> dailyKs = dictDailyKs[dtString];
                foreach (KLine k in dailyKs)
                {
                    Monitor m = strategy.GetMonitor(k.Symbol);
                    m.TA.PushKLine(0, k);
                }

                count++;              
                if (interval != 0 && count % interval == 0)
                {
                    Pause(string.Format("刚推送完{0}的KLines(已处理了{1}天)", dtString, count));
                }
            }
        }

        private void ProducerFromBars()
        {
            int count = 0;          // when count = interval, then break
            Dictionary<string, float> dictFactor = new Dictionary<string, float>();
            foreach (string dtString in dictDailyBars.Keys)
            {
                List<DailyBar> dailyBars = dictDailyBars[dtString];

                // Create dictFactor (symbol -- factor)               
                foreach (DailyBar daily in dailyBars)
                {
                    string symbol = string.Format("{0}.{1}", daily.exchange, daily.sec_id);
                    dictFactor[symbol] = daily.adj_factor;
                }

                // 推送一天的所有Bar
                List<Bar> bars = dictBars[dtString];
                foreach (Bar bar in bars)
                {
                    string symbol = string.Format("{0}.{1}", bar.exchange, bar.sec_id);
                    Monitor m = strategy.GetMonitor(symbol);
                    m.PushBar(bar, dictFactor[symbol]);
                }

                // 推送一天的所有日线bar
                foreach (DailyBar daily in dailyBars)
                {
                    string symbol = string.Format("{0}.{1}", daily.exchange, daily.sec_id);
                    Monitor m = strategy.GetMonitor(symbol);
                    m.PushDailyBar(daily);
                }

                count++;
                if (interval != 0 && count % interval == 0)
                {
                    Pause(string.Format("刚推送完{0}的Bars(已处理了{1}天)", dtString, count));
                }
            }
        }

        #endregion

        #region private methods
        private void GetDailyBars()
        {
            // GetDailyBar
            var dailyBars = strategy.GetDailyBars(strategy.Symbols, Utils.DTString(startTime), Utils.DTString(endTime));

            // Create DictDailyBars
            DateTime dt = startTime;
            List<DailyBar> bars = new List<DailyBar>();
            foreach (DailyBar bar in dailyBars)
            {
                DateTime barDT = Utils.UtcToDateTime(bar.utc_time);
                if (barDT > dt)
                {
                    dictDailyBars[Utils.DTString(dt)] = bars;
                    bars = new List<DailyBar>();
                    bars.Add(bar);
                    dt = barDT;
                }
                else if (barDT == dt) // equal
                {
                    bars.Add(bar);
                }
                else
                {
                    strategy.WriteMDLog(string.Format("DailyBar({0}) 次序错误)", Utils.DTString(dt)));
                }
            }
            dictDailyBars[Utils.DTString(dt)] = bars;

            strategy.WriteMDLog(string.Format("起止日期({0}-{1}), 天数为({2})", Utils.DTString(startTime), Utils.DTString(dt), dictDailyBars.Count));
        }

        private void GetBars()
        {
            // Bars
            foreach (string dtString in dictDailyBars.Keys)
            {
                DateTime dt = Convert.ToDateTime(dtString);
                DateTime dt2 = new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);

                var bars = strategy.GetBars(strategy.Symbols, 60, Utils.DTLongString(dt), Utils.DTLongString(dt2));
                dictBars[dtString] = bars;

                strategy.WriteMDLog(string.Format("{0}(周{1}) Bars 数({2})", dtString, (int)dt.DayOfWeek, bars.Count));
            }
        }

        private void SaveToFile()
        {
            Dictionary<string, float> dictFactor = new Dictionary<string, float>();

            dictDailyKs = new Dictionary<string, List<KLine>>();
            dictKs = new Dictionary<string, List<KLine>>();
            foreach (string dtString in dictDailyBars.Keys)
            {
                List<DailyBar> dailyBars = dictDailyBars[dtString];
                List<KLine> ks = new List<KLine>();

                foreach (DailyBar daily in dailyBars)
                {
                    string symbol = string.Format("{0}.{1}", daily.exchange, daily.sec_id);
                    Monitor m = strategy.GetMonitor(symbol);
                    dictFactor[symbol] = daily.adj_factor;      // set adj_factor for Bar to KLine

                    ks.Add(m.GetKLine(daily));                   
                }
                dictDailyKs[dtString] = ks;

                List<Bar> bars = dictBars[dtString];
                ks = new List<KLine>();
                foreach (Bar bar in bars)
                {
                    string symbol = string.Format("{0}.{1}", bar.exchange, bar.sec_id);
                    Monitor m = strategy.GetMonitor(symbol);

                    ks.Add(m.GetKLine(bar, dictFactor[symbol]));
                }
                dictKs[dtString] = ks;
            }
            
            Stream fStream = new FileStream(@"Data\Daily.dat", FileMode.Create);
            BinaryFormatter binFormat = new BinaryFormatter();//创建二进制序列化器
            binFormat.Serialize(fStream, dictDailyKs);

            fStream.Close();
            fStream = new FileStream(@"Data\Bars.dat", FileMode.Create);
            binFormat.Serialize(fStream, dictKs);
            fStream.Close();
        }

        private void Pause(string msg)
        {
            strategy.WriteMDLog(msg, true);
            pause = true;
            while (pause)
            {
                System.Threading.Thread.Sleep(1000);
            }
        }

        #endregion

        //private void CalculateBar(Tick tick)
        //{
        //    if (semiBar == null)
        //    {
        //        semiBar = new Bar();
        //        bar1M = new Bar();
        //        isFirstTick = true;
        //        lastMinute = Utils.UtcToDateTime(tick.utc_time).Minute;
        //    }

        //    // judge minute changed
        //    DateTime dt = Utils.UtcToDateTime(tick.utc_time);
        //    if (dt.Minute > lastMinute)
        //    {
        //        isFirstTick = true;
        //        lastMinute = dt.Minute;

        //        // copy semiBar to Bar
        //        bar1M.high = semiBar.high;
        //        bar1M.low = semiBar.low;
        //        bar1M.open = semiBar.open;
        //        bar1M.close = semiBar.close;
        //        bar1M.volume = semiBar.volume;
        //        bar1M.strtime = string.Format("{0:00}:{1:00}", DateTime.Now.Minute, DateTime.Now.Second);

        //        // clear semibar
        //        semiBar.high = semiBar.low = semiBar.open = semiBar.close = 0;
        //        semiBar.volume = 0.0;
        //    }

        //    // open
        //    if (isFirstTick)
        //    {
        //        semiBar.high = semiBar.low = semiBar.open = semiBar.close = tick.last_price;
        //    }

        //    // high
        //    if (semiBar.high < tick.last_price) semiBar.high = tick.last_price;
        //    // low
        //    if (semiBar.low > tick.last_price) semiBar.low = tick.last_price;
        //    // close
        //    semiBar.close = tick.last_price;
        //    // volumn
        //    semiBar.volume += tick.last_volume;

        //    isFirstTick = false;

        //}

        
    }
}
