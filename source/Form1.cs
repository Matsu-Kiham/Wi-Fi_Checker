using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Net;

namespace Wi_Fi_Checker
{
    public partial class Form1 : Form
    {
        bool form2_check;
        bool form3_check;
        int ip_num;
        string ip_fixed;
        int ip_first_num;


        public Form1()
        {
            InitializeComponent();

            form2_check = false;
            form3_check = false;
            string auto_filename = "./Auto.txt";
            StreamReader sr = new StreamReader(auto_filename, Encoding.UTF8);
            string line;
            line = sr.ReadLine();
            sr.Close();
            line = line.TrimEnd();
            int auto = int.Parse(line);
            if (auto == 0)
            {
                button5.Text = "自動スキャンOff";
                timer1.Enabled = false;
            }
            else
            {
                button5.Text = "自動スキャンOn";
                timer1.Enabled = true;
            }

            string list_filename2 = "./MAC_log.txt";
            string[] log_before = new string[1];
            string line2;
            int log_before_num = 0;
            StreamReader sr2 = new StreamReader(list_filename2, Encoding.UTF8);
            while ((line2 = sr2.ReadLine()) != null)
            {
                log_before[log_before_num] = line2.TrimEnd();
                Array.Resize(ref log_before, log_before.Length + 1);
                log_before_num++;
            }
            sr2.Close();

            string list_filename3 = "./MAC_log_backup.txt";
            string[] log_backup = new string[1];
            string line3;
            int log_backup_num = 0;
            StreamReader sr3 = new StreamReader(list_filename3, Encoding.UTF8);
            while ((line3 = sr3.ReadLine()) != null)
            {
                log_backup[log_backup_num] = line3.TrimEnd();
                Array.Resize(ref log_backup, log_backup.Length + 1);
                log_backup_num++;
            }
            sr3.Close();

            if (log_backup_num > log_before_num)
            {
                StreamWriter sw = new StreamWriter(new FileStream(list_filename2, FileMode.Truncate), Encoding.UTF8);
                int recovery_counter = 0;
                while (recovery_counter < log_backup_num)
                {
                    sw.WriteLine(log_backup[recovery_counter]);
                    recovery_counter++;
                }
                sw.Close();
            }

            if (log_backup_num < log_before_num)
            {
                StreamWriter sw = new StreamWriter(new FileStream(list_filename3, FileMode.Truncate), Encoding.UTF8);
                int recovery_counter = 0;
                while (recovery_counter < log_before_num)
                {
                    sw.WriteLine(log_before[recovery_counter]);
                    recovery_counter++;
                }
                sw.Close();
            }

            string ip_filename = "./ip.txt";
            StreamReader sr5 = new StreamReader(ip_filename, Encoding.UTF8);
            string ip_first;
            string ip_end;
            ip_first = sr5.ReadLine();
            ip_end = sr5.ReadLine();
            sr5.Close();
            ip_first = ip_first.TrimEnd();
            ip_end = ip_end.TrimEnd();
            int ip_fixed_extract_counter = 0;
            int ip_fixed_check = 0;
            int ip_first_number;
            int ip_end_number;
            string ip_first_number_string = "";
            string ip_end_number_string;
            ip_fixed = "";
            while (ip_fixed_extract_counter < ip_first.Length)
            {
                if (ip_fixed_check < 3)
                {
                    ip_fixed = ip_fixed + ip_first[ip_fixed_extract_counter];
                }
                else
                {
                    ip_first_number_string = ip_first_number_string + ip_first[ip_fixed_extract_counter]; ;
                }

                if (ip_first[ip_fixed_extract_counter] == '.')
                {
                    ip_fixed_check++;
                }
                ip_fixed_extract_counter++;
            }

            ip_end_number_string = ip_end.Replace(ip_fixed, "");
            ip_first_number = int.Parse(ip_first_number_string);
            ip_end_number = int.Parse(ip_end_number_string);

            ip_first_num = ip_first_number;
            ip_num = ip_end_number - ip_first_number + 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            form2_check = true;
            Form2 fo2 = new Form2();
            if (fo2.ShowDialog() == DialogResult.OK)
            {
                string list_filename = "./MAC_list.txt";
                string[] resistered = new string[100];
                string line;
                int list_num = 0;
                StreamReader sr = new StreamReader(list_filename, Encoding.UTF8);
                while ((line = sr.ReadLine()) != null)
                {
                    resistered[list_num] = line.TrimEnd();
                    list_num++;
                }
                sr.Close();
                StreamWriter sw = new StreamWriter(new FileStream(list_filename,FileMode.Truncate), Encoding.UTF8);
                if (list_num == 0)
                {
                    sw.WriteLine(fo2.dtex);
                }
                if (list_num > 0)
                {
                    bool duplication = false;
                    int duplication_counter = 0;
                    while (duplication_counter < list_num)
                    {
                        if (resistered[duplication_counter] == fo2.dtex)
                        {
                            duplication = true;
                        }
                        duplication_counter++;
                    }

                    int list_write_counter = 0;
                    while (list_write_counter < list_num)
                    {
                        sw.WriteLine(resistered[list_write_counter]);
                        list_write_counter++;
                    }

                    if (duplication == false)
                    {
                        sw.WriteLine(fo2.dtex);
                    }
                }
                sw.Close();
            }
            form2_check = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            form3_check = true;
            string list_filename = "./MAC_list.txt";
            string[] resistered = new string[100];
            string line;
            int list_num = 0;
            StreamReader sr = new StreamReader(list_filename, Encoding.UTF8);
            while ((line = sr.ReadLine()) != null)
            {
                resistered[list_num] = line.TrimEnd();
                list_num++;
            }
            sr.Close();


            StreamWriter sw = new StreamWriter(new FileStream(list_filename, FileMode.Truncate), Encoding.UTF8);

            if (list_num > 0)
            {
                Form3 fo3 = new Form3();
                int listbox_counter = 0;
                while (listbox_counter < list_num)
                {
                    fo3.listBox1.Items.Add(resistered[listbox_counter]);
                    listbox_counter++;
                }

                if (fo3.ShowDialog() == DialogResult.OK)
                {
                    string unresister = fo3.listBox1.Text;
                    string[] resistered2 = new string[99];
                    int resister_counter1 = 0;
                    int resister_counter2 = 0;
                    while (resister_counter1 < list_num)
                    {
                        if (resistered[resister_counter1] != unresister)
                        {
                            resistered2[resister_counter2] = resistered[resister_counter1];
                            sw.WriteLine(resistered2[resister_counter2]);
                            resister_counter2++;
                        }
                        resister_counter1++;
                    }
                } 
                else
                {
                    int resister_counter1 = 0;
                    while (resister_counter1 < list_num)
                    {
                        sw.WriteLine(resistered[resister_counter1]);                        
                        resister_counter1++;
                    }
                }
            }
            sw.Close();
            form3_check = false;
        }

