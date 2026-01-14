using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace WpfBindingMemoryOverflowFixed
{
    public partial class MainWindowFixed : Window, INotifyPropertyChanged
    {
        private readonly DispatcherTimer _timer;
        private bool _addingPhase = true;
        private int _cycle = 0;
        private const int BatchSize = 300; // tiles per add phase
        private static readonly TimeSpan PhaseDelay = TimeSpan.FromMilliseconds(250);

        private int _tilesCount;
        public int TilesCount
        {
            get { return _tilesCount; }
            private set
            {
                if (_tilesCount == value) return;
                _tilesCount = value;
                RaisePropertyChanged("TilesCount");
            }
        }

        public ObservableCollection<LeakyModelFixed> Tiles { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindowFixed()
        {
            InitializeComponent();

            // Window is its own DataContext
            DataContext = this;

            Tiles = new ObservableCollection<LeakyModelFixed>();
            Tiles.CollectionChanged += Tiles_CollectionChanged;

            _timer = new DispatcherTimer { Interval = PhaseDelay };
            _timer.Tick += Timer_Tick;

            UpdateInfo();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            _timer.Start();
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
        }

        private void Gc_Click(object sender, RoutedEventArgs e)
        {
            ForceGC();
            UpdateInfo();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_addingPhase)
            {
                for (int i = 0; i < BatchSize; i++)
                    Tiles.Add(new LeakyModelFixed());
            }
            else
            {
                Tiles.Clear();
                ForceGC();
                _cycle++;
            }

            _addingPhase = !_addingPhase;
            UpdateInfo();
        }

        private void Tiles_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            TilesCount = Tiles.Count;
        }

        private void UpdateInfo()
        {
            var mb = Process.GetCurrentProcess().PrivateMemorySize64 / (1024.0 * 1024.0);
            InfoText.Text = $"Cycle: {_cycle}, Tiles: {TilesCount}, Private Bytes ~ {mb:F1} MB";
        }

        private static void ForceGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
