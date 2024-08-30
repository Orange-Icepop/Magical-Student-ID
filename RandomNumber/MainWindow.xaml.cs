using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RandomNumber
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SkipNum.IsEnabled = false;
            TimesNum.IsEnabled = false;
            TimesNum.Text = "1";
        }

        #region 勾选框事件
        private void EnableSkipNum(object sender, RoutedEventArgs e)
        {
            SkipNum.IsEnabled = true;
        }

        private void DisableSkipNum(object sender, RoutedEventArgs e)
        {
            SkipNum.IsEnabled = false;
        }

        private void EnableRandCount(object sender, RoutedEventArgs e)
        {
            TimesNum.IsEnabled = true;
        }

        private void DisableRandCount(object sender, RoutedEventArgs e)
        {
            TimesNum.Text = "1";
            TimesNum.IsEnabled = false;
        }
        #endregion

        private List<int> SkipNumList = new List<int>();

        private void SkipNumCheck()
        {
            var skipNums = SkipNum.Text.Split(',');
            SkipNumList.Clear();
            foreach (var item in skipNums)
            {
                item.Trim();
                if (int.TryParse(item, out int num) && num >= 0 && num <= 1000000)
                {
                    SkipNumList.Add(num);
                }
                else
                {
                    MessageBox.Show("请输入正确的排除数字。\n排除数字规则：单个正整数，以半角逗号分割。", "错误：排除数字错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void RandNum(object sender, RoutedEventArgs e)
        {
            /*
            if (MinNum.Text != "" && MaxNum.Text != "" && TimesNum.Text != "")
            {
                if (int.TryParse(MinNum.Text, out int smallNum) && int.TryParse(MaxNum.Text, out int largeNum) && int.TryParse(TimesNum.Text, out int times))
                {
                    if (smallNum <= 1000000 && smallNum >= 0 && largeNum <= 1000000 && largeNum >= 0 && times <= 100 && times >= 0)
                    {
                        if (smallNum <= largeNum)
                        {
            */
            if (Verify())
            {
                Random random = new Random();
                List<string> resultList = new List<string>();
                StringBuilder result = new StringBuilder();
                for (int i = 0; i < times; i++)
                {
                    int middle = random.Next(smallNum, largeNum + 1);
                    if (SkipNumList.Contains(middle))
                    {
                        i--;
                    }
                    else if (AvoidRepeat.IsChecked == true && resultList.Contains(middle.ToString()))
                    {
                        i--;
                    }
                    else
                    {
                        resultList.Add(middle.ToString());
                        resultList.Add("\n");
                    }
                }
                foreach (string item in resultList)
                {
                    result.Append(item);
                }
                string finalResult = result.ToString();
                MessageBox.Show(finalResult, "随机数结果", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            /*
                        }
                        else
                        {
                            MessageBox.Show("最大数值小于最小数值", "错误：数值大小错误", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("请确保输入的数字在0~1,000,000以内且抽取数量在0~100以内", "错误：数值输入过大", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("请确保输入的是整数", "错误：数值输入格式错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("请输入数字范围", "错误：没有数值输入", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            */
        }

        private bool Verify()
        {
            if (MinNum.Text == "" || MaxNum.Text == "" || TimesNum.Text == "")
            {
                MessageBox.Show("请输入数字范围", "错误：没有数值输入", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (!int.TryParse(MinNum.Text, out smallNum) || !int.TryParse(MaxNum.Text, out largeNum) || !int.TryParse(TimesNum.Text, out times))
            {
                MessageBox.Show("请确保输入的是整数", "错误：数值输入格式错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (smallNum >= 1000000 || smallNum <= 0 || largeNum >= 1000000 || largeNum <= 0 || times >= 100 || times <= 0)
            {
                MessageBox.Show("请确保输入的数字在0~1,000,000以内且抽取数量在0~100以内", "错误：数值输入超过范围", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (smallNum >= largeNum)
            {
                MessageBox.Show("最大数值小于最小数值", "错误：数值大小错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (AvoidRepeat.IsChecked == true && largeNum - smallNum < times)
            {
                MessageBox.Show("避免重复抽取时，最大数值与最小数值之差应大于抽取数量", "错误：数值大小错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        private int smallNum;
        private int largeNum;
        private int times;

        /*
        private void SaveConf(object sender, RoutedEventArgs e)
        {
            var conf = new
            {
                MinNum = MinNum.Text,
                MaxNum = MaxNum.Text,
                EnableSkipNum = EnableSkip.IsChecked,
                SkipNum = SkipNum.Text,
            }
            File.WriteAllText("conf.json", "");
        }
        */
    }
}
