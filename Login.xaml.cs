using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MongoDB;

namespace MsgTransfer
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            MongoOperation mongoop = new MongoOperation();
            mongoop.Connect2Mongo("127.0.0.1", "27017", "GF");
            mongoop.UseCollection("用户表");

            Document login = new Document();
            Document privateinfo = new Document();

            Document ddd = new Document();

            login["Name"] = username.Text;

            ICursor logs = mongoop.Qury(login);


            int flag = 0;
            foreach (Document doc in logs.Documents)
            {
                Document priv = (Document)doc["PrivateInfo"];
                if (priv["Pwd"].ToString() == userpass.Text)
                {
                    MessageBox.Show("成功登陆！");
                    flag = 1;
                    MainWindow mwindow = new MainWindow(doc["Name"].ToString(),doc["UserID"].ToString());
                    mwindow.Show();
                    this.Close();             
                }
            }
            if (flag == 0)
            {
                MessageBox.Show("用户名或密码不正确！");
            }
        }
    }
}
