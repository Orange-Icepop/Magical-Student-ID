using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RandomNumber.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using Microsoft.Win32;

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
            this.Loaded += LoadConf;
        }

        #region 勾选框事件
        private void EnableSkipNum(object sender, RoutedEventArgs e)
        {
            SkipNum.IsEnabled = true;
        }

        private void DisableSkipNum(object sender, RoutedEventArgs e)
        {
            SkipNum.IsEnabled = false;
            SkipNum.Text = string.Empty;
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

        private List<int> SkipNumCheck()
        {
            var ls = new List<int>();
            if (EnableSkip.IsChecked == false || SkipNum.Text == "")
            {
                return ls;
            }
            var skipNums = SkipNum.Text.Split(',');
            foreach (var item in skipNums)
            {
                item.Trim();
                if (int.TryParse(item, out int num) && num >= 0 && num <= 1000000)
                {
                    ls.Add(num);
                }
                else
                {
                    MessageBox.Show("请输入正确的排除数字。\n排除数字规则：单个正整数，以半角逗号分割。", "错误：排除数字错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    throw new Exception("Skip number format error");
                }
            }
            return ls;
        }

        private void RandNum(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Verify())
                {
                    var skipList = SkipNumCheck();
                    StringBuilder result = new StringBuilder();
                    var resultList = RandomService.StartRandom(smallNum, largeNum, skipList, (uint)times, AvoidRepeat.IsChecked == true);
                    foreach (var num in resultList)
                    {
                        result.AppendLine(num.ToString());
                    }
                    string finalResult = result.ToString();
                    MessageBox.Show(finalResult, "随机数结果", MessageBoxButton.OK, MessageBoxImage.Information);
                    AskForSavingFile(finalResult);
                }
            }
            catch (Exception ex)
            {
                // Skip number format error
                MessageBox.Show("抽取失败", $"抽取因以下错误而中止：{ex.Message}", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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

        private void SaveConf(object sender, RoutedEventArgs e)
        {
            if (Verify())
            {
                var conf = new
                {
                    MinNum = MinNum.Text,
                    MaxNum = MaxNum.Text,
                    EnableSkipNum = EnableSkip.IsChecked,
                    SkipNum = SkipNum.Text,
                    AvoidRepeat = AvoidRepeat.IsChecked,
                    EnableTimes = EnableTimes.IsChecked,
                    TimesNum = TimesNum.Text,
                };
                string configString = JsonConvert.SerializeObject(conf, Formatting.Indented);
                File.WriteAllText("conf.json", configString);
                MessageBox.Show("配置已保存", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void LoadConf(object sender, EventArgs e)
        {
            if (File.Exists("conf.json"))
            {
                try
                {
                    var conf = JObject.Parse(File.ReadAllText("conf.json"));
                    MinNum.Text = conf["MinNum"].ToString();
                    MaxNum.Text = conf["MaxNum"].ToString();
                    EnableSkip.IsChecked = bool.Parse(conf["EnableSkipNum"].ToString());
                    SkipNum.IsEnabled = bool.Parse(conf["EnableSkipNum"].ToString());
                    SkipNum.Text = conf["SkipNum"].ToString();
                    AvoidRepeat.IsChecked = bool.Parse(conf["AvoidRepeat"].ToString());
                    EnableTimes.IsChecked = bool.Parse(conf["EnableTimes"].ToString());
                    TimesNum.IsEnabled = bool.Parse(conf["EnableTimes"].ToString());
                    TimesNum.Text = conf["TimesNum"].ToString();
                }
                catch
                {
                    MessageBox.Show("配置文件损坏，将不载入配置文件", "配置文件错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    MinNum.Text = "";
                    MaxNum.Text = "";
                    EnableSkip.IsChecked = false;
                    SkipNum.IsEnabled = false;
                    SkipNum.Text = "";
                    AvoidRepeat.IsChecked = false;
                    EnableTimes.IsChecked = false;
                    TimesNum.IsEnabled = false;
                    TimesNum.Text = "1";
                }
            }
            else
            {
                MinNum.Text = "";
                MaxNum.Text = "";
                EnableSkip.IsChecked = false;
                SkipNum.IsEnabled = false;
                SkipNum.Text = "";
                AvoidRepeat.IsChecked = false;
                EnableTimes.IsChecked = false;
                TimesNum.IsEnabled = false;
                TimesNum.Text = "1";
            }
        }

        private void AskForSavingFile(string result)
        {
            var askResult = MessageBox.Show("是否保存结果到文件？", "提示", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (askResult != MessageBoxResult.Yes) return;
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "文本文档 (*.txt)|*.txt",
                Title = "保存随机数结果到文件"
            };
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllText(saveFileDialog.FileName, result);
                MessageBox.Show("文件已保存", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}
