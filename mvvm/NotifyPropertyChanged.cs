using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace mvvm
{
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string param = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(param));
    }
}
