using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace StandardFeature
{

    #region XDC

    [Serializable]
    public class ProtocolTemplate
    {
        [XmlArrayAttribute("Templates")]
        public Template[] Template
        {
            get;
            set;
        }
        
    }
    [Serializable]
    public class Template
    {

        #region Fields

        [XmlAttribute("ProtocolType")]
        public string ProtocolType
        {
            get;
            set;
        }
        [XmlAttribute("DataType")]
        public string DataType
        {
            get;
            set;
        }

        [XmlAttribute("ID")]
        public string ID
        {
            get;
            set;
        }

        [XmlAttribute("Name")]
        public string Name
        {
            get;
            set;
        }

        #endregion

        [XmlArrayAttribute("Fields")]
        public Field[] Fields
        {
            get;
            set;
        }
    }

    [Serializable]
    public class Field
    {
        [XmlAttribute("Name")]
        public string Name
        {
            get;
            set;
        }
        [XmlAttribute("Size")]
        public string Size
        {
            get;
            set;
        }

        [XmlAttribute("Hex")]
        public string Hex
        {
            get;
            set;
        }

        [XmlArrayAttribute("Values")]
        public Value[] Values
        {
            get;
            set;
        }
    }

    [Serializable]
    public class Value
    {
        [XmlAttribute("Comment")]
        public string Comment
        {
            get;
            set;
        }


        [XmlText]
        public string value { get; set; }
    }
    #endregion
}
