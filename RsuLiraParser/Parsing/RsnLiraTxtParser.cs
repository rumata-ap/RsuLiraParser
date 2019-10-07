using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parsing
{
    public class RsnLiraTxtParser
    {
        private string source;
        private string path;

        public string GetTxtRsnFile()
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.DefaultExt = "*.*";
            ofd.Filter = "Все файлы (*.*)|*.*|Текст (*.txt)|*.txt";
            ofd.Title = "Выбор файла с расчетными сочетаниями нагрузок";
            ofd.ShowDialog();
            return ofd.FileName;
        }

        public void ReadRsuTxt()
        {
            //barsRsuSrc = new List<string>();
            //shellsRsuSrc = new List<string>();
            int type = 0;
            int lenline = 73;
            //Regex regex = new Regex(@"\d{1}$");
            if (path == null) { path = GetTxtRsnFile(); }
            if (path == "") { return; }
            source = "";
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                source = sr.ReadToEnd();
            }
        }

        public void RsnTableParsing()
        {
            if (source == null || source == "") return;
            string[] src= source.Split(new string[] { "--------------------------------------------------------------------------------\r\n" }, StringSplitOptions.None);
        }
    }
}
