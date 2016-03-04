using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QTP.Domain
{
    public class ConnectStatusEventArgs : EventArgs
    {
        private int num;
        private bool connectSucceed;

        public ConnectStatusEventArgs(bool connectSucceed, int num)
        {
            this.connectSucceed = connectSucceed;
            this.num = num;
        }

        public bool ConnectSucceed
        {
            get { return connectSucceed; }
        }
        public int Num
        {
            get { return num; }
        }

        
    }
}
