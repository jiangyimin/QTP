using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GMSDK;
using QTP_LTSNet;

namespace QTP.MDWrapper
{
    enum QExchange
    {
        SHA = 0,
        SHB,
        SZA,
        SZB
    }
    public class QTick
    {
        public QTick(string sender, Tick tick)
        {
            this.sender = sender;
            time = DateTime.Now;
            exchange = tick.exchange;
            sec_id = tick.sec_id;
        }

        public QTick(string sender, SecurityFtdcDepthMarketDataField tick)
        {
            this.sender = sender;
            time = DateTime.Now;
            exchange = tick.ExchangeID;
            sec_id = tick.InstrumentID;
        }

        public string sender;
        public DateTime time;
        public string exchange;
        public string sec_id;

        
    }
}
