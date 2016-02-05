using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using QTP.Infra;
using QTP_LTSNet;
using GMSDK;

namespace QTP.MDWrapper
{
    public class MdAdapter : IMdAdapter
    {
        private List<string> cbMessages;
        public List<string> CBMessages
        {
            get { return cbMessages; }
        }

        private EventServer es;

        // for LTS
        private LTSMDAdapter mdLTS;
        private SecurityFtdcReqUserLoginField userLTS;
        private Dictionary<int, string> exchangeLTS;


        // for GM
        private GMSDK.MdApi mdGM;
        private Thread threadGM;
        private List<string> symbolsGM;

        private Dictionary<int, string> exchangeGM;


        public MdAdapter(EventServer es)
        {
            cbMessages = new List<string>();
            this.es = es;

            // Exchange Name
            exchangeGM = new Dictionary<int, string>();
            exchangeGM.Add((int)QExchange.SHA, "SHSE");
            exchangeGM.Add((int)QExchange.SZA, "SZSE");

            exchangeLTS = new Dictionary<int, string>();
            exchangeLTS.Add((int)QExchange.SHA, "SSE");
            exchangeLTS.Add((int)QExchange.SZA, "SZE");


        }

        public void InitLTSMDAdapter(string frontAddress, string brokerID, string userID, string password)
        {
            mdLTS = new LTSMDAdapter();
            mdLTS.OnFrontConnected += new FrontConnected(OnFrontConnectedMarket);
            mdLTS.OnRspSubMarketData += new RspSubMarketData(OnRspSubMarketData);
            mdLTS.OnRspUnSubMarketData += new RspUnSubMarketData(OnRspUnSubMarketData);
            mdLTS.OnHeartBeatWarning += new HeartBeatWarning(OnHeartBeatWarning);
            mdLTS.OnRspError += new RspError(OnRspError);
            mdLTS.OnRspUserLogin += new RspUserLogin(OnRspUserLogin);
            mdLTS.OnRtnDepthMarketData += new RtnDepthMarketData(OnRtnDepthMarketData);

            mdLTS.RegisterFront(frontAddress);
            mdLTS.Init();

            // Create User
            userLTS = new SecurityFtdcReqUserLoginField();
            userLTS.BrokerID = brokerID;
            userLTS.UserID = userID;
            userLTS.Password = password;

        }


        public void InitGMMDAdapter(string userID, string password)
        {
            mdGM = MdApi.Instance;
            mdGM.TickEvent += On_Tick;
            mdGM.BarEvent += On_Bar;

            // 实时行情
            int ret = mdGM.Init(userID, password, MDMode.MD_MODE_LIVE, "SHSE.000001.tick");
            symbolsGM = new List<string>();
        }

        #region Callbacks for LTSMDAdapter
        /// <summary>
        /// 连接回调函数
        /// </summary>
        public void OnFrontConnected()
        {
            cbMessages.Add(DateTime.Now + " 交易前置机连接成功！");

        }
        public void OnFrontConnectedMarket()
        {
            cbMessages.Add(DateTime.Now + " LTS行情前置机连接成功！");
        }


        /// <summary>
        /// 发生错误
        /// </summary>
        /// <param name="pRspInfo"></param>
        /// <param name="nRequestID"></param>
        /// <param name="bIsLast"></param>
        void OnRspError(SecurityFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            cbMessages.Add("--->>> ErrorID=" + pRspInfo.ErrorID + ", ErrorMsg=" + pRspInfo.ErrorMsg);
        }

        void OnHeartBeatWarning(int nTimeLapse)
        {
            cbMessages.Add("--->>> nTimerLapse = " + nTimeLapse);
        }

        void OnRspUserLogin(SecurityFtdcRspUserLoginField pRspUserLogin, SecurityFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            if (bIsLast && pRspInfo.ErrorID == 0)
            {
                ///获取当前交易日
                cbMessages.Add("--->>> 行情登录成功, 当前交易日 = " + mdLTS.GetTradingDay());
            }
        }

        void OnRspUnSubMarketData(SecurityFtdcSpecificInstrumentField pSpecificInstrument, SecurityFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
        }

        void OnRspSubMarketData(SecurityFtdcSpecificInstrumentField pSpecificInstrument, SecurityFtdcRspInfoField pRspInfo, int nRequestID, bool bIsLast)
        {
            cbMessages.Add("sub");
        }

        void OnRtnDepthMarketData(SecurityFtdcDepthMarketDataField pDepthMarketData)
        {
            QTick qtick = new QTick("LTS", pDepthMarketData);

            es.RaiseEvent(pDepthMarketData.InstrumentID, qtick);
        }

        #endregion


        #region Callbacks for GMMDAdapter
        /// <summary>
        /// 连接回调函数
        /// </summary>
        private void On_Tick(Tick tick)
        {
            foreach(string sym in symbolsGM)
            {
                MdApi.Instance.Subscribe(sym);
            }
            symbolsGM.Clear();
                
            QTick qtick = new QTick("GM", tick);
            es.RaiseEvent(tick.sec_id, qtick);
        }

        private void On_Bar(Bar bar)
        {
        }


        // DMAPI Run Thread
        private void GM_Run()
        {
            mdGM.Run();
        }

        #endregion

        #region IMdAdapter

        public void Run()
        {
            int ret = mdLTS.ReqUserLogin(userLTS, 0);

            threadGM = new Thread(GM_Run);
            threadGM.Start();
        }


        public int Subscribe(string symbol, int exchange)
        {
            string[] sym = new string[] { symbol };
            mdLTS.SubscribeMarketData(sym, 1, exchangeLTS[exchange]);

            symbolsGM.Add(exchangeGM[exchange]+"."+symbol+".tick");

            return 0; 
        }


        public void Close()
        {
            if (threadGM != null)
                threadGM.Abort();

            if (mdGM != null)
                mdGM.Close();
        }

        #endregion
    }
}
