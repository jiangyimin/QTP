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
        internal class Zone
        {
            public int x { get; set;}

            public int lastM { get; set; }
            public void Clear()
            {
                x  = lastM = 0;
            }
        }

        private Stopwatch watch = new Stopwatch();
        private MyStrategy strategy;
        private List<int> periods;
        
        // Procuder 
        private bool pause = false;
        private Dictionary<string, Dictionary<int, Zone>> symbolPeriods = new Dictionary<string,Dictionary<int,Zone>>();
        private Dictionary<string, List<KLine>> symbolKs = new Dictionary<string, List<KLine>>();
        private Dictionary<string, List<Bar>> symbolBars = new Dictionary<string, List<Bar>>();

        // get from parameters
        private bool useLocal;
        private DateTime startTime, endTime;
        private int interval = 0;
        private bool save;
        

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
            this.periods.Remove(1);

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

            // Create symbolPeriods
            foreach (Monitor m in strategy.GetMonitorEnumerator())
            {
                Dictionary<int, Zone> pZone = new Dictionary<int, Zone>();
                foreach (int p in periods)
                {
                    pZone[p] = new Zone();
                }

                symbolPeriods[m.Target.Symbol] = pZone;
                symbolKs[m.Target.Symbol] = new List<KLine>();
                symbolBars[m.Target.Symbol] = new List<Bar>();
            }

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
                ClearSymbolDay(true);
                List<KLine> ks = dictKs[dtString];

                foreach (KLine k in ks)
                {
                    Monitor m = strategy.GetMonitor(k.Symbol);
                    PushPeriodKLines(dtString, m, k);
                }

                List<KLine> dailyKs = dictDailyKs[dtString];
                foreach (KLine k in dailyKs)
                {
                    Monitor m = strategy.GetMonitor(k.Symbol);

                    // push last Period Klines of this symbol
                    Dictionary<int, Zone> pZone = symbolPeriods[k.Symbol];
                    foreach (int p in pZone.Keys)
                    {
                        if (symbolKs[k.Symbol].Count > pZone[p].x)
                            m.TA.PushKLine(p, SumKLines(symbolKs[k.Symbol], pZone[p].x));
                    }

                    // and daily K
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
                ClearSymbolDay(false);
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

                    PushPeriodBars(dtString, m, bar, dictFactor[symbol]);
                }

                // 推送一天的所有日线bar
                foreach (DailyBar daily in dailyBars)
                {
                    string symbol = string.Format("{0}.{1}", daily.exchange, daily.sec_id);
                    Monitor m = strategy.GetMonitor(symbol);

                    // push last Period Bars of this symbol
                    Dictionary<int, Zone> pZone = symbolPeriods[symbol];
                    foreach (int p in pZone.Keys)
                    {
                        if (symbolBars[symbol].Count > pZone[p].x)
                            m.PushBar(SumBars(p, symbolBars[symbol], pZone[p].x), dictFactor[symbol]);
                    }

                    // and daily bar
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

        private void ClearSymbolDay(bool isKLine)
        {
            foreach (string symbol in symbolPeriods.Keys)
            {
                foreach (int p in symbolPeriods[symbol].Keys)
                {
                    symbolPeriods[symbol][p].Clear();
                }
            }

            if (isKLine)
            {
                foreach (string symbol in symbolKs.Keys)
                    symbolKs[symbol].Clear();
            }
            else
            {
                foreach (string symbol in symbolBars.Keys)
                    symbolBars[symbol].Clear();
            }

        }

        private void PushPeriodKLines(string dtString, Monitor m, KLine k)
        {
            // add klines of this symbol
            List<KLine> klines = symbolKs[k.Symbol];

            // process periods
            Dictionary<int, Zone> pZone = symbolPeriods[k.Symbol];
            foreach (int p in pZone.Keys)
            {
                int minutes = Utils.GetLastingMinutes(Utils.UtcToDateTime(k.UTC));
                if (klines.Count > 0 && minutes - pZone[p].lastM >= p)
                {
                    m.TA.PushKLine(p, SumKLines(klines, pZone[p].x));

                    pZone[p].x = klines.Count;
                    pZone[p].lastM = (minutes / p) * p;
                }
            }

            m.TA.PushKLine(1, k);
            klines.Add(k);
        }

        private void PushPeriodBars(string dtString, Monitor m, Bar bar, float factor)
        {
            // add klines of this symbol
            string symbol = string.Format("{0}.{1}", bar.exchange, bar.sec_id);
            List<Bar> bars = symbolBars[symbol];

            // process periods
            Dictionary<int, Zone> pZone = symbolPeriods[symbol];
            foreach (int p in pZone.Keys)
            {
                int minutes = Utils.GetLastingMinutes(Utils.UtcToDateTime(bar.utc_time));
                if (bars.Count > 0 && minutes - pZone[p].lastM >= p)
                {
                    m.PushBar(SumBars(p, bars, pZone[p].x), factor);

                    pZone[p].x = bars.Count;
                    pZone[p].lastM = (minutes / p) * p;
                }
            }

            m.PushBar(bar, factor);
            bars.Add(bar);
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

        private KLine SumKLines(List<KLine> ks, int start)
        {
            KLine k = ks[start].Copy();
            k.CLOSE = ks[ks.Count - 1].CLOSE;

            for (int i = start + 1; i < ks.Count; i++)
            {
                // high
                if (k.HIGH < ks[i].HIGH) k.HIGH = ks[i].HIGH;
                // low
                if (k.LOW > ks[i].LOW) k.LOW = ks[i].LOW;
                // volumn
                k.VOLUME += ks[i].VOLUME;
            }
            return k;
        }

        private Bar SumBars(int p, List<Bar> bars, int start)
        {
            Bar bar = new Bar();
            Bar b0 = bars[start];
            bar.exchange = b0.exchange;
            bar.sec_id = b0.sec_id;
            bar.utc_time = b0.utc_time;
            bar.bar_type = p * 60;
            bar.high = b0.high;
            bar.low = b0.low;
            bar.open = b0.open;
            bar.close = bars[bars.Count - 1].close;
            bar.volume = b0.volume;

            for (int i = start + 1; i < bars.Count; i++)
            {
                // high
                if (bar.high < bars[i].high) bar.high = bars[i].high;
                // low
                if (bar.low > bars[i].low) bar.low = bars[i].low;
                // volumn
                bar.volume += bars[i].volume;
            }
            return bar;
        }
        #endregion
    }
}