        [DllImport("iphlpapi.dll", ExactSpelling = true)]
        private static extern int SendARP(int DestIP, int SrcIp, byte[] pMacAddr, ref int PhyAddrLen);



        private void timer1_Tick(object sender, EventArgs e)
        {
            
            if (form2_check == false)
            {
                if (form3_check == false)
                {
                    label2.Text = "接続スキャンを実行中です．このアプリを操作しないでください．";
                    label2.Refresh();
                    string list_filename = "./MAC_list.txt";
                    string[] resistered = new string[ip_num];
                    string line;
                    int list_num = 0;
                    StreamReader sr = new StreamReader(list_filename, Encoding.UTF8);
                    while ((line = sr.ReadLine()) != null)
                    {
                        resistered[list_num] = line.TrimEnd();
                        list_num++;
                    }
                    sr.Close();

                    string list_filename2 = "./MAC_log.txt";
                    string[] log_before = new string[1];
                    string line2;
                    int log_before_num = 0;
                    StreamReader sr2 = new StreamReader(list_filename2, Encoding.UTF8);
                    while ((line2 = sr2.ReadLine()) != null)
                    {
                        log_before[log_before_num] = line2.TrimEnd();
                        Array.Resize(ref log_before, log_before.Length + 1);
                        log_before_num++;
                    }
                    sr2.Close();

                    StreamWriter sw2 = new StreamWriter(new FileStream(list_filename2, FileMode.Truncate), Encoding.UTF8);

                    int log_before_w_num = 0;
                    while (log_before_w_num < log_before_num)
                    {
                        sw2.WriteLine(log_before[log_before_w_num]);
                        log_before_w_num++;
                    }

                    //refer to http://hensa40.cutegirl.jp/archives/6689
                    //from Line253 and Line254, Line 303 to Line 329
                    string[] dstIpAddr = new string[ip_num];
                    IPAddress[] dstIpAddress = new IPAddress[ip_num];
                    int[] destIpAddress = new int[ip_num];
                    string[] dstPhyAddr = new string[ip_num];
                    int ip_counter = 0;
                    while (ip_counter < ip_num)
                    {
                        
                        dstIpAddr[ip_counter] = ip_fixed + (ip_first_num + ip_counter).ToString();
                        dstIpAddress[ip_counter] = IPAddress.Parse(dstIpAddr[ip_counter]);
                        destIpAddress[ip_counter] = BitConverter.ToInt32(dstIpAddress[ip_counter].GetAddressBytes(), 0);
                        byte[] pMacAddr = new byte[6];
                        int PhyAddrLen = pMacAddr.Length;
                        int ret = SendARP(destIpAddress[ip_counter], 0, pMacAddr, ref PhyAddrLen);
                        if (ret == 0)
                        {
                            dstPhyAddr[ip_counter] = string.Format("{0:x2}-{1:x2}-{2:x2}-{3:x2}-{4:x2}-{5:x2}", pMacAddr[0],
                                                                                                           pMacAddr[1],
                                                                                                           pMacAddr[2],
                                                                                                           pMacAddr[3],
                                                                                                           pMacAddr[4],
                                                                                                           pMacAddr[5]);
                        }
                        else
                        {
                            dstPhyAddr[ip_counter] = "エラー";
                        }

                        int mac_check_counter = 0;
                        bool mac_check = false;
                        while (mac_check_counter < list_num)
                        {
                            if (dstPhyAddr[ip_counter] == resistered[mac_check_counter])
                            {
                                mac_check = true;
                            }
                            mac_check_counter++;
                        }

                        if (dstPhyAddr[ip_counter] == "エラー")
                        {
                            mac_check = true;
                        }

                        if (mac_check == false)
                        {
                            DateTime dt = DateTime.Now;
                            string result = dt.ToString();
                            string log = result + "  MACアドレス：" + dstPhyAddr[ip_counter];
                            sw2.WriteLine(log);
                            this.label1.Text = "未登録のMACアドレスの接続がありました";
                            label1.Refresh();
                            System.Media.SystemSounds.Exclamation.Play();
                        }

                        ip_counter++;
                    }

                    sw2.Close();
                    label2.Text = "";

                    
                    string[] log_after = new string[1];
                    string line3;
                    int log_after_num = 0;
                    StreamReader sr4 = new StreamReader(list_filename2, Encoding.UTF8);
                    while ((line3 = sr4.ReadLine()) != null)
                    {
                        log_after[log_after_num] = line3.TrimEnd();
                        Array.Resize(ref log_after, log_after.Length + 1);
                        log_after_num++;
                    }
                    sr4.Close();

                    string log_backup_filename = "./MAC_log_backup.txt";
                    StreamWriter sw4 = new StreamWriter(new FileStream(log_backup_filename, FileMode.Truncate), Encoding.UTF8);
                    int log_after_w_num = 0;
                    while (log_after_w_num < log_after_num)
                    {
                        sw4.WriteLine(log_after[log_after_w_num]);
                        log_after_w_num++;
                    }
                    sw4.Close();
                }
            }
                
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string list_filename2 = "./MAC_log.txt";
            //refer to https://www.kisoplus.com/file/file.html
            //from Line396 to Line406
            string line, str = "";
            StreamReader sr = new StreamReader(list_filename2, Encoding.UTF8);
            if (File.Exists(list_filename2))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    str = str + line + "\r\n";
                }
                textBox1.Text = str;
            }
            sr.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            label2.Text = "接続スキャンを実行中です．フォームを操作しないでください．";
            label2.Refresh();
            string list_filename = "./MAC_list.txt";
            string[] resistered = new string[ip_num];
            string line;
            int list_num = 0;
            StreamReader sr = new StreamReader(list_filename, Encoding.UTF8);
            while ((line = sr.ReadLine()) != null)
            {
                resistered[list_num] = line.TrimEnd();
                list_num++;
            }
            sr.Close();

