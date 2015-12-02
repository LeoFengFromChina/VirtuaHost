using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StandardFeature;
using MessagePars_NDC;
using MessagePars_DDC;
using StatePars_NDC;
using StatePars_DDC;
using ScreenPars_DDC;
using ScreenPars_NDC;
using FitPars_DDC;
using FitPars_NDC;
using XmlHelper;

namespace VirtualDualHost
{
    public static class BaseFunction
    {
        public static void Intial(XDCProtocolType pType = XDCProtocolType.NDC, DataType dType = DataType.Message)
        {
            XDCUnity.CurrentDataType = dType;
            XDCUnity.CurrentProtocolType = pType;
            switch (pType)
            {
                case XDCProtocolType.DDCorNDC:
                    break;
                case XDCProtocolType.NDC:
                    {
                        XDCUnity.MessageFormat = new MessageFormat_NDC();
                        XDCUnity.MessageOperator = new MessageOperator_NDC();
                        XDCUnity.StateOperator = new StateOperator_NDC();
                        XDCUnity.ScreenOperator = new ScreenOperator_NDC();
                        XDCUnity.FitOperator = new FitOperator_NDC();

                    }
                    break;
                case XDCProtocolType.DDC:
                    {
                        XDCUnity.MessageFormat = new MessageFormat_DDC();
                        XDCUnity.MessageOperator = new MessageOperator_DDC2();
                        XDCUnity.StateOperator = new StateOperator_DDC();
                        XDCUnity.ScreenOperator = new ScreenOperator_DDC();
                        XDCUnity.FitOperator = new FitOperator_DDC();
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
