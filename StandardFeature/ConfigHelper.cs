using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace StandardFeature
{
    public class ConfigHelper<T> where T : class
    {
        private XmlSerializerHelper<T> xmlHelp;
        public ConfigHelper()
        {
            xmlHelp = new XmlSerializerHelper<T>();
        }
        /// <summary>
        /// 保存CONFIG
        /// </summary>
        /// <param name="xmlPath">xml地址</param>
        /// <param name="t">xml对象</param>
        /// <returns>是否成功</returns>
        public bool SaveConfig(string xmlPath, T t)
        {
            try
            {
                return xmlHelp.Serialize(xmlPath, t);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 获得CONFIG
        /// </summary>
        /// <param name="xmlPath">xml地址</param>
        /// <returns>config对象</returns>
        public T GetConfig(string xmlPath)
        {

            try
            {
                return xmlHelp.Deserialize(xmlPath);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 对象序列化为xml格式的字符串
        /// </summary>
        /// <param name="entity">对象</param>
        /// <returns>xml格式的字符串</returns>
        public string XMLSerialize(T entity)
        {
            try
            {
                return xmlHelp.XMLSerialize(entity);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// xml格式的字符串序列化为对象
        /// </summary>
        /// <param name="xmlString">xml格式的字符串</param>
        /// <returns>对象</returns>
        public T DeXMLSerialize(string xmlString)
        {
            try
            {
                return xmlHelp.DeXMLSerialize(xmlString);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }

    public class XmlSerializerHelper<T> where T : class
    {
        private XmlSerializer xmlSer = null;
        private FileStream fs = null;
        private StringBuilder buffer = null;
        private TextWriter writer = null;
        private TextReader reader = null;
        public XmlSerializerHelper()
        {
            xmlSer = new XmlSerializer(typeof(T));
            buffer = new StringBuilder();
        }

        public bool Serialize(string xmlPath, T t)
        {
            try
            {
                fs = new FileStream(xmlPath, FileMode.Create);
                xmlSer.Serialize(fs, t);
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

                if (null != fs)
                {
                    fs.Close();
                    fs.Dispose();
                    fs = null;
                }
            }
        }
        public T Deserialize(string xmlPath)
        {
            try
            {
                fs = new FileStream(xmlPath, FileMode.Open);
                T t = (T)xmlSer.Deserialize(fs);
                return t;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (null != fs)
                {
                    fs.Close();
                    fs.Dispose();
                    fs = null;
                }
            }
        }
        public string XMLSerialize(T entity)
        {
            try
            {
                writer = new StringWriter(buffer);
                xmlSer.Serialize(writer, entity);
                return buffer.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (null != writer)
                {
                    writer.Close();
                    writer.Dispose();
                    writer = null;
                }
                if (null != buffer)
                {
                    buffer.Clear();
                }
            }
        }
        public T DeXMLSerialize(string xmlString)
        {
            try
            {
                buffer.Append(xmlString);
                reader = new StringReader(buffer.ToString());
                return (T)xmlSer.Deserialize(reader);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (null != reader)
                {
                    reader.Close();
                    reader.Dispose();
                    reader = null;
                }
                if (null != buffer)
                {
                    buffer.Clear();
                }
            }
        }

    }
}