            string list_filename2 = "./MAC_log.txt";
            string[] log_before = new string[1];
            string line2;
            int log_before_num = 0;
            StreamReader sr2 = new StreamReader(list_filename2, Encoding.UTF8);
            while ((line2 = sr2.ReadLine()) != null)
            {
                log_before[log_before_num] = line2.TrimEnd();
                Array.Resize(ref log_before, log_before.Length + 1);
                log_before_num++;
            }
            sr2.Close();

            StreamWriter sw2 = new StreamWriter(new FileStream(list_filename2, FileMode.Truncate), Encoding.UTF8);

            int log_before_w_num = 0;
            while (log_before_w_num < log_before_num)
            {
                sw2.WriteLine(log_before[log_before_w_num]);
                log_before_w_num++;
            }

            //refer to http://hensa40.cutegirl.jp/archives/6689
            //from Line 449 to Line 475
            string[] dstIpAddr = new string[ip_num];
            IPAddress[] dstIpAddress = new IPAddress[ip_num];
            int[] destIpAddress = new int[ip_num];
            string[] dstPhyAddr = new string[ip_num];
            int ip_counter = 0;
            while (ip_counter < ip_num)
            {
                
                dstIpAddr[ip_counter] = ip_fixed + (ip_first_num + ip_counter).ToString();
                dstIpAddress[ip_counter] = IPAddress.Parse(dstIpAddr[ip_counter]);
                destIpAddress[ip_counter] = BitConverter.ToInt32(dstIpAddress[ip_counter].GetAddressBytes(), 0);
                byte[] pMacAddr = new byte[6];
                int PhyAddrLen = pMacAddr.Length;
                int ret = SendARP(destIpAddress[ip_counter], 0, pMacAddr, ref PhyAddrLen);
                if (ret == 0)
                {
                    dstPhyAddr[ip_counter] = string.Format("{0:x2}-{1:x2}-{2:x2}-{3:x2}-{4:x2}-{5:x2}", pMacAddr[0],
                                                                                                   pMacAddr[1],
                                                                                                   pMacAddr[2],
                                                                                                   pMacAddr[3],
                                                                                                   pMacAddr[4],
                                                                                                   pMacAddr[5]);
                }
                else
                {
                    dstPhyAddr[ip_counter] = "エラー";
                }

                int mac_check_counter = 0;
                bool mac_check = false;
                while (mac_check_counter < list_num)
                {
                    if (dstPhyAddr[ip_counter] == resistered[mac_check_counter])
                    {
                        mac_check = true;
                    }
                    mac_check_counter++;
                }

                if (dstPhyAddr[ip_counter] == "エラー")
                {
                    mac_check = true;
                }

                if (mac_check == false)
                {
                    DateTime dt = DateTime.Now;
                    string result = dt.ToString();
                    string log = result + "  MACアドレス：" + dstPhyAddr[ip_counter];
                    sw2.WriteLine(log);
                    this.label1.Text = "未登録のMACアドレスの接続がありました";
                    label1.Refresh();
                    System.Media.SystemSounds.Exclamation.Play();
                }

                ip_counter++;
            }

