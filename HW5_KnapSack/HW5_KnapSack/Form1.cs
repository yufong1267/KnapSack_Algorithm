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

namespace HW5_KnapSack
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        //存放讀取的檔案內容 & 檔案位址
        private String HW_Message = "" , file_path = "";

        //預設背包內容 和 數量
        private BackPackThing[] PackThing = new BackPackThing[100];
        private int packthing_number = 0;

        //建立選擇的判斷
        private string choose = "";
        private int best_weight = 0;
        private int best_value = 0;

        //背包最大載重限制
        private int Pack_LimitWeight = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            //資料存取 把讀取進來的資料 處存
            data_preset();

            //演算法 窮舉
            Knapsack();

            //寫入原本檔案
            String write_file = "取"+ choose + "，重量：" + best_weight + "，價值：" + best_value;
          //  MessageBox.Show("" + write_file);
            StreamWriter sw = new StreamWriter(file_path);
            sw.WriteLine(HW_Message + "\r\n" + write_file);            // 寫入文字
            sw.Close();                     // 關閉串流
        }

        private void data_preset() {

            //雖然存在Heap裡面 但是不知道會不會啟動GC去把位址內容清空 所以先做初始化陣列
            for (int i = 0; i < 100 ; i++)
            {
                PackThing[i] = null;
            }

            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "Text Documents|*.txt", Multiselect = false, ValidateNames = true })
                {
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        using (StreamReader sr = new StreamReader(ofd.FileName, Encoding.GetEncoding(950), true))
                        {
                            //存取檔案路徑
                            file_path = ofd.FileName;

                            HW_Message = sr.ReadToEnd();

                            //確認有沒有讀取到
                            //MessageBox.Show("" + HW_Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("讀檔失敗!!!!");
            }


            //依序每一行 極限重量 >> 內容編號 >> 內容重量 >> 內容價值
            string[] single_line = HW_Message.Split('\n');
            int mode_count = 0;
            foreach(String line in single_line)
            {
                if (mode_count == 0)
                {
                    //存取極限重量
                    Pack_LimitWeight = Int32.Parse(line);
                }else if(mode_count == 1)
                {
                    //存放編號
                    string[] spli = { "  ", " ", "," };
                    string[] number = line.Split(spli, System.StringSplitOptions.RemoveEmptyEntries);
                    int thing_index = 0;
                    foreach(String number_reg in number)
                    {
                        PackThing[thing_index] = new BackPackThing(Int32.Parse(number_reg));
                        //MessageBox.Show("" + PackThing[thing_index].thing_num);
                        thing_index++;
                    }
                    //獲取內容數量
                    packthing_number = thing_index;
                }
                else if(mode_count == 2)
                {
                    //存放重量
                    string[] spli = { "  ", " ", "," };
                    string[] number = line.Split(spli, System.StringSplitOptions.RemoveEmptyEntries);
                    int thing_index = 0;
                    foreach (String number_reg in number)
                    {
                        PackThing[thing_index].store_weight(Int32.Parse(number_reg));
                        //MessageBox.Show("" + PackThing[thing_index].thing_weight);
                        thing_index++;
                    }
                }
                else if (mode_count == 3)
                {
                    //存放價值
                    string[] spli = { "  ", " ", "," };
                    string[] number = line.Split(spli, System.StringSplitOptions.RemoveEmptyEntries);
                    int thing_index = 0;
                    foreach (String number_reg in number)
                    {
                        PackThing[thing_index].store_value(Int32.Parse(number_reg));
                        //MessageBox.Show("" + PackThing[thing_index].thing_value);
                        thing_index++;
                    }
                }
                mode_count++;
            }

        }

        private void Knapsack() {
            int set_number = (int)Math.Pow(2, packthing_number);
            String choose_reg = "";
            int choose_weight = 0;
            int choose_value = 0;
            for (int i = 0; i < set_number; i++)
            {
                //初始化
                choose_reg = "";
                choose_weight = 0;
                choose_value = 0;

                //轉換成2進位
                string binary = Convert.ToString(i, 2);
                
                //全部掃描一次
                for (int j = binary.Length - 1; j >= 0; j--)
                {
                    
                    if(binary[j].Equals('1'))
                    {
                        choose_reg +=  PackThing[((binary.Length - 1) - j)].thing_num + " ";
                        choose_weight += PackThing[((binary.Length - 1) - j)].thing_weight;
                        choose_value += PackThing[((binary.Length - 1) - j)].thing_value;
                    }
                }

                if(choose_value > best_value && choose_weight <= Pack_LimitWeight)
                {
                    choose = choose_reg;
                    best_value = choose_value;
                    best_weight = choose_weight;
                }
            }

            MessageBox.Show("組合:" + choose + "\n價值:" + best_value + "\n重量:" + best_weight + "已經寫入原始檔案");
        }
    }
}
