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
using Parsing;

namespace RsuLiraParser
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            RsnLiraTxtParser pars = new RsnLiraTxtParser();
            pars.ReadRsuTxt();
            pars.RsnTableParsing();
            //btCancel.IsManipulationEnabled = true;
        }

        private void pb_SizeChanged(object sender, SizeChangedEventArgs e)
        {
           statusBar.Width = btCancel.Width;
        }
    }
}
