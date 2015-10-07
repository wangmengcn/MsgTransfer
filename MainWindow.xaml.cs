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
        HttpModuel http = new HttpModuel();
        string msggeturl = "http://192.168.88.128:2000";
        string msgstaturl = "http://192.168.88.128:2001";
        string msgstatchangeurl = "http://192.168.88.128:2002";

        List<Messageobj> orderstat = new List<Messageobj>();
        List<Messageobj> workingstat = new List<Messageobj>();
        List<Messageobj> donestat = new List<Messageobj>();
        List<Messageobj> mystat = new List<Messageobj>();

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







        //pubmsg发送一条任务消息到服务器,url:为目标服务地址,msg:为任务消息体
        private bool pubmsg(string url,Messageobj msg)
        {
            bool httpres = false;
            if(url!=null&&msg!=null)
            {
                string jsonstr = JsonConvert.SerializeObject(msg);
                string postresult = http.postjson(url, jsonstr);
                if (postresult == "数据提交完毕")
                {
                    httpres = true;
                }
            }
            return httpres;
        }


        //getmsg获取返回的消息体，url:为服务地址,msg:为检索消息体条件

        private Messageobj getmsg(string url,Messageobj msg)
        {
            Messageobj httpmsg = new Messageobj();
            string jsonstr = JsonConvert.SerializeObject(msg);
            string postresult = http.postjson(url, jsonstr);
            httpmsg = (Messageobj)JsonConvert.DeserializeObject(postresult, typeof(Messageobj));
            return httpmsg;
        }

        //taskstat获取当前用户的任务信息，url:服务地址，msg要获取的list名
        private List<Messageobj> taskstat(string url, ListInfo msg)
        {
            List<Messageobj> msglist = new List<Messageobj>();
            if (url != null && msg != null)
            {
                string jsonstr = JsonConvert.SerializeObject(msg);
                string postresult = http.postjson(url, jsonstr);
                if (postresult != "[emptylist")
                {
                    JArray taskinfos = (JArray)JsonConvert.DeserializeObject(postresult);
                    for(int i=0;i<taskinfos.Count;i++)
                    {
                        string taskstr = taskinfos[i].ToString();
                        Messageobj obj= (Messageobj)JsonConvert.DeserializeObject(taskstr, typeof(Messageobj));
                        msglist.Add(obj);
                    }
                }
            }
            return msglist;
        }

        private bool chagestat(string url, MsgStat msg)
        {
            bool httpres = false;
            if (url != null && msg != null)
            {
                string jsonstr = JsonConvert.SerializeObject(msg);
                string postresult = http.postjson(url, jsonstr);
                if (postresult == "数据提交完毕")
                {
                    httpres = true;
                }
            }
            return httpres;
        }

        private void platform_LayoutUpdated(object sender, EventArgs e)
        {

        }

        private void platform_Loaded(object sender, RoutedEventArgs e)
        {
            ListInfo orderlinfo = new ListInfo();
            orderlinfo.list = "20151005orderlist";

            ListInfo workinginfo = new ListInfo();
            workinginfo.list = "20151005workinglist";

            ListInfo doneinfo = new ListInfo();
            doneinfo.list = "20151005donelist";

            ListInfo myinfo = new ListInfo();
            myinfo.list = "20151005mytasklist";


            orderstat = taskstat(msgstaturl, orderlinfo);
            
            workingstat = taskstat(msgstaturl, workinginfo);

            donestat = taskstat(msgstaturl, doneinfo);

            mystat = taskstat(msgstaturl, myinfo);

            initwindow(order, "order", orderstat);
            initwindow(workinglist, "working", workingstat);
            initwindow(donelist, "order", donestat);
            initwindow(mylist, "order", mystat);
        }

        private void initwindow(ListView lw, string listtype,List<Messageobj> msgs)
        {
            int counter = 0;
            foreach(Messageobj msg in msgs)
            {
                
                ListViewItem lwitem = new ListViewItem();
                lwitem.Content = msg.taskname;
                lwitem.ToolTip = counter.ToString();
                lw.Items.Add(lwitem);
                
                if(counter==0)
                {
                    switch (listtype)
                    {
                        case "order":
                            order1.Text = msg.taskname;
                            //order2.Text = msg.taskid;
                            order3.Text = msg.usrname;
                            order4.Text = msg.creatername;
                            order5.Text = msg.taskstat;
                            foreach(string filename in msg.tf.filename)
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
                            foreach (string filename in msg.tf.filename)
                            {
                                my6.Items.Add(filename);
                            }
                            my7.Content = msg.taskinfo;
                            break;
                    }
                }
                counter++;

            }
        }

        private void pubtask_Click(object sender, RoutedEventArgs e)
        {
            NewTask nt = new NewTask("wm", "20151007", msggeturl);
            nt.Show();
        }
    }
}
