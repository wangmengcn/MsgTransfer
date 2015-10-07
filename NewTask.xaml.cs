using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MsgTransfer
{
    /// <summary>
    /// NewTask.xaml 的交互逻辑
    /// </summary>
    public partial class NewTask : Window
    {
        public NewTask()
        {
            InitializeComponent();
        }

        public NewTask(string creator,string creatorid,string puburl)
        {
            creatname = creator;
            creatid = creatorid;
            url = puburl;
            InitializeComponent();
        }

        //变量声明
        private string creatname;
        private string creatid;
        private string url;
    }
}
