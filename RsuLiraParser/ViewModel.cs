using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Parsing;

namespace RsuLiraParser
{
    partial class ViewModel : INotifyPropertyChanged
    {
        string patchRsu;
        string patchRsuL;
        string patchRsuN;
        string patchRsuNL;
        string pathSQLiteDB;
        string pathXML;
        string statusReadRsu = "Выбор файла с расчетными кратковременными РСУ";
        string statusReadRsuL = "Выбор файла с расчетными длительными РСУ";
        string statusReadRsuN = "Выбор файла с нормативными кратковременными РСУ";
        string statusReadRsuNL = "Выбор файла с нормативными длительными РСУ";
        string statusXML = "Сохранить РСУ в XML";
        string statusSQL = "Сохранить РСУ в базу SQLite";
        string dbFileName;
        int countRsu;
        int counter;
        string progressHint;

        RsuLiraTxtParser rsu;
        RsuLiraTxtParser rsuL;
        RsuLiraTxtParser rsuN;
        RsuLiraTxtParser rsuNL;


        RelayCommand selectRsuFile;
        RelayCommand selectRsuLFile;
        RelayCommand selectRsuNFile;
        RelayCommand selectRsuNLFile;
        RelayCommand writeRsuToXMLFile;
        RelayCommand createSQLiteDb;
        RelayCommand saveSQLiteDb;
        RelayCommand updateFull;


        public string PatchRsu { get => patchRsu; set { patchRsu = value; OnPropertyChanged(); } }
        public string PatchRsuL { get => patchRsuL; set { patchRsuL = value; OnPropertyChanged(); } }
        public string PatchRsuN { get => patchRsuN; set { patchRsuN = value; OnPropertyChanged(); } }
        public string PatchRsuNL { get => patchRsuNL; set { patchRsuNL = value; OnPropertyChanged(); } }
        public string PathSQLiteDB { get => pathSQLiteDB; set { pathSQLiteDB = value; OnPropertyChanged(); } }
        public string StatusReadRsu { get => statusReadRsu; set { statusReadRsu = value; OnPropertyChanged(); } }
        public string StatusReadRsuL { get => statusReadRsuL; set { statusReadRsuL = value; OnPropertyChanged(); } }
        public string StatusReadRsuN { get => statusReadRsuN; set { statusReadRsuN = value; OnPropertyChanged(); } }
        public string StatusReadRsuNL { get => statusReadRsuNL; set { statusReadRsuNL = value; OnPropertyChanged(); } }
        public string StatusXML { get => statusXML; set { statusXML = value; OnPropertyChanged(); } }
        public string StatusSQL { get => statusSQL; set { statusSQL = value; OnPropertyChanged(); } }
        public int CountRsu { get => countRsu; set { countRsu = value; OnPropertyChanged(); } }
        public int Counter { get => counter; set { counter = value; OnPropertyChanged(); } }
        public string ProgressHint { get => progressHint; set { progressHint = value; OnPropertyChanged(); } }
        public string PathXML { get => pathXML; set { pathXML = value; OnPropertyChanged(); } }

        #region Команды
        public RelayCommand SelectRsuFile
        {
            get { return selectRsuFile ?? (selectRsuFile = new RelayCommand(obj => { Task.Factory.StartNew(ReadRsu); })); }
        }
        public RelayCommand SelectRsuLFile
        {
            get { return selectRsuLFile ?? (selectRsuLFile = new RelayCommand(obj => { Task.Factory.StartNew(ReadRsuL); })); }
        }
        public RelayCommand SelectRsuNFile
        {
            get { return selectRsuNFile ?? (selectRsuNFile = new RelayCommand(obj => { Task.Factory.StartNew(ReadRsuN); })); }
        }
        public RelayCommand SelectRsuNLFile
        {
            get { return selectRsuNLFile ?? (selectRsuNLFile = new RelayCommand(obj => { Task.Factory.StartNew(ReadRsuNL); })); }
        }
        public RelayCommand WriteRsuToXMLFile
        {
            get { return writeRsuToXMLFile ?? (writeRsuToXMLFile = new RelayCommand(obj => { WriteRsuToXML(); })); }
        }
        public RelayCommand CreateSQLiteDb
        {
            get { return createSQLiteDb ?? (createSQLiteDb = new RelayCommand(obj => { CreateSQLiteDB(); })); }
        }
        public RelayCommand SaveSQLiteDb
        {
            get { return saveSQLiteDb ?? (saveSQLiteDb = new RelayCommand(obj => { SaveRsuToSQLiteDB(); /*WriteToSQLiteDB();*/ })); }
        }

