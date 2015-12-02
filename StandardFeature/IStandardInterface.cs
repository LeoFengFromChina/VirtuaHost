using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StandardFeature
{
    /// <summary>
    /// 格式化消息接口
    /// </summary>
    public interface IMessageFormat
    {
        Queue<string> NeedSendToBothHost
        {
            get;
            set;
        }
        XDCMessage Format(byte[] msgByte, int msgLength);
    }

    public interface IMessageOperator
    {
        List<ParsRowView> GetView(XDCMessage XDCmsg);

    }
    public interface IStateOperator
    {
        List<ParsRowView> GetView(string parsText);
    }
    public interface IFitOperator
    {
        List<ParsRowView> GetView(string parsText);
    }
    public interface IScreenOperator
    {
        List<ParsRowView> GetView(string parsText);
    }
}
