using Simno.BaseData.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Run
{
    public partial class Login : DevExpress.XtraEditors.XtraForm
    {
        public Login()
        {
            InitializeComponent();
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            this.Visible = false;
            IUser user = new UserImp();
            string login = user.GetUserIdByLoginNameAndPwd(LoginUID.Text,LoginPW.Text);
            if (login != "")
            {
                IUserContext userContext = (new UserContextFactory()).Create(login);
                MainFrm main = new MainFrm(userContext);
                main.Show();
            }
            else
            {
                MessageBox.Show("用户名或密码错误！");
            }
           
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
