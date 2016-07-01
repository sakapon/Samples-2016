using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;

namespace BindingConsole
{
    public class PersonMap : INotifyPropertyChanged
    {
        Dictionary<int, string> Persons = new Dictionary<int, string>();

        public string this[int id]
        {
            get { return Persons[id]; }
            set
            {
                if (Persons.ContainsKey(id) && Persons[id] == value) return;
                Persons[id] = value;
                NotifyPropertyChanged(Binding.IndexerName);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
