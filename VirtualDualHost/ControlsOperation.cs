using System.Collections.Generic;
using System.Windows.Forms;

namespace VirtualDualHost
{
    public static class ControlsOperation
    {
        public static void SetButtonClick(Button currentButton)
        {
            currentButton.Enabled = !currentButton.Enabled;
        }
        public static Dictionary<string, List<string>> ButtonTextAll = new Dictionary<string, List<string>>();
        public static string GetCurrentButtonText(Button currentButton)
        {
            string resultText = string.Empty;
            if (ButtonTextAll.Count <= 0)
            {
                ButtonTextAll.Add("btn_Start", new List<string> { "Start", "Stop" });
            }
            if (ButtonTextAll.ContainsKey(currentButton.Name))
            {
                List<string> buttonNameList = ButtonTextAll[currentButton.Name];
                resultText = currentButton.Text == buttonNameList[0] ? buttonNameList[1] : buttonNameList[0];
            }
            return resultText;
        }

        public static void SetTextBoxEnable(TextBox txt)
        {
            txt.Enabled = !txt.Enabled;
        }
    }
}
