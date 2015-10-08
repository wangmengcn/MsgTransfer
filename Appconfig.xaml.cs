using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Xml;

namespace MsgTransfer
{
    /// <summary>
    /// Appconfig.xaml 的交互逻辑
    /// </summary>
    public partial class Appconfig : Window
    {
        //变量声明
        List<string> apps = new List<string>();
        List<string> apppaths = new List<string>();
        List<string> appinfos = new List<string>();

        public Appconfig()
        {
            InitializeComponent();
        }

        public Appconfig(string taskname,string taskinfo)
        {
            InitializeComponent();
            string info = "当前任务：" + taskname;
            if(taskinfo!=string.Empty)
            {
                info += "   任务描述：" + taskinfo;
            }
            currenttask.Content = info;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            XMLConfig loadxml = new XMLConfig();
            loadxml.loaddoc("./appconfig/appconfig.xml");
            XmlNodeList nodelist = loadxml.getnodes("/Appconfig/App");
            int count = 0;
            foreach(XmlNode node in nodelist)
            {
                ListViewItem it = new ListViewItem();
                string name= node.Attributes["Name"].Value;
                string apppath = node.Attributes["Apppath"].Value;
                string appinfo = node.Attributes["Appinfo"].Value;
                it.Content = name;
                it.ToolTip = count.ToString();
                it.MouseDoubleClick += It_MouseDoubleClick;
                toolist.Items.Add(it);
                apps.Add(name);
                apppaths.Add(apppath);
                appinfos.Add(appinfo);
                count++;
            }
        }

        private void It_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListViewItem it = sender as ListViewItem;
            if(it!=null)
            {
                int index = int.Parse(it.ToolTip.ToString());
                Process newprocess = new Process();
                newprocess.StartInfo.FileName = apppaths[index];
                newprocess.StartInfo.WorkingDirectory=@"D:\GF\二维程序备份\Collaborative\Collaborative\bin\Debug";
                newprocess.Start();
            }
        }
    }
}
