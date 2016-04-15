using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QTP.DBAccess;
using QTP.TAlib;
using GMSDK;

namespace QTP.Domain
{
    public class Bridge : PoolManager
    {
        
        public Bridge(MyStrategy strategy, TPool pool)
            : base (strategy, pool)
        {
        }
    }
}
