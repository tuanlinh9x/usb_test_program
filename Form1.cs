using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;

namespace USB_TEST_ONR
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static string file_path;

        string data_file;

        private void Form1_Load(object sender, EventArgs e)
        {
            String[] ports = SerialPort.GetPortNames();
            comboBox1.Items.AddRange(ports);
            file_path = Directory.GetCurrentDirectory() + "/teraterm_nocount.log";
            // open file
            if (!File.Exists(file_path))
            {
                // exit the app 
                //MessageBox.Show("No configuration file found. Please add Configuration file");
                //System.Windows.Forms.Application.Exit();
            }
            else
            {

                data_file = File.ReadAllText(file_path);
                MessageBox.Show(data_file);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if((comboBox1.SelectedIndex == -1) || (comboBox2.SelectedIndex == -1))
            {
                MessageBox.Show("please choose the COM port and the speed");
            }
            else
            {
                if(button1.Text == "Open Port")
                {
                    // open the port

                    serialPort1.BaudRate = int.Parse(comboBox2.Text);
                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.ReadBufferSize = 8000;
                    if (!serialPort1.IsOpen)
                    {
                        serialPort1.Open();
                    }
                    button1.Text = "Close Port";
                }
                else if(button1.Text == "Close Port")
                {
                    // close the port                    
                    if (serialPort1.IsOpen)
                    {
                        serialPort1.Close();
                    }
                    button1.Text = "Open Port";
                }

            }                
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < 32769; i++)
            {
                serialPort1.Write(data_file);
                
            }
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            MessageBox.Show(elapsedMs.ToString());

                           
            

        }

        void text_show(object sender, EventArgs e)
        {
            while (serialPort1.BytesToRead != 0)
            {
                int temp;
                richTextBox1.AppendText(serialPort1.ReadExisting());
            }
           // richTextBox1.AppendText("received ");


        }
            private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //this.Invoke(new EventHandler(text_show));
            string test = serialPort1.ReadExisting();
            serialPort1.Write(test);
        }
    }
}
