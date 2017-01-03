using System;
using System.Text;
using System.Windows.Forms;

namespace TestingForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            byte[] calculatedData = Encoding.ASCII.GetBytes("506B083E;5048771047436372=250622000000503000?CAEA    0000000000009361<?0;:?<39497");

            string hex1 = getHexString(calculatedData);

            byte[] b2 = ConvertAsciiToEbcdic(calculatedData);

            string hex2 = getHexString(b2);



        }

        public byte[] ConvertASCIIStringToEBCDIC(string argASCIIString)
        {
            byte[] result = Encoding.ASCII.GetBytes(argASCIIString);
            try
            {
                byte[] byteASCII = Encoding.ASCII.GetBytes(argASCIIString);

                result = ConvertAsciiToEbcdic(byteASCII);


            }
            catch (System.Exception ex)
            {
            }
            return result;
        }
        public byte[] ConvertAsciiToEbcdic(byte[] asciiData)
        {
            // Create two different encodings.        
            Encoding ascii = Encoding.ASCII;
            Encoding ebcdic = Encoding.GetEncoding("IBM500");

            //return EBCDIC Data
            return Encoding.Convert(ascii, ebcdic, asciiData);
        }
        private string getHexString(byte[] array)
        {
            string ASCIIstr2 = null;
            for (int i = 0; i < array.Length; i++)
            {
                int asciicode = (int)(array[i]);
                ASCIIstr2 += Convert.ToString(asciicode, 16);//字符串ASCIIstr2 为对应的ASCII字符串
            }
            return ASCIIstr2;
        }
    }
}
