using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QTP.Domain;

namespace QTP.Console
{
    interface IStrategyUC
    {
        MyStrategy Subject { set; }
        void ShowData();
        void TimerRefresh();
    }
}
