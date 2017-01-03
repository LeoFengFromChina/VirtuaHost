using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WCFtest.eCATWCF;
using eCATInspectorSerivceProtocol;
using System.Threading;
namespace WCFtest
{
    public partial class Form_Main : Form
    {
        public Form_Main()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        Dictionary<string, CacheViewItem> cacheDic = new Dictionary<string, CacheViewItem>();
        public void OnDataCacheClear(eCATWCF.DataCacheType argType, string argDateTimestamp)
        {
            dgv_BufferListView.DataSource = GetViewList(argType.ToString());
            //dgv_BufferListView.Refresh();
        }

        public void OnDataChanged(string argkey, string argValue, eCATWCF.DataCacheType argType, string argDatetimeStamp)
        {
            if (cacheDic.ContainsKey(argkey))
            {
                cacheDic[argkey].BufferValue = argValue;
                cacheDic[argkey].Timespan = argDatetimeStamp;
                cacheDic[argkey].DataCacheType = argType.ToString();
                dgv_BufferListView.DataSource = GetViewList();
                //dgv_BufferListView.Refresh();
            }
        }

        public void OnDataDeleted(string argKey, eCATWCF.DataCacheType argType, string argDateTimeStamp)
        {
            if (cacheDic.ContainsKey(argKey))
            {
                cacheDic.Remove(argKey);
            }
        }
        eCATWCF.IeCATInspectorService client = null;
        int token = -1;

        delegate void Binding();
        event Binding BindDataGridView;

        delegate void SetNoticeText(string lableName, string showText);
        event SetNoticeText SetNotice;

        private void Form1_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width, Screen.PrimaryScreen.WorkingArea.Height - this.Height);

            btn_Search.Click += textBox1_TextChanged;
            BindDataGridView += new Binding(Form_Main_BindDataGridView);
            SetNotice += new SetNoticeText(Form_Main_SetNotice);
            ConnectToWCF();
        }

        void Form_Main_SetNotice(string lableName, string showText)
        {
            switch (lableName)
            {
                case "Count":
                    {
                        lbl_BufferCount.Text = showText;
                    }
                    break;
                default:
                    break;
            }
        }

        void Form_Main_BindDataGridView()
        {
            dgv_BufferListView.DataSource = GetViewList();
        }

        private void ConnectToWCF()
        {
            int reTryTime = 1;
            //while (true)
            //{
            try
            {
                #region doing

                InspectorCallback callback = new InspectorCallback(this);
                System.ServiceModel.InstanceContext icontext = new System.ServiceModel.InstanceContext(callback);
                client = new IeCATInspectorServiceClient(icontext);

                if (client.LoginIn(out token, "LeoFeng", "GRGBanking.com"))
                {
                    WCFtest.eCATWCF.DataSnapShot dss = client.QuerySnapshotOfData(token);
                    foreach (WCFtest.eCATWCF.DataSnapShotItem item in dss.Items)
                    {
                        CacheViewItem cvi = new CacheViewItem(item.Key, item.Value, "", item.Type.ToString());
                        cacheDic.Add(cvi.Key, cvi);
                    }
                    client.ListenDataChanged(token);
                    client.Hearbeat(token);
                }

                BindDataGridView();

                SetNotice("Status", "OK");
                //break;

                #endregion
            }
            catch (Exception ex)
            {
                //notice something
                SetNotice("Status", "Connect to eCAT Failed.ReTryTime===> " + reTryTime);
                reTryTime++;
            }
            //}
        }
        private List<CacheViewItem> GetViewList(string ClearType = null)
        {
            List<CacheViewItem> resultList = new List<CacheViewItem>();
            foreach (KeyValuePair<string, CacheViewItem> item in cacheDic)
            {
                if (!string.IsNullOrEmpty(ClearType)
                    && item.Value.DataCacheType.Equals(ClearType))
                {
                    continue;
                    //to do.
                }
                else if (!string.IsNullOrEmpty(textBox1.Text.Trim()))
                {
                    //查询
                    if (item.Key.ToUpper().StartsWith(textBox1.Text.Trim().ToUpper()))
                        resultList.Add(item.Value);
                }
                else
                    resultList.Add(item.Value);
            }
            SetNotice("Count", resultList.Count.ToString());
            return resultList.OrderByDescending(s => s.Timespan).ToList();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            List<CacheViewItem> list = GetViewList();
            if (string.IsNullOrEmpty(textBox1.Text.Trim()))
                dgv_BufferListView.DataSource = list;
            else
                dgv_BufferListView.DataSource = list.FindAll(s => s.Key.ToUpper().StartsWith(textBox1.Text.Trim().ToUpper()));
            dgv_BufferListView.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConnectToWCF();
        }
    }

    public class InspectorCallback : IeCATInspectorServiceCallback
    {
        private Form_Main m_main;

        public InspectorCallback(Form_Main argMain)
        {
            this.m_main = argMain;
        }


        public void OnDataChanged(string argKey, string argValue, eCATWCF.DataCacheType argType, string argDateTime)
        {
            this.m_main.OnDataChanged(argKey, argValue, argType, argDateTime);
        }

        public void OnDataDeleted(string argKey, eCATWCF.DataCacheType argType, string argDateTime)
        {
            this.m_main.OnDataDeleted(argKey, argType, argDateTime);
        }

        public void OnDataCacheClear(eCATWCF.DataCacheType argType, string argDateTime)
        {
            this.m_main.OnDataCacheClear(argType, argDateTime);
        }
    }

    public class CacheViewItem
    {
        public CacheViewItem(string key, string bufferValue, string timespan, string cacheType)
        {
            Key = key;
            BufferValue = bufferValue;
            Timespan = timespan;
            DataCacheType = cacheType;
        }
        public string Key { get; set; }
        public string BufferValue { get; set; }
        public string Timespan { get; set; }
        public string DataCacheType { get; set; }
    }
}
