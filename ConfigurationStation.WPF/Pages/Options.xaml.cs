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
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Page
    {
        public OptionsModel Model { get; set; }
        public OptionsController Controller { get; set; }
        public Options()
        {
            Model = new OptionsModel();
            Controller = new OptionsController(Model);
            InitializeComponent();
        }
    }
}
