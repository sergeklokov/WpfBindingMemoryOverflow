using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBindingMemoryOverflowFixed
{

    // Per-tile model; INPC ensures safe WPF bindings (no PropertyDescriptor fallback).
    public sealed class LeakyModelFixed : INotifyPropertyChanged
    {
        private string _text = "Tile";
        private double _strokeThickness = 2.0;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Text
        {
            get { return _text; }
            set
            {
                if (_text == value) return;
                _text = value;
                RaisePropertyChanged("Text");
            }
        }

        public double StrokeThickness
        {
            get { return _strokeThickness; }
            set
            {
                if (_strokeThickness == value) return;
                _strokeThickness = value;
                RaisePropertyChanged("StrokeThickness");
            }
        }

        private void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