            sw2.Close();
            label2.Text = "";

            string[] log_after = new string[1];
            string line3;
            int log_after_num = 0;
            StreamReader sr4 = new StreamReader(list_filename2, Encoding.UTF8);
            while ((line3 = sr4.ReadLine()) != null)
            {
                log_after[log_after_num] = line3.TrimEnd();
                Array.Resize(ref log_after, log_after.Length + 1);
                log_after_num++;
            }
            sr4.Close();

            string log_backup_filename = "./MAC_log_backup.txt";
            StreamWriter sw4 = new StreamWriter(new FileStream(log_backup_filename, FileMode.Truncate), Encoding.UTF8);
            int log_after_w_num = 0;
            while (log_after_w_num < log_after_num)
            {
                sw4.WriteLine(log_after[log_after_w_num]);
                log_after_w_num++;
            }
            sw4.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string auto_filename = "./Auto.txt";
            StreamReader sr = new StreamReader(auto_filename, Encoding.UTF8);
            string line;
            line = sr.ReadLine();
            sr.Close();
            line = line.TrimEnd();
            int auto = int.Parse(line);
            StreamWriter sw3 = new StreamWriter(new FileStream(auto_filename, FileMode.Truncate), Encoding.UTF8);


            if (auto == 0)
            {
                sw3.WriteLine("1");
                button5.Text = "自動スキャンOn";
                timer1.Enabled = true;
            }
            else
            {
                sw3.WriteLine("0");
                button5.Text = "自動スキャンOff";
                timer1.Enabled = false;
            }
            sw3.Close();
        }

        


        private void button6_Click(object sender, EventArgs e)
        {
            Form4 fo4 = new Form4();
            if (fo4.ShowDialog() == DialogResult.OK)
            {
                string ip_first = fo4.textBox1.Text;
                string ip_end = fo4.textBox2.Text;
                int ip_fixed_extract_counter = 0;
                int ip_fixed_check = 0;
                int ip_first_number;
                int ip_end_number;
                string ip_first_number_string = "";
                string ip_end_number_string;
                ip_fixed = "";
                while (ip_fixed_extract_counter < ip_first.Length)
                {
                    if (ip_fixed_check < 3)
                    {
                        ip_fixed = ip_fixed + ip_first[ip_fixed_extract_counter];
                    }
                    else
                    {
                        ip_first_number_string = ip_first_number_string + ip_first[ip_fixed_extract_counter]; ;
                    }

                    if (ip_first[ip_fixed_extract_counter] == '.')
                    {
                        ip_fixed_check++;
                    }
                    ip_fixed_extract_counter++;
                }

                ip_end_number_string = ip_end.Replace(ip_fixed, "");
                ip_first_number = int.Parse(ip_first_number_string);
                ip_end_number = int.Parse(ip_end_number_string);

                ip_first_num = ip_first_number;
                ip_num = ip_end_number - ip_first_number + 1;

                string ip_filename = "./ip.txt";
                StreamWriter sw5 = new StreamWriter(new FileStream(ip_filename, FileMode.Truncate), Encoding.UTF8);
                sw5.WriteLine(ip_first);
                sw5.WriteLine(ip_end);
                sw5.Close();
            }
        }
    }


}
