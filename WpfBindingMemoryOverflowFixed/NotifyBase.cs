//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Runtime.CompilerServices;
//using System.Text;
//using System.Threading.Tasks;

//namespace WpfBindingMemoryOverflowFixed
//{
//    public abstract class NotifyBase : INotifyPropertyChanged
//    {
//        public event PropertyChangedEventHandler PropertyChanged;

//        protected void Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
//        {
//            if (Equals(storage, value)) return;
//            storage = value;
//            var handler = PropertyChanged;
//            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
//        }
//    }
//}
