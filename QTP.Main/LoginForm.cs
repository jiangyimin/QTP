using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

using QTP.DBAccess;

namespace QTP.Main
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            // Read config file and Set ConnectionString
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            CRUD.ConnectionString =
                config.ConnectionStrings.ConnectionStrings["QTP_DB"].ConnectionString.ToString();

            // check password
            Global.Login = CRUD.GetTLogin();
            //if (Global.Login.Password != textBoxPassword.Text)
            //{
            //    MessageBox.Show("密码错误!");
            //    return;
            //}

            this.DialogResult = DialogResult.OK;
        }
    }
}
