using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
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

        Httpmsg http = new Httpmsg();
        MongoOperation mp = new MongoOperation();

        private void datapub_Click(object sender, RoutedEventArgs e)
        {
            Messageobj msg = new Messageobj();
            msg.taskname = taskname_txt.Text;
            msg.taskid = Guid.NewGuid().ToString();
            msg.usrname = usrname_txt.Text;
            msg.usrid = "20151007";
            msg.taskinfo = taskinfo_txt.Text;
            msg.creatername = creatname;
            msg.createid = creatid;
            msg.taskstat = "order";

            Taskfile tf = new Taskfile();

            List<string> filenames = new List<string>();
            List<string> fileroute = new List<string>();

            foreach(System.Windows.Controls.ListViewItem it in datalistview.Items)
            {
                filenames.Add(it.Content.ToString());
                string dataurl = "http://192.168.88.128/msg/" + it.Content.ToString();
                fileroute.Add(dataurl);
            }
            tf.filename = filenames;
            tf.fileurl = fileroute;

            msg.tf = tf;

            if(http.pubmsg(url,msg))
            {
                System.Windows.Forms.MessageBox.Show("任务已发布");
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("任务未发布");
            }
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ok_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void datapicker_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog filedialog = new OpenFileDialog();
            filedialog.Title = "选择文件";
            filedialog.FileName = string.Empty;            
            filedialog.RestoreDirectory = true;           
            DialogResult result = filedialog.ShowDialog();
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string fullname = filedialog.FileName;
                if(fullname!=string.Empty)
                {
                    string filename = System.IO.Path.GetFileName(fullname);
                    System.Windows.Controls.ListViewItem item = new System.Windows.Controls.ListViewItem();
                    item.Content = filename;
                    item.ToolTip = fullname;
                    datalistview.Items.Add(item);
                }
               
            }
            else
            {
                return;
            }
        }

        private void datapub_Click_1(object sender, RoutedEventArgs e)
        {
            if(datalistview.Items.Count!=0)
            {
                foreach(System.Windows.Controls.ListViewItem it in datalistview.Items)
                {
                    mp.fsupload(it.ToolTip.ToString(), it.Content.ToString());
                }
                System.Windows.MessageBox.Show("数据上传完毕");
                taskpub.IsEnabled = true;
            }
            else
            {
                System.Windows.MessageBox.Show("未选择数据，仍然继续？");
            }
        }
    }
}
