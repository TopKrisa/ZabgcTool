using FastColoredTextBoxNS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace ZabgcTool_SDK_.IDE
{
    /// <summary>
    /// Логика взаимодействия для IDEWindow.xaml
    /// </summary>
    public partial class IDEWindow : Window
    {
        private string _Path;
        string[] Text;
        string _addres;

        public IDEWindow(Language language, string path, string Address)
        {
            FastColoredTextBox textBox = new FastColoredTextBox();
            MouseLeftButtonDown += (s, e) =>
              {
                  WindowState = WindowState.Normal;
                  DragMove();
              };
            textBox.KeyDown += (s, e) =>
            {
                if(e.KeyCode == Keys.S&&e.Control)
                {
                    SaveDataToFTP();
                }
                if (e.KeyCode == Keys.S && e.Control&& e.Shift)
                {
                    SaveToPC_Click(null, null);
                }
            };
            InitializeComponent();
            try
            {
                string encoding = string.Empty;


                _Path = path;
                Stream fs = new FileStream($"{_Path}", FileMode.Open);
                using (StreamReader sr = new StreamReader(fs, true))
                    Encoding.Content = sr.CurrentEncoding.EncodingName;
                NameFile.Content = Path.GetFileName(_Path);
                Line.Content = $"Строка: {textBox.Selection.Start.iLine + 1}";
                Column.Content = $"Позиция: {textBox.Selection.Start.iChar}";
                _addres = Address;
                Host.Child = textBox;
                textBox.Font = new System.Drawing.Font("Consolas", 13);
                textBox.ForeColor = System.Drawing.Color.WhiteSmoke;
                textBox.IndentBackColor = System.Drawing.Color.Transparent;
                textBox.PaddingBackColor = System.Drawing.Color.Transparent;
                textBox.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
                textBox.LineNumberColor = System.Drawing.Color.FromArgb(30, 56, 119);
                textBox.ServiceLinesColor = Color.FromArgb(48, 48, 49);
                Language.Content = language;
                textBox.Language = language;
                textBox.Text = File.ReadAllText(_Path);
                new FastColoredTextBoxNS.SyntaxHighlighter(textBox).HTMLSyntaxHighlight(new Range(textBox));

                Text = textBox.Text.Split('\n');
                Strings.Content = $"Строк: {textBox.Text.Split('\n').Length}";
                Symbols.Content = $"Cимволов: {textBox.Text.Length}";

            }
            catch
            {

            }
            textBox.TextChanged += (s, e) =>
            {
                Text = textBox.Text.Split('\n');
                Strings.Content = $"Строк: {textBox.Text.Split('\n').Length}";
                Symbols.Content = $"Cимволов: {textBox.Text.Length}";
                SaveChecker.Visibility = Visibility.Visible;

            };
            textBox.SelectionChanged += (s, e) =>
            {
                Line.Content = $"Строка: {textBox.Selection.Start.iLine + 1}";
                Column.Content = $"Позиция: {textBox.Selection.Start.iChar}";
            };

        }

        private void butns_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void butn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        protected override void OnClosed(EventArgs e)
        {

            File.Delete(_Path);
            base.OnClosed(e);

        }
        private void SaveToFTP_Click(object sender, RoutedEventArgs e)
        {
            SaveDataToFTP();
        }
        private void SaveToPC_Click(object sender, RoutedEventArgs e)
        {

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = System.IO.Path.GetFileName(_Path);
            saveFileDialog.DefaultExt = System.IO.Path.GetExtension(_Path);

            if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                File.Create(saveFileDialog.FileName).Close();
                File.WriteAllLines(saveFileDialog.FileName, Text);
            }
            SaveChecker.Visibility = Visibility.Collapsed;
        }
        private void SaveDataToFTP()
        {
            File.WriteAllLines(_Path, Text);
            new FTP.MainFTP(this).FTPSaver(_Path, _addres);
            SaveChecker.Visibility = Visibility.Collapsed;
        }
    }
}
