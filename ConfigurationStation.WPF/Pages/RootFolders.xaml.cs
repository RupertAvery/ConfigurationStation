using ConfigurationStation.WPF.Controllers;
using ConfigurationStation.WPF.Models;
using System.Windows.Controls;

namespace ConfigurationStation.WPF.Pages
{
    /// <summary>
    /// Interaction logic for RootFolders.xaml
    /// </summary>
    public partial class RootFolders : Page
    {
        public RootFoldersModel Model { get; set; }
        public RootFolderController Controller { get; set; }

        public RootFolders()
        {
            Model = new RootFoldersModel();
            Controller = new RootFolderController(Model);
            Controller.FindResource = FindResource;
            InitializeComponent();
        }

    }
}
