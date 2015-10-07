using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsgTransfer
{
    //MsgTransfer作为统一的任务消息体进行消息的发送和接收，通过Newtonsoft.dll进行Messageobj到json格式的相互转化
    public class Messageobj
    {
        public string usrname;
        public string usrid;
        public string creatername;
        public string createid;
        public string taskname;
        public string taskid;
        public string taskinfo;
        public string taskstat;
        public Taskfile tf;
    }

    public class Taskfile
    {
        public List<string> filename = new List<string>();
        public List<string> fileurl = new List<string>();
    }

    public class ListInfo
    {
        public string list;
    }

    public class MsgStat
    {
        public string usrid;
        public Messageobj msg;
        public string changeto;
    }
}
