using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace CsharpSocket
{
    public partial class Form1 : Form
    {
        static string output = "";
        public Form1()
        {
            InitializeComponent();
           
        }
        public void createListener()
        {

            TcpListener tcpListener = null;

            IPAddress ipAddress = IPAddress.Parse("10.1.2.66");
            try
            {

                tcpListener = new TcpListener(ipAddress, 1120);
                tcpListener.Start();
                output = "Waiting for a connection...";
            }
            catch (Exception e)
            {
                output = "Error: " + e.ToString();
                MessageBox.Show(output);
            }
            while (true)
            {
                Thread.Sleep(10);
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                byte[] bytes = new byte[1024*5];
                NetworkStream stream = tcpClient.GetStream();
                stream.Read(bytes, 0, bytes.Length);
                string output = "";
            }
        }
        static string Connect(string serverIP)
        {
            string output = "";

            try
            {
                StringBuilder message = new StringBuilder();
                XmlDocument xdoc = new XmlDocument();
                //string xml1 = "<?xml version=\"1.0\" encoding=\"UTF - 8\" standalone=\"no\" ?>< cnpsXML CarbonAPIVer = \"1.2\" TaskType = \" JobList \" />";
                //byte[] encodedString = Encoding.UTF8.GetBytes(xml1);
               // MemoryStream ms = new MemoryStream(encodedString);
                //ms.Flush();
                //ms.Position = 0;
                xdoc.Load("JobList.xml");
                message.Append("CarbonAPIXML1");
                message.Append(" ");
                message.Append(xdoc.DocumentElement.OuterXml.Length);
                message.Append(" ");
                message.Append(xdoc.DocumentElement.OuterXml.ToString());

                Int32 port = 1120;
                TcpClient client = new TcpClient(serverIP, port);

                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message.ToString());
               

                NetworkStream stream = client.GetStream();


                stream.Write(data, 0, data.Length);

                output = "Sent: " + message;
                MessageBox.Show(output);

                data = new Byte[1024*25];

                String responseData = String.Empty;

               // Int32 bytes = stream.Read(data, 0, data.Length);
                //responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                Int32 bytes = 0;
                while ((bytes = stream.Read(data, 0, data.Length)) > 0)
                {
                    responseData += System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                }

                output = "Received: " + responseData;
                MessageBox.Show(output);

                stream.Close();
                client.Close();
                return responseData;
            }
            catch (ArgumentNullException e)
            {
                output = "ArgumentNullException: " + e;
                MessageBox.Show(output);
                return e.Message;
            }
            catch (SocketException e)
            {
                output = "SocketException: " + e.ToString();
                MessageBox.Show(output);
                return e.Message;
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string serverIP = textBox1.Text;
            
            textBox2.Text = Connect(serverIP);
        }
    }
}
