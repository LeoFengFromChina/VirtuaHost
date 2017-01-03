using System.Windows.Forms;

namespace VirtualDualHost
{
    public partial class uc_Opc : UserControl
    {
        public uc_Opc()
        {
            InitializeComponent();
        }
        public string OperationCode
        {
            get
            {
                return txt_OPC.Text;
            }
            set
            {
                txt_OPC.Text = value;
            }
        }
        public string InteractiveMsg
        {
            get
            {
                return rtb_InteractiveMsg.Text;
            }
            set
            {
                rtb_InteractiveMsg.Text = value;
            }
        }
        public string ReplyMsg
        {
            get
            {
                return rtb_ReplyMsg.Text;
            }
            set
            {
                rtb_ReplyMsg.Text = value;
            }
        }
    }
}
