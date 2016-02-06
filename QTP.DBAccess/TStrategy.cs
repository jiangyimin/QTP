﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTP.DBAccess
{
    public class TStrategy
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Status { get; set; }
        public int PoolId { get; set; }

        public string MonitorClass { get; set; }
        public string RiskMClass { get; set; }
        public string TradeChannel { get; set; }
        // GM UserName and Password
        public string UserName { get; set; }
        public string Password { get; set; }

        public List<TInstrument> Instruments { get; set; }
        public List<TPosition> Positions { get; set; }

    }
}
