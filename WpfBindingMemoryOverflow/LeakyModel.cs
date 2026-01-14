using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfBindingMemoryOverflow
{
    // Intentionally *no* INotifyPropertyChanged to make the binding leak more visible.
    public sealed class LeakyModel
    {
        public static LeakyModel Instance { get; } = new LeakyModel();

        public string Text { get; set; } = "Tile";
        public double StrokeThickness { get; set; } = 2.0;
    }
}
