using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace VirtualDualHost
{
    public partial class Form_StateCheck : Form
    {
        public Form_StateCheck()
        {
            InitializeComponent();
        }
        Dictionary<string, List<string>> _stateDic_NDC = null;
        Dictionary<string, List<string>> _stateDic_DDC = null;
        public Form_StateCheck(Dictionary<string, List<string>> stateDic_NDC, Dictionary<string, List<string>> stateDic_DDC)
        {

            InitializeComponent();
            _stateDic_NDC = stateDic_NDC;
            _stateDic_DDC = stateDic_DDC;
            if (_stateDic_NDC != null && _stateDic_NDC.Count > 0)
            {
                GetLis("NDC");
            }
            if (_stateDic_DDC != null && _stateDic_DDC.Count > 0)
            {
                GetLis("DDC");
            }
        }

        private void GetLis(string protocolType)
        {
            Dictionary<string, List<string>> _stateDic = null;
            if (protocolType.Equals("ndc", StringComparison.OrdinalIgnoreCase))
            {
                _stateDic = _stateDic_NDC;
            }
            else
                _stateDic = _stateDic_DDC;


            string richTextBoxContent = string.Empty;
            string tempStateNum = string.Empty;
            List<StateView> stateViewList = new List<StateView>();
            foreach (KeyValuePair<string, List<string>> item in _stateDic)
            {
                tempStateNum = string.Empty;
                StateView sv = new StateView();
                sv.StateType = item.Key;
                richTextBoxContent += item.Key + "|";
                foreach (string itemList in item.Value)
                {
                    tempStateNum += itemList + ",";
                }
                sv.StateNums = tempStateNum.Substring(0, tempStateNum.Length - 1);
                stateViewList.Add(sv);
            }
            if (protocolType.Equals("ndc", StringComparison.OrdinalIgnoreCase))
            {
                rtb_NDC.Text = richTextBoxContent;
                dgv_StateCanView_NDC.DataSource = stateViewList;
            }
            else
            {
                rtb_DDC.Text = richTextBoxContent;
                dgv_StateCanView_DDC.DataSource = stateViewList;
            }
        }

        private void Form_StateCheck_Load(object sender, EventArgs e)
        {

        }
    }

    public class StateView
    {
        private string _stateType = string.Empty;
        private string _stateNums = string.Empty;
        public string StateType
        {
            get
            {
                return _stateType;
            }
            set
            {
                _stateType = value;
            }
        }
        public string StateNums
        {
            get
            {
                return _stateNums;
            }
            set
            {
                _stateNums = value;
            }
        }
        public StateView()
        { }
        public StateView(string stateType, string stateNums)
        {
            _stateType = stateType;
            _stateNums = stateNums;
        }
    }
}
