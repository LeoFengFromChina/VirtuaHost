using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestingForm
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            BindingList<CassettleView> viewList = new BindingList<CassettleView>();
            viewList.Add(new CassettleView("1111", "500", "100", "OK", "1000"));
            viewList.Add(new CassettleView("2222", "100", "100", "OK", "1000"));
            viewList.Add(new CassettleView("3333", "10", "100", "OK", "1000"));
            viewList.Add(new CassettleView("4444", "5", "100", "OK", "1000"));
            dataGridView1.DataSource = viewList;

        }
    }

    public class CassettleView
    {
        private string _cassettleID = string.Empty;
        private string _status = string.Empty;
        private string _deno = string.Empty;
        private string _count = string.Empty;
        private string _limited = string.Empty;
        public string CassettleID
        {
            get
            {
                return _cassettleID;
            }
            set
            {
                _cassettleID = value;
            }
        }
        public string Deno
        {
            get
            {
                return _deno;
            }
            set
            {
                _deno = value;
            }
        }
        public string Count
        {
            get
            {
                return _count;
            }
            set
            {
                _count = value;
            }
        }
        public string Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }
        public string Limited
        {
            get
            {
                return _limited;
            }
            set
            {
                _limited = value;
            }
        }
        public CassettleView()
        { }
        public CassettleView(string cassettleID, string deno, string count, string status, string limited)
        {
            _cassettleID = cassettleID;
            _deno = deno;
            _count = count;
            _status = status;
            _limited = limited;
        }
    }
}
