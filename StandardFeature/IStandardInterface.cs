using System.Collections.Generic;

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
        XDCMessage Format(byte[] msgByte, int msgLength, TcpHead Head = TcpHead.NoHead, bool isDebug = false);
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
        List<object> GetView(string parsText);
    }
}
