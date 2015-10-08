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
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.Grid;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MsgTransfer
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //变量声明
        //HttpModuel http = new HttpModuel();
        Httpmsg http = new Httpmsg();
        MongoOperation mp = new MongoOperation();
        string msggeturl = "http://192.168.88.128:2000";
        string msgstaturl = "http://192.168.88.128:2001";
        string msgstatchangeurl = "http://192.168.88.128:2002";

        List<Messageobj> orderstat = new List<Messageobj>();
        List<Messageobj> workingstat = new List<Messageobj>();
        List<Messageobj> donestat = new List<Messageobj>();
        List<Messageobj> mystat = new List<Messageobj>();


        ListInfo orderlinfo = new ListInfo();
        ListInfo workinginfo = new ListInfo();
        ListInfo doneinfo = new ListInfo();
        ListInfo myinfo = new ListInfo();
        //USERLIST用户的任务信息枚举
        enum USERLIST
        {
            ODERLIST=0,         //尚未进行的任务列表
            WORKINGLIST=1,      //正在进行的任务列表
            DONELIST=2,         //已完成的任务列表
            MYTASKLIST=3        //用户自己发布的任务列表
        }

        #region demo
    //    string msggeturl = "http://192.168.88.128:2000";
    //    string msgstaturl = "http://192.168.88.128:2001";
    //    string msgstatchangeurl = "http://192.168.88.128:2002";
    //    Messageobj msg = new Messageobj();
    //    Taskfile tf = new Taskfile();
    //    msg.usrid = "20151005";
    //        msg.usrname = "wm";
    //        msg.createid = "20151005";
    //        msg.creatername = "wm";
    //        msg.taskid = Guid.NewGuid().ToString();
    //    msg.taskname = "消息中间件";
    //        msg.taskinfo = "消息中间件测试";
    //        msg.taskstat = "order";

    //        tf.filename.Add("ld.tif");
    //        tf.fileurl.Add("http://eclipsesv.com");

    //        tf.filename.Add("ld.tif");
    //        tf.fileurl.Add("http://eclipsesv.com");

    //        msg.tf = tf;

    //        pubmsg(msggeturl, msg);


    //    ListInfo linfo = new ListInfo();
    //    linfo.list = "20151005orderlist";

    //        List<Messageobj> orderlist = taskstat(msgstaturl, linfo);
    //        if(orderlist.Count!=0)
    //        {
    //            MsgStat stat = new MsgStat();
    //    stat.usrid = "20151005";
    //            stat.msg = orderlist[0];
    //            stat.changeto = "0";
    //            chagestat(msgstatchangeurl, stat);

    //}
    #endregion


        //窗体响应事件         
        private void platform_Loaded(object sender, RoutedEventArgs e)
        {
            
            orderlinfo.list = "20151007orderlist";

            
            workinginfo.list = "20151007workinglist";

            
            doneinfo.list = "20151007donelist";

            
            myinfo.list = "20151007mytasklist";


            orderstat = http.taskstat(msgstaturl, orderlinfo);
            
            workingstat = http.taskstat(msgstaturl, workinginfo);

            donestat = http.taskstat(msgstaturl, doneinfo);

            mystat = http.taskstat(msgstaturl, myinfo);

            initwindow(order, "order", orderstat);
            initwindow(workinglist, "working", workingstat);
            initwindow(donelist, "done", donestat);
            initwindow(mylist, "mytask", mystat);
        }

        private void workinglist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListViewItem it = workinglist.SelectedItem as ListViewItem;
            if(it!=null)
            {
                int index = int.Parse(it.ToolTip.ToString());
                Messageobj msg = workingstat[index];
                infodisplay("working", msg);
            }
            
        }

        private void order_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListViewItem it = order.SelectedItem as ListViewItem;
            if(it!=null)
            {
                int index = int.Parse(it.ToolTip.ToString());
                Messageobj msg = orderstat[index];
                infodisplay("order", msg);
            }
            
        }

        private void donelist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListViewItem it = donelist.SelectedItem as ListViewItem;
            if(it!=null)
            {
                int index = int.Parse(it.ToolTip.ToString());
                Messageobj msg = donestat[index];
                infodisplay("done", msg);
            }
            
        }

        private void mylist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListViewItem it = mylist.SelectedItem as ListViewItem;
            if(it!=null)
            {
                int index = int.Parse(it.ToolTip.ToString());
                Messageobj msg = mystat[index];
                infodisplay("mytask", msg);
            }
            
        }

        //开始任务
        private void starttask_Click(object sender, RoutedEventArgs e)
        {
            ListViewItem it = order.SelectedItem as ListViewItem;
            if (it != null)
            {
                int index = int.Parse(it.ToolTip.ToString());
                Messageobj msg = orderstat[index];
                MsgStat mstat = new MsgStat();
                mstat.usrid = "20151007";
                mstat.changeto = "0";
                mstat.msg = msg;
                http.chagestat(msgstatchangeurl, mstat);
                MessageBox.Show("任务状态已改变");
                reloadform();
            }
        }

        //任务完成
        private void finishtask_Click(object sender, RoutedEventArgs e)
        {
            ListViewItem it = workinglist.SelectedItem as ListViewItem;
            if (it != null)
            {
                int index = int.Parse(it.ToolTip.ToString());
                Messageobj msg = workingstat[index];
                MsgStat mstat = new MsgStat();
                mstat.usrid = "20151007";
                mstat.changeto = "1";
                mstat.msg = msg;
                http.chagestat(msgstatchangeurl, mstat);
                MessageBox.Show("任务状态已改变");
                reloadform();
            }
        }

        //获取任务数据
        private void getdata_Click(object sender, RoutedEventArgs e)
        {
            ListViewItem it = workinglist.SelectedItem as ListViewItem;
            if (it != null)
            {
                int index = int.Parse(it.ToolTip.ToString());
                Messageobj msg = workingstat[index];
                MessageBox.Show("选择数据存放文件夹");
                System.Windows.Forms.FolderBrowserDialog floderdialog = new System.Windows.Forms.FolderBrowserDialog();
                floderdialog.Description = "选择文件夹";
                if(floderdialog.ShowDialog()== System.Windows.Forms.DialogResult.OK)
                {
                    string selectedfloder = floderdialog.SelectedPath;
                    foreach(string filename in msg.tf.filename)
                    {
                        string localfile = selectedfloder + @"\" + filename;
                        mp.fsdownload(localfile, filename);
                    }
                    MessageBox.Show("任务数据下载完毕");
                }
            }
        }

        //点击读取配置文件,执行任务
        private void luanchtask_Click(object sender, RoutedEventArgs e)
        {

        }


        //私有函数
        private void reloadform()
        {
            orderstat = http.taskstat(msgstaturl, orderlinfo);

            workingstat = http.taskstat(msgstaturl, workinginfo);

            donestat = http.taskstat(msgstaturl, doneinfo);

            mystat = http.taskstat(msgstaturl, myinfo);

            initwindow(order, "order", orderstat);
            initwindow(workinglist, "working", workingstat);
            initwindow(donelist, "done", donestat);
            initwindow(mylist, "mytask", mystat);
        }

        private void initwindow(ListView lw, string listtype,List<Messageobj> msgs)
        {
            int counter = 0;
            lw.Items.Clear();
           
            foreach (Messageobj msg in msgs)
            {
                
                ListViewItem lwitem = new ListViewItem();
                lwitem.Content = msg.taskname;
                lwitem.ToolTip = counter.ToString();
                switch(msg.taskstat)
                {
                    case "order":
                        lwitem.Background = Brushes.AliceBlue;
                        break;
                    case "working":
                        lwitem.Background = Brushes.ForestGreen;
                        break;
                    case "done":
                        lwitem.Background = Brushes.Green;
                        break;
                }
                
               // lwitem.MouseDown += selectitemchange;

                lw.Items.Add(lwitem);
                
                if(counter==0)
                {
                    infodisplay(listtype, msg);
                }
                counter++;

            }
        }

        

        private void infodisplay(string type,Messageobj msg)
        {
            switch (type)
            {
                case "order":
                    order1.Text = msg.taskname;
                    //order2.Text = msg.taskid;
                    order3.Text = msg.usrname;
                    order4.Text = msg.creatername;
                    order5.Text = msg.taskstat;
                    order6.Items.Clear();
                    foreach (string filename in msg.tf.filename)
                    {
                        order6.Items.Add(filename);
                    }
                    order7.Content = msg.taskinfo;
                    break;
                case "working":
                    working1.Text = msg.taskname;
                    //working2.Text = msg.taskid;
                    working3.Text = msg.usrname;
                    working4.Text = msg.creatername;
                    working5.Text = msg.taskstat;
                    working6.Items.Clear();
                    foreach (string filename in msg.tf.filename)
                    {
                        working6.Items.Add(filename);
                    }
                    working7.Content = msg.taskinfo;
                    break;
                case "done":
                    done1.Text = msg.taskname;
                    //done2.Text = msg.taskid;
                    done3.Text = msg.usrname;
                    done4.Text = msg.creatername;
                    done5.Text = msg.taskstat;
                    done6.Items.Clear();
                    foreach (string filename in msg.tf.filename)
                    {
                        done6.Items.Add(filename);
                    }
                    done7.Content = msg.taskinfo;
                    break;
                case "mytask":
                    my1.Text = msg.taskname;
                    //my2.Text = msg.taskid;
                    my3.Text = msg.usrname;
                    my4.Text = msg.creatername;
                    my5.Text = msg.taskstat;
                    my6.Items.Clear();
                    foreach (string filename in msg.tf.filename)
                    {
                        my6.Items.Add(filename);
                    }
                    my7.Content = msg.taskinfo;
                    break;
            }
        }

       

        private void pubtask_Click(object sender, RoutedEventArgs e)
        {
            NewTask nt = new NewTask("wm", "20151007", msggeturl);
            nt.ShowDialog();
            reloadform();
        }

        
    }
}