        public RelayCommand UpdateFull { get { return updateFull ?? (updateFull = new RelayCommand(obj => { clearVM(); })); } }

        #endregion

        #region Методы

        void Test()
        {
            //Thread.Sleep(1000);
            StatusSQL = "Подождите, идет обработка данных";
            RsuLiraTxtParser.GetSQLiteDBFile();
            //Thread.Sleep(2000);
            StatusSQL = "Сохранить РСУ в базу SQLite";
        }
        void ReadRsu()
        {
            StatusReadRsu = "Подождите, идет обработка данных";
            if (rsu != null) { rsu = null; }
            rsu = new RsuLiraTxtParser() { Type = typeRsu.C };
            PatchRsu = rsu.GetTxtRsuFile();
            if (patchRsu == null || patchRsu == "") { PatchRsu = null; StatusReadRsu = "Выбор файла с расчетными кратковременными РСУ"; return; }
            rsu.Path = patchRsu;
            rsu.ReadRsuTxt();
            rsu.CreateRsuArrays();
            StatusReadRsu = "Выбор файла с расчетными кратковременными РСУ";
        }
        void ReadRsuL()
        {
            StatusReadRsuL = "Подождите, идет обработка данных";
            if (rsuL != null) { rsuL = null; }
            rsuL = new RsuLiraTxtParser() { Type = typeRsu.CL };
            PatchRsuL = rsuL.GetTxtRsuFile();
            if (patchRsuL == null || patchRsuL == "") { PatchRsuL = null; StatusReadRsuL = "Выбор файла с расчетными длительными РСУ"; return; }
            rsuL.Path = patchRsuL;
            rsuL.ReadRsuTxt();
            rsuL.CreateRsuArrays();
            StatusReadRsuL = "Выбор файла с расчетными длительными РСУ";
        }
        void ReadRsuN()
        {
            StatusReadRsuN = "Подождите, идет обработка данных";
            if (rsuN != null) { rsuN = null; }
            rsuN = new RsuLiraTxtParser() { Type = typeRsu.N };
            PatchRsuN = rsuN.GetTxtRsuFile();
            if (patchRsuN == null || patchRsuN == "") { PatchRsuN = null; StatusReadRsuN = "Выбор файла с нормативными кратковременными РСУ"; return; }
            rsuN.Path = patchRsuN;
            rsuN.ReadRsuTxt();
            rsuN.CreateRsuArrays();
            StatusReadRsuN = "Выбор файла с нормативными кратковременными РСУ";
        }
        void ReadRsuNL()
        {
            StatusReadRsuNL = "Подождите, идет обработка данных";
            if (rsuNL != null) { rsuNL = null; }
            rsuNL = new RsuLiraTxtParser() { Type = typeRsu.NL };
            PatchRsuNL = rsuNL.GetTxtRsuFile();
            if (patchRsuNL == null || patchRsuNL == "") { PatchRsuNL = null; StatusReadRsuNL = "Выбор файла с нормативными длительными РСУ"; return; }
            rsuNL.Path = patchRsuNL;
            rsuNL.ReadRsuTxt();
            rsuNL.CreateRsuArrays();
            StatusReadRsuNL = "Выбор файла с нормативными длительными РСУ";
        }
        async void WriteRsuToXML()
        {
            if (StatusReadRsuNL != "Подождите, идет обработка данных" && StatusReadRsuN != "Подождите, идет обработка данных" &&
                                 StatusReadRsuL != "Подождите, идет обработка данных" && StatusReadRsu != "Подождите, идет обработка данных")
            {
                if (patchRsu != null && rsu == null) ReadRsu();
                if (patchRsuL != null && rsuL == null) ReadRsuL();
                if (patchRsuN != null && rsuN == null) ReadRsuN();
                if (patchRsuNL != null && rsuNL == null) ReadRsuNL();
                Task t1, t2, t3, t4;
                void foo() { bool a = true; }
                t1 = new Task(foo);
                t2 = new Task(foo);
                t3 = new Task(foo);
                t4 = new Task(foo);
                StatusXML = "Подождите, идет обработка данных";
                if (rsu != null || rsuL != null || rsuN != null || rsuNL != null)
                {
                    string folderPath = "";
                    if (rsu != null) { folderPath = rsu.ChooseXMLFolder(); PathXML = folderPath; rsu.PathXML = folderPath + "\\РСУ_расчетные.xml"; }
                    if (rsuL != null) { if (folderPath == "") { folderPath = rsuL.ChooseXMLFolder(); PathXML = folderPath; } rsuL.PathXML = folderPath + "\\РСУ_расчетные_длительные.xml"; }
                    if (rsuN != null) { if (folderPath == "") { folderPath = rsuN.ChooseXMLFolder(); PathXML = folderPath; } rsuN.PathXML = folderPath + "\\РСУ_нормативные.xml"; }
                    if (rsuNL != null) { if (folderPath == "") { folderPath = rsuNL.ChooseXMLFolder(); PathXML = folderPath; } rsuNL.PathXML = folderPath + "\\РСУ_нормативные_длительные.xml"; }
                }
                if (rsu != null) { rsu.RecordInserted += ChangeCounter; rsu.TableCounted += ChangeProdressHint; t1 = new Task(rsu.WriteXML); t1.Start(); }
                if (rsuL != null) { rsuL.RecordInserted += ChangeCounter; rsuL.TableCounted += ChangeProdressHint; t2 = new Task(rsuL.WriteXML); t2.Start(); }
                if (rsuN != null) { rsuN.RecordInserted += ChangeCounter; rsuN.TableCounted += ChangeProdressHint; t3 = new Task(rsuN.WriteXML); t3.Start(); }
                if (rsuNL != null) { rsuNL.RecordInserted += ChangeCounter; rsuNL.TableCounted += ChangeProdressHint; t4 = new Task(rsuNL.WriteXML); t4.Start(); }
                if (rsu != null) t1.Wait();
                if (rsuL != null) t2.Wait();
                if (rsuN != null) t3.Wait();
                if (rsuNL != null) t4.Wait();
                StatusXML = "Сохранить РСУ в XML";
            }

        }

