using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ConfigurationStation.WPF.Models
{
    public abstract class NotifyViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }
}
