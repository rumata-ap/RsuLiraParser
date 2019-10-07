using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Parsing
{
    public class RsuLiraTxtParser
    {
        string path;
        string pathXML;
        string pathSQLite;
        List<string> barsRsuSrc;
        List<string> shellsRsuSrc;
        List<RsuBar> barsRsu;
        List<RsuShell> shellsRsu;
        XDocument xdoc;
        XElement Combinations;
        XElement Bars;
        XElement Shells;
        typeRsu type = typeRsu.C;

        public delegate void InsertSQLiteDbTableStateHandler(int counter);
        public delegate void CountSQLiteDbTableStateHandler(string tableName, int count);
        public event InsertSQLiteDbTableStateHandler RecordInserted;
        public event CountSQLiteDbTableStateHandler TableCounted;
        public List<RsuBar> BarsRsu { get => barsRsu; }
        public List<RsuShell> ShellsRsu { get => shellsRsu; }
        public string Path { get => path; set => path = value; }        
        public string PathXML { get => pathXML; set => pathXML = value; }
        public string PathSQLite { get => pathSQLite; set => pathSQLite = value; }
        public XDocument Xdoc { get => xdoc; }
        public typeRsu Type { get => type; set => type = value; }

        public string Jhgj { get; private set; }

        public string GetTxtRsuFile()
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.DefaultExt = "*.*";
            ofd.Filter = "Все файлы (*.*)|*.*|Текст (*.txt)|*.txt";
            ofd.Title = "Выбор файла с расчетными сочетаниями усилий";
            ofd.ShowDialog();
            return ofd.FileName;
        }

        public static string GetSQLiteDBFile()
        {
            Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
            ofd.DefaultExt = "*.db3";
            ofd.Filter = "База данных SQLite (*.db3)|*.db3|Все файлы (*.*)|*.*";
            ofd.Title = "Выбор базы данных SQLite  для записи расчетных сочетаний усилий";
            ofd.ShowDialog();
            return ofd.FileName;
        }

        public static string SaveSQLiteDBFile()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = "*.db3";
            sfd.Filter = "База данных SQLite (*.db3)|*.db3";
            sfd.Title = "Сохранение базы данных SQLite";
            //sfd.AddExtension = true;
            sfd.OverwritePrompt = true;
            sfd.ShowDialog();
            return sfd.FileName;
        }

        public string ChooseXMLFolder()
        {
            string res = "";
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Выбор папки для сохранения файлов РСУ";
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                res = fbd.SelectedPath;
            }
            return res;
        }

        public void ReadRsuTxt()
        {
            barsRsuSrc = new List<string>();
            shellsRsuSrc = new List<string>();
            int type = 0;
            int lenline = 73;
            Regex regex = new Regex(@"\d{1}$");
            if (path == null) { return; }
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.Default))
            {
                string line, sline, ssline;
                string startLine, insertLine; startLine = "";
                while ((line = sr.ReadLine()) != null)
                {
                    if (line.Length > 0)
                    {
                        if (line.Substring(0, 1) == "|")
                        {
                            sline = line.Substring(26, 7);
                            ssline = line.Substring(89, 8);

                            switch (sline)
                            {
                                case "  N    ":
                                    type = 0;
                                    break;
                                case "  NX   ":
                                    type = 1;
                                    break;
                            }

                            if (ssline == "   RZ   ") { type = 2; }

                            switch (type)
                            {
                                case 0:
                                    lenline = 73;
                                    break;
                                case 1:
                                    lenline = 89;
                                    break;
                                case 2:
                                    lenline = 97;
                                    break;
                            }

                            sline = line.Substring(0, lenline);
                            ssline = line.Substring(0, 10);

                            MatchCollection matches1 = regex.Matches(ssline);
                            if (matches1.Count > 0) { startLine = ssline; insertLine = line; }
                            else { insertLine = line.Substring(10); insertLine = insertLine.Insert(0, startLine); }

                            MatchCollection matches2 = regex.Matches(sline);
                            if (matches2.Count > 0 && type == 0) { barsRsuSrc.Add(insertLine.Substring(1, lenline)); }
                            else if (matches2.Count > 0 && type > 0) { shellsRsuSrc.Add(insertLine.Substring(1, lenline)); }

                            if (matches2.Count == 0)
                            {
                                sline = line.Substring(0, lenline - 5);
                                MatchCollection matches3 = regex.Matches(sline);
                                if (matches3.Count > 0 && type == 0) { barsRsuSrc.Add(insertLine.Substring(1, lenline)); }
                                else if (matches3.Count > 0 && type > 0) { shellsRsuSrc.Add(insertLine.Substring(1, lenline)); }
                            }
                        }
                    }
                }
            }
        }

        public void CreateRsuArrays()
        {
            barsRsu = new List<RsuBar>(); shellsRsu = new List<RsuShell>();
            Task t1 = new Task(FillingListRsuBarsParallel);
            Task t2 = new Task(FillingListRsuShellsParallel);
            if (barsRsuSrc.Count > 0) t1.Start();
            if (shellsRsuSrc.Count > 0) t2.Start();
            if (barsRsuSrc.Count > 0) t1.Wait();
            if (shellsRsuSrc.Count > 0) t2.Wait();
            barsRsuSrc = null; shellsRsuSrc = null;
        }

        public void WriteXML()
        {
            xdoc = new XDocument();
            Combinations = new XElement("Combinations");
            Bars = new XElement("BarsCombinations");
            Shells = new XElement("ShellsCombinations");
            Combinations.Add(new object[] { Bars, Shells });
            Task t1 = new Task(FillingXMLRsuBars); t1.Start();
            Task t2 = new Task(FillingXMLRsuShells); t2.Start();
            t1.Wait(); t2.Wait();
            xdoc.Add(Combinations);
            TableCounted("Данные успешно записаны в файл", barsRsu.Count);
            xdoc.Save(pathXML);
            xdoc = null;
        }

        public void WriteToSQLiteDB()
        {
            SQLiteConnection m_dbConn;
            SQLiteCommand m_sqlCmd;

            if (!File.Exists(pathSQLite)) { SQLiteConnection.CreateFile(pathSQLite); }
            using(m_dbConn = new SQLiteConnection("Data Source=" + pathSQLite + ";Version=3;"))
            {             
                m_dbConn.Open();
                m_sqlCmd = new SQLiteCommand("begin", m_dbConn);
                m_sqlCmd.ExecuteNonQuery();
                m_sqlCmd = new SQLiteCommand(CreateRsuBarTablesQuery(type), m_dbConn);
                m_sqlCmd.ExecuteNonQuery();
                m_sqlCmd = new SQLiteCommand("DELETE FROM Rsu" + type + "Bars", m_dbConn);
                m_sqlCmd.ExecuteNonQuery();
                m_sqlCmd = new SQLiteCommand(CreateRsuShellTablesQuery(type), m_dbConn);
                m_sqlCmd.ExecuteNonQuery();
                m_sqlCmd = new SQLiteCommand("DELETE FROM Rsu" + type + "Shells", m_dbConn);
                m_sqlCmd.ExecuteNonQuery();

                if (barsRsu!=null)
                {
                    TableCounted("Запись данных в таблицу Rsu" + type + "Bars", barsRsu.Count);
                    int i = 0;
                    foreach (RsuBar r in barsRsu)
                    {
                        m_sqlCmd = new SQLiteCommand(InsertRsuBarsTablesQuery(type, r.NumFe, r.NumSect, r.N, r.Mk, r.My, r.Qz, r.Mz, r.Qy), m_dbConn);
                        m_sqlCmd.ExecuteNonQuery();
                        i++; 
                        RecordInserted(i);
                    }
                }

                if (shellsRsu != null)
                {
                    TableCounted("Запись данных в таблицу Rsu" + type + "Shells", shellsRsu.Count);
                    int i = 0;
                    foreach (RsuShell r in shellsRsu)
                    {
                        m_sqlCmd = new SQLiteCommand(InsertRsuShellsTablesQuery(type, r.NumFe, r.NumSect,
                            r.Nx, r.Ny, r.Txy, r.Mx, r.My, r.Mxy, r.Qx, r.Qy, r.Rz), m_dbConn);
                        m_sqlCmd.ExecuteNonQuery();
                        i++;
                        RecordInserted(i);
                    }
                }

                m_sqlCmd = new SQLiteCommand("end", m_dbConn);
                m_sqlCmd.ExecuteNonQuery();
                TableCounted("Данные успешно записаны в базу", shellsRsu.Count);
            }
        }

        string CreateRsuBarTablesQuery(typeRsu typeRsu)
        {
            return "CREATE TABLE IF NOT EXISTS Rsu" + typeRsu + "Bars ( `ID` INTEGER PRIMARY KEY, `NumFe` INTEGER, `NumSect` INTEGER," +
                " `N` REAL, `Mk` REAL, `My` REAL, `Qz` REAL, `Mz` REAL, `Qy` REAL)";
        }

        string CreateRsuShellTablesQuery(typeRsu typeRsu)
        {
            return "CREATE TABLE IF NOT EXISTS Rsu" + typeRsu + "Shells ( `ID` INTEGER PRIMARY KEY, `NumFe` INTEGER, `NumSect` INTEGER," +
                    " `Nx` REAL, `Ny` REAL, `Txy` REAL, `Mx` REAL, `My` REAL, `Mxy` REAL, `Qx` REAL, `Qy` REAL, `Rz` REAL)";
        }

        string InsertRsuBarsTablesQuery(typeRsu typeRsu, int numFe, int numSect, params double[] forces)
        {
            return "INSERT INTO Rsu" + typeRsu + "Bars( NumFe, NumSect, N, Mk, My, Qz, Mz, Qy) VALUES" +
                String.Format("( {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})", numFe, numSect, forces[0], forces[1], forces[2], forces[3], forces[4], forces[5]);
        }

        string InsertRsuShellsTablesQuery(typeRsu typeRsu, int numFe, int numSect, params double[] forces)
        {
            return "INSERT INTO Rsu" + typeRsu + "Shells( NumFe, NumSect, Nx, Ny, Txy, Mx, My, Mxy, Qx, Qy, Rz) VALUES" +
                String.Format("( {0}, {1}, {2}, {3}, {4}, {5}, {6}, {7}, {8}, {9}, {10})", 
                numFe, numSect, forces[0], forces[1], forces[2], forces[3], forces[4], forces[5], forces[6], forces[7], forces[8]);
        }

        void AddRsuToDB()
        {
            RsuDBContext db = new RsuDBContext();
            if (barsRsu != null) { db.RsuBars.AddRange(barsRsu); db.RsuShells.AddRange(shellsRsu); }
            db.SaveChanges();
        }


        void FillingListRsuBars(int x)
        {
            RsuBar rsu = new RsuBar();
            rsu.NumFe = Convert.ToInt32(barsRsuSrc[x].Substring(0, 6));
            rsu.NumSect = Convert.ToInt32(barsRsuSrc[x].Substring(6, 3));
            rsu.N = Convert.ToDouble(barsRsuSrc[x].Substring(25, 7));
            rsu.Mk = Convert.ToDouble(barsRsuSrc[x].Substring(33, 7));
            rsu.My = Convert.ToDouble(barsRsuSrc[x].Substring(41, 7));
            rsu.Qz = Convert.ToDouble(barsRsuSrc[x].Substring(49, 7));
            rsu.Mz = Convert.ToDouble(barsRsuSrc[x].Substring(57, 7));
            rsu.Qy = Convert.ToDouble(barsRsuSrc[x].Substring(65, 7));
            barsRsu.Add(rsu); return;           
        }
        void FillingListRsuBarsParallel()
        {
            //TableCounted("Запись стержневых РСУ типа " + type + "в файл", barsRsu.Count);
            for (int i = 0; i < barsRsuSrc.Count; i++)
            {               
                FillingListRsuBars(i);
                //RecordInserted(i);
            }
            //TableCounted("Данные успешно записаны в файл", barsRsu.Count);
        }
        void FillingListRsuShells(int num)
        {
            RsuShell rsu = new RsuShell();
            rsu.NumFe = Convert.ToInt32(shellsRsuSrc[num].Substring(0, 6));
            rsu.NumSect = Convert.ToInt32(shellsRsuSrc[num].Substring(6, 3));
            rsu.Nx = Convert.ToDouble(shellsRsuSrc[num].Substring(25, 7));
            rsu.Ny = Convert.ToDouble(shellsRsuSrc[num].Substring(33, 7));
            rsu.Txy = Convert.ToDouble(shellsRsuSrc[num].Substring(41, 7));
            rsu.Mx = Convert.ToDouble(shellsRsuSrc[num].Substring(49, 7));
            rsu.My = Convert.ToDouble(shellsRsuSrc[num].Substring(57, 7));
            rsu.Mxy = Convert.ToDouble(shellsRsuSrc[num].Substring(65, 7));
            rsu.Qx = Convert.ToDouble(shellsRsuSrc[num].Substring(73, 7));
            rsu.Qy = Convert.ToDouble(shellsRsuSrc[num].Substring(81, 7));
            if (shellsRsuSrc[num].Length > 90) { rsu.Rz = Convert.ToDouble(shellsRsuSrc[num].Substring(89, 7)); }
            shellsRsu.Add(rsu); return;           
        }
        void FillingListRsuShellsParallel()
        {
            
            for (int i = 0; i < shellsRsuSrc.Count; i++)
            {
                FillingListRsuShells(i);
            }           
        }

        void FillingXMLRsuBars()
        {
            int i = 0;
            TableCounted("Запись стержневых РСУ типа " + type + "в файл", shellsRsu.Count);
            var barsRsuGroupsByNumFE = from t in barsRsu group t by t.NumFe;
            foreach (var item in barsRsuGroupsByNumFE)
            {                
                XAttribute numFE = new XAttribute("number", item.Key);
                XElement FE = new XElement("FE");
                FE.Add(numFE);
                var barsRsuGroupsByNumSect = from t in item group t by t.NumSect;
                foreach (var g in barsRsuGroupsByNumSect)
                {
                    XElement sect = new XElement("Section");
                    XAttribute numSect = new XAttribute("number", g.Key);
                    sect.Add(numSect);
                    foreach (RsuBar rsu in g)
                    {
                        XElement comb = new XElement("Comb");
                        XAttribute n = new XAttribute("N", rsu.N);
                        XAttribute mk = new XAttribute("Mk", rsu.Mk);
                        XAttribute my = new XAttribute("My", rsu.My);
                        XAttribute qz = new XAttribute("Qz", rsu.Qz);
                        XAttribute mz = new XAttribute("Mz", rsu.Mz);
                        XAttribute qy = new XAttribute("Qy", rsu.Qy);
                        comb.Add(new object[] { n, mk, my, qz, mz, qy });
                        sect.Add(comb); i++; RecordInserted(i);
                    }
                    FE.Add(sect);
                }
                Bars.Add(FE);
            }
        }

        void FillingXMLRsuShells()
        {
            int i = 0;
            TableCounted("Запись оболочечных РСУ типа " + type + "в файл", shellsRsu.Count);
            var shellsRsuGroupsByNumFE = from t in shellsRsu group t by t.NumFe;
            foreach (var item in shellsRsuGroupsByNumFE)
            {
                XAttribute numFE = new XAttribute("number", item.Key);
                XElement FE = new XElement("FE");
                FE.Add(numFE);
                foreach (RsuShell rsu in item)
                {
                    XElement comb = new XElement("Comb");
                    XAttribute nx = new XAttribute("Nx", rsu.Nx);
                    XAttribute ny = new XAttribute("Ny", rsu.Ny);
                    XAttribute txy = new XAttribute("Txy", rsu.Txy);
                    XAttribute mx = new XAttribute("Mx", rsu.Mx);
                    XAttribute mxy = new XAttribute("Mxy", rsu.Mxy);
                    XAttribute qx = new XAttribute("Qx", rsu.Qx);
                    XAttribute mys = new XAttribute("My", rsu.My);
                    XAttribute qys = new XAttribute("Qy", rsu.Qy);
                    XAttribute rz = new XAttribute("Rz", rsu.Rz);
                    comb.Add(new object[] { nx, ny, txy, mx, mys, mxy, qx, qys, rz });
                    FE.Add(comb); i++; RecordInserted(i);
                }
                Shells.Add(FE);
            }
            
        }
    }
}
