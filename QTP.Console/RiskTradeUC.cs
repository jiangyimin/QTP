﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using QTP.Domain;

namespace QTP.Console
{
    public partial class RiskTradeUC : UserControl
    {
        private MyStrategy strategy;

        public RiskTradeUC()
        {
            InitializeComponent();
        }

        public MyStrategy Subject
        {
            set 
            {
                strategy = value;

                strategy.MessageHint += DispMessage;
            }
        }

        public void DispMessage(string msg)
        {
            if (this.label1.InvokeRequired == false)
            {
                label1.Text = msg;
            }
            else
            {
                MyStrategy.MessageHintCallback handle = new MyStrategy.MessageHintCallback(DispMessage);
                this.label1.BeginInvoke(handle, msg);

            }
        }
    }
}