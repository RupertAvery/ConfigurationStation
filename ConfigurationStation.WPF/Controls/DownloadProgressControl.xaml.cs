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

namespace ConfigurationStation.WPF.Controls
{

    /// <summary>
    /// Interaction logic for DownloadProgressControl.xaml
    /// </summary>
    public partial class DownloadProgressControl : UserControl
    {
        public DownloadProgressModel Model { get; set; }
        public DownloadProgressControl()
        {
            Model = new DownloadProgressModel();
            Model.Value = 0;
            Model.Maximum = 100;
            InitializeComponent();
        }
    }
}
