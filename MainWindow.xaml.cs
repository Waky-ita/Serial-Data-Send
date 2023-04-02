using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.IO.Ports;
using Serial_Data_Send.Properties;
using System.Runtime.InteropServices.ComTypes;

namespace Serial_Data_Send
{
    
    public partial class MainWindow : Window
    {
        //Create serial port
        SerialPort serial = new SerialPort();
        string recieved_data;

        public MainWindow()
        {
            InitializeComponent();
            //Button lock 
            ClosePortButton.IsEnabled = false;
            //Name of file with settings
            string fileSettings = ("Settings.ini");

            // Create file if not exist
            if (!File.Exists(fileSettings))
            {                
                using (StreamWriter sw = File.CreateText(fileSettings));
            }
            
            // Read strings from file Settings.ini
            string fileContent = File.ReadAllText("Settings.ini");


            //Remove spaces and divine in lines then add to list on Combo Box
            string[] lines = fileContent.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in lines)
            {
                StringToSendComboBox.Items.Add(line);
            }

            //Check aviable ports and add to ComboBox
            string[] aviablePorts = SerialPort.GetPortNames();

            foreach (string ports in aviablePorts)
            {
                AviablePortsComboBox.Items.Add(ports);
            }
            

            //Add Baud Rate to Combo Box
            string[] LoadComboBoxDataBaudRate()
            {
                string[] strArray = {
                                    "1200",
                                    "2400",
                                    "4800",
                                    "9600",
                                    "14400",
                                    "19200",
                                    "28800",
                                    "38400",
                                    "57600",
                                    "115200",
                                    "230400",
                                    };
                return strArray;
            }
            AviableBaudRateComboBox.ItemsSource = LoadComboBoxDataBaudRate();

            //Add  data bits to Combo Box
            string[] LoadComboBoxDataDataBits()
            {
                string[] strArray = {
                                    "5",
                                    "6",
                                    "7",
                                    "8",
                                    };
                return strArray;
            }
            AviableDataBitsComboBox.ItemsSource = LoadComboBoxDataDataBits();

            //Add  stop bits to Combo Box
            string[] LoadComboBoxDataStopBits()
            {
                string[] strArray = {
                                    "1",
                                    "2",                                   
                                    };
                return strArray;
            }
            AviableStopBitsComboBox.ItemsSource = LoadComboBoxDataStopBits();

            //Add  parity bits to Combo Box
            string[] LoadComboBoxDataParityBits()
            {
                string[] strArray = {
                                    "None",
                                    "Odd",
                                    "Even",
                                    "Mark",
                                    "Space",
                                    };
                return strArray;
            }
            AviableParityBitsComboBox.ItemsSource = LoadComboBoxDataParityBits();


        }

        //Event on button click Open
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                serial.PortName = AviablePortsComboBox.Text;
                serial.BaudRate = Convert.ToInt32(AviableBaudRateComboBox.Text);
                serial.DataBits = Convert.ToInt32(AviableDataBitsComboBox.Text);
                serial.StopBits = (StopBits)Enum.Parse(typeof(StopBits), AviableStopBitsComboBox.Text);
                serial.Parity = (Parity)Enum.Parse(typeof(Parity), AviableParityBitsComboBox.Text);
                serial.Open();
                recieved_data = serial.ReadExisting();
                serial.WriteTimeout = 500;
                serial.ReadTimeout = 500;

                //Buttons lock 
                OpenPortButton.IsEnabled = false;
                ClosePortButton.IsEnabled = true;



            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error");
            }
        }
        
        //Event on button click Close
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (serial.IsOpen)
            {
                serial.Close();
                //Buttons lock 
                OpenPortButton.IsEnabled = true;                            
                ClosePortButton.IsEnabled = false;
                
            }
        }
        
        //Event on button click Send
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                if (serial.IsOpen)
                {
                    if (AddTimeCheckBox.IsChecked ?? false)
                    {
                        serial.WriteLine(StringToSendComboBox.Text + DateTime.Now.ToString("yyyy/dd/MM HH.mm.ss"));
                        MessageBoxField.Text += StringToSendComboBox.Text + DateTime.Now.ToString("yyyy/dd/MM HH.mm.ss") + Environment.NewLine;
                        MessageBoxField.ScrollToEnd();
                    }
                    else
                    {
                        serial.WriteLine(StringToSendComboBox.Text);
                        MessageBoxField.Text += StringToSendComboBox.Text + Environment.NewLine;
                        MessageBoxField.ScrollToEnd();
                    }
                    

                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error");
            }
        }

      
    }
}
