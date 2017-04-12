using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace TestingForm
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        public static string StringToUnicode(char singleChar)
        {
            byte[] buffer;
            string result = string.Empty;
            buffer = System.Text.Encoding.Unicode.GetBytes(singleChar.ToString());
            result = (String.Format("{0:X2}", buffer[0]));
            return result;
        }
        private void button1_Click(object sender, EventArgs e)
        {

            Process.Start("notepad.exe", @"C:\Users\lenovo\Desktop\CIMB\Logs_FromATM\20161013\log\COM20161013.txt");


        }

        private void Form2_Load(object sender, EventArgs e)
        {

            int a = 100;
            double b = 99;

            if(a> b)
            {

            }
            else
            {

            }

        }
    }

    public interface IMyInterface
    {
        void Write();
    }
    public class MyClass1 : IMyInterface
    {
        public void Write()
        {
            MessageBox.Show("Hello world 1");
        }
    }
    public class MyClass2 : MyClass1
    {
        public new void Write()
        {
            MessageBox.Show("Hello world 2");
        }
    }

    public class MyClass3 : MyClass2, IMyInterface
    {
        public new void Write()
        {
            MessageBox.Show("Hello world 3");
        }
    }
    public class MyClass4 : MyClass3, IMyInterface
    {
        public new void Write()
        {
            MessageBox.Show("Hello world 5");
        }
        void IMyInterface.Write() {
            MessageBox.Show("Hello world 4");
        }

    }
}