        void CreateSQLiteDB()
        {
            dbFileName = RsuLiraTxtParser.SaveSQLiteDBFile();
            if (dbFileName == null || dbFileName == "") { return; }
            if (rsu != null) rsu.PathSQLite = dbFileName;
            if (rsuL != null) rsuL.PathSQLite = dbFileName;
            if (rsuN != null) rsuN.PathSQLite = dbFileName;
            if (rsuNL != null) rsuNL.PathSQLite = dbFileName;
            PathSQLiteDB = dbFileName;
        }

        async void SaveRsuToSQLiteDB()
        {
            if (StatusReadRsuNL == "Подождите, идет обработка данных" || StatusReadRsuN == "Подождите, идет обработка данных" ||
                                 StatusReadRsuL == "Подождите, идет обработка данных" || StatusReadRsu == "Подождите, идет обработка данных") { return; }
            if (StatusSQL == "Подождите, идет обработка данных") return;
            StatusSQL = "Подождите, идет обработка данных";

            if (rsu != null) rsu.PathSQLite = dbFileName;
            if (rsuL != null) rsuL.PathSQLite = dbFileName;
            if (rsuN != null) rsuN.PathSQLite = dbFileName;
            if (rsuNL != null) rsuNL.PathSQLite = dbFileName;
            if (dbFileName == null || dbFileName == "")
            {
                dbFileName = RsuLiraTxtParser.GetSQLiteDBFile();                
                if (dbFileName == null || dbFileName == "") return;
                if (rsu != null) rsu.PathSQLite = dbFileName;
                if (rsuL != null) rsuL.PathSQLite = dbFileName;
                if (rsuN != null) rsuN.PathSQLite = dbFileName;
                if (rsuNL != null) rsuNL.PathSQLite = dbFileName;               
                PathSQLiteDB = dbFileName;
            }

            await Task.Run(() =>
            {
                if (rsu != null) { rsu.RecordInserted += ChangeCounter; rsu.TableCounted += ChangeProdressHint; rsu.WriteToSQLiteDB(); }
                if (rsuL != null) { rsuL.RecordInserted += ChangeCounter; rsuL.TableCounted += ChangeProdressHint; rsuL.WriteToSQLiteDB(); }
                if (rsuN != null) { rsuN.RecordInserted += ChangeCounter; rsuN.TableCounted += ChangeProdressHint; rsuN.WriteToSQLiteDB(); }
                if (rsuNL != null) { rsuNL.RecordInserted += ChangeCounter; rsuNL.TableCounted += ChangeProdressHint; rsuNL.WriteToSQLiteDB(); }
                if (File.Exists("rsuDB.db3")) { File.Copy("rsuDB.db3", pathSQLiteDB, true); }
                StatusSQL = "Сохранить РСУ в базу SQLite";               
            });

            //MessageBox.Show("Данные успешно сохранены");
        }

        void ChangeCounter(int i) { Counter = i; }
        void ChangeProdressHint(string message, int count)
        {
            ProgressHint = message;
            CountRsu = count;
        }

        void clearVM()
        {
            Application.Current.MainWindow.DataContext = new ViewModel();
        }


        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
