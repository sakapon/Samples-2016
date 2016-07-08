using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace BindingConsole
{
    public class PersonMap : INotifyPropertyChanged
    {
        Dictionary<int, string> People = new Dictionary<int, string>();

        public string this[int id]
        {
            get { return People[id]; }
            set
            {
                if (People.ContainsKey(id) && People[id] == value) return;
                People[id] = value;
                NotifyPropertyChanged(Binding.IndexerName);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName]string propertyName = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
