using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MsgTransfer
{
    public class Httpmsg
    {
        //声明变量
        HttpModuel http = new HttpModuel();

        //操作方法

        public bool pubmsg(string url, Messageobj msg)
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


        //getmsg获取返回的消息体，url:为服务地址,msg:为检索消息体条件

        public Messageobj getmsg(string url, Messageobj msg)
        {
            Messageobj httpmsg = new Messageobj();
            string jsonstr = JsonConvert.SerializeObject(msg);
            string postresult = http.postjson(url, jsonstr);
            httpmsg = (Messageobj)JsonConvert.DeserializeObject(postresult, typeof(Messageobj));
            return httpmsg;
        }

        //taskstat获取当前用户的任务信息，url:服务地址，msg要获取的list名
        public List<Messageobj> taskstat(string url, ListInfo msg)
        {
            List<Messageobj> msglist = new List<Messageobj>();
            if (url != null && msg != null)
            {
                string jsonstr = JsonConvert.SerializeObject(msg);
                string postresult = http.postjson(url, jsonstr);
                if (postresult != "[emptylist")
                {
                    JArray taskinfos = (JArray)JsonConvert.DeserializeObject(postresult);
                    for (int i = 0; i < taskinfos.Count; i++)
                    {
                        string taskstr = taskinfos[i].ToString();
                        Messageobj obj = (Messageobj)JsonConvert.DeserializeObject(taskstr, typeof(Messageobj));
                        msglist.Add(obj);
                    }
                }
            }
            return msglist;
        }

        public bool chagestat(string url, MsgStat msg)
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

    }
}
