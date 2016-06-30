using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;

namespace BindingConsole
{
    public class PersonMap : INotifyPropertyChanged
    {
        Dictionary<int, string> persons = new Dictionary<int, string>();

        public string this[int id]
        {
            get { return persons[id]; }
            set
            {
                if (persons.ContainsKey(id) && persons[id] == value) return;
                persons[id] = value;
                NotifyPropertyChanged(Binding.IndexerName);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
