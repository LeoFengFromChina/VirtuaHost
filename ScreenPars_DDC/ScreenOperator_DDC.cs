using StandardFeature;
using System.Collections.Generic;
using System.Text;

namespace ScreenPars_DDC
{
    public class ScreenOperator_DDC : IScreenOperator
    {
        List<object> toReturn;
        public List<object> GetView(string parsText)
        {
            toReturn = new List<object>();
            bool isMix = false;
            StringBuilder tempStr = new StringBuilder();
            char tempCmd = ' ';
            foreach (char itemChar in parsText)
            {

                if (itemChar == DDCSrennCmdChars.FF
                    || itemChar == DDCSrennCmdChars.SI
                    || itemChar == DDCSrennCmdChars.SO
                    || itemChar == DDCSrennCmdChars.ESC)
                {
                    if (tempStr.Length > 0)
                    {
                        EnqueueObject(tempCmd, tempStr.ToString());
                        tempStr.Clear();
                    }
                    tempCmd = itemChar;
                }
                else
                {
                    tempStr.Append(itemChar);
                }
            }
            if (tempStr.Length > 0)
            {
                EnqueueObject(tempCmd, tempStr.ToString());
                tempStr.Clear();
            }
            return toReturn;
        }

        public void EnqueueObject(char Head, string content)
        {
            object newObj = null;
            if (Head == DDCSrennCmdChars.SI)
            {
                DDC_SI_Command dsi = new DDC_SI_Command();
                if (content.Length > 0)
                    dsi.StartRow = content.Substring(0, 1);
                if (content.Length > 1)
                    dsi.StartColumn = content.Substring(1, 1);
                if (content.Length > 2)
                    dsi.Content = content.Substring(2, content.Length - 2);

                newObj = dsi;
            }
            else if (Head == DDCSrennCmdChars.ESC)
            {
                DDC_ESCP_Command escp = new DDC_ESCP_Command();
                char indentifier = content[0];
                switch (indentifier)
                {
                    case 'P':
                        {
                            if (content.Length > 3)
                                escp.Content = content.Substring(1, 3);
                            newObj = escp;
                        }
                        break;
                    default:
                        break;
                }
            }
            else if (Head == DDCSrennCmdChars.SO)
            {
                DDC_SO_Command dso = new DDC_SO_Command();
                dso.Content = content;
                newObj = dso;
            }
            if (newObj != null)
                toReturn.Add(newObj);
        }

    }
}
