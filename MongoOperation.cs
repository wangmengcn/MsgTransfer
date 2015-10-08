using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB;
using MongoDB.GridFS;
using System.Windows.Forms;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using System.IO;

namespace MsgTransfer
{
    class MongoOperation
    {
        private Mongo g_mongo;
        private MongoDB.IMongoDatabase g_test;
        IMongoCollection col ;
        string g_Serverip;
        string g_ServerPort;
        string g_database;
        

        public void  Connect2Mongo(string ip,string port,string database)
        {
            try
            {
                var connstr = @"Server=" + ip + ":" + port;      //外网地址
                //var connstr = @"Server=localhost:27017";
                g_mongo = new Mongo(connstr);
                g_mongo.Connect();

                g_test = g_mongo[database];

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "异常信息");
            }
            
        }

        public void UseCollection(string tablename)
        {
            if(tablename!=null &&tablename!="")
            {
                switch(tablename)
                {
                    case"角色表":
                        col = g_test.GetCollection("RoleTable");
                        break;
                    case"用户表":
                        col = g_test.GetCollection("UserTable");
                        break;
                    case"单位部门表":
                        col = g_test.GetCollection("DepTable");
                        break;
                    case"研判过程":
                        col = g_test.GetCollection("SJTable");
                        break;
                    case"研判任务":
                        col = g_test.GetCollection("SJTaskTable");
                        break;
                    case"洪涝案例":
                        col = g_test.GetCollection("FloodRecTable");
                        break;
                    case"地震案例":
                        col = g_test.GetCollection("EQRecTable");
                        break;
                    case"旱灾案例":
                        col = g_test.GetCollection("DroutRecTable");
                        break;
                    case"雪灾案例":
                        col = g_test.GetCollection("SnowTable");
                        break;
                    case"标准表":
                        col = g_test.GetCollection("StandarsTable");
                        break;
                    case"背景知识":
                        col = g_test.GetCollection("BackgroundTable");
                        break;
                    case"损毁等级":
                        col = g_test.GetCollection("DamageGradeTable");
                        break;
                    case"解译标志":
                        col = g_test.GetCollection("TargetsTable");
                        break;
                    case"模型库":
                        col = g_test.GetCollection("ModelTable");
                        break;
                    case "灾害元数据":
                        col = g_test.GetCollection("DisasterMetadata");
                        break;
                }
            }
        }

        public void InsertMsg(Document doc)
        {
            try
            {
                col.Save(doc);
            }
            catch
            {
                MessageBox.Show("未能上传数据");
            }
            
        }

        public void DelMsg(Document doc)
        {
            col.Delete(doc);
        }

        public void Modify(Document doc)
        {
            col.Update(doc);
        }

        public ICursor Qury(Document doc)
        {
            ICursor result = null;
            result=col.Find(doc);
            return result;
        }

        public ICursor FindAll()
        {
            ICursor result = null;
            result = col.FindAll();
            return result;
        }


        public void fsupload(string localfilename,string filename)
        {
            string strconn = "mongodb://192.168.88.128:27017";
            try
            {
                MongoServer server = MongoServer.Create(strconn);

                MongoDB.Driver.MongoDatabase test = server["msgtask"];
                MongoGridFS fs = test.GridFS;
                System.IO.Stream fs_stream = new System.IO.FileStream(localfilename, System.IO.FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite);

                fs.Upload(fs_stream, filename);
            }
            catch(MongoDB.Driver.MongoException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
            
        }

        public void fsdownload(string filename)
        {
            string strconn = "mongodb://192.168.88.128:27017";
            MongoServer server = MongoServer.Create(strconn);

            MongoDB.Driver.MongoDatabase test = server["msgtask"];
            MongoGridFS fs = test.GridFS;
            string apppath = Application.StartupPath;
            string localpath = apppath + "\\word\\Background\\" + filename;
            fs.Download(localpath,filename);
        }


        public void fsdownload(string localpath,string remotefile)
        {
            string strconn = "mongodb://192.168.88.128:27017";
            MongoServer server = MongoServer.Create(strconn);

            MongoDB.Driver.MongoDatabase test = server["msgtask"];
            MongoGridFS fs = test.GridFS;

            fs.Download(localpath, remotefile);
        }

        
    }
}
