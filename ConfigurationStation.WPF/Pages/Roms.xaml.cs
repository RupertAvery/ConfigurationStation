using ConfigurationStation.WPF.Controllers;
using ConfigurationStation.WPF.Models;
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

namespace ConfigurationStation.WPF.Pages
{
    /// <summary>
    /// Interaction logic for Roms.xaml
    /// </summary>
    public partial class Roms : Page
    {
        public RomsModel Model { get; internal set; }
        public RomsController Controller { get; internal set; }
        public Roms()
        {
            Model = new RomsModel();
            Controller = new RomsController(Model);
            InitializeComponent();
        }

    }
}
