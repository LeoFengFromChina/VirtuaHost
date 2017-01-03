using StandardFeature;
using System.Collections.Generic;
using System.Text;

namespace ScreenPars_NDC
{
    public class ScreenOperator_NDC : IScreenOperator
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

                if (itemChar == NDCSrennCmdChars.FF
                    || itemChar == NDCSrennCmdChars.SI
                    || itemChar == NDCSrennCmdChars.SO
                    || itemChar == NDCSrennCmdChars.ESC)
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
                            char indentifierP = content[1];
                            if (indentifierP == '1'
                                || indentifierP == '2'
                                || indentifierP == 'E')
                            {
                                //1显示Logo,2.显示picture,3.先生image(后面是直接文件名如：abc.jpg)
                                if (content.Length > 2)
                                    escp.Content = content.Substring(2, content.Length - 2);
                                newObj = escp;

                            }
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
