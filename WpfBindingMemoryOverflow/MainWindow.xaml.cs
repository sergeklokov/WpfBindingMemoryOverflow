
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace WpfBindingMemoryOverflow
{
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer _timer;
        private bool _addingPhase = true;
        private int _cycle = 0;
        private const int BatchSize = 300;
        private static readonly TimeSpan PhaseDelay = TimeSpan.FromMilliseconds(250);

        public MainWindow()
        {
            InitializeComponent();
            _timer = new DispatcherTimer { Interval = PhaseDelay };
            _timer.Tick += Timer_Tick;
        }

        private void StartLeak_Click(object sender, RoutedEventArgs e) => _timer.Start();
        private void Stop_Click(object sender, RoutedEventArgs e) => _timer.Stop();
        private void Gc_Click(object sender, RoutedEventArgs e) { ForceGC(); UpdateInfo(); }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_addingPhase)
            {
                for (int i = 0; i < BatchSize; i++)
                    TilesHost.Children.Add(new LeakyTile());
            }
            else
            {
                TilesHost.Children.Clear();
                ForceGC();
                _cycle++;
            }

            _addingPhase = !_addingPhase;
            UpdateInfo();
        }

        private void UpdateInfo()
        {
            var mb = Process.GetCurrentProcess().PrivateMemorySize64 / (1024.0 * 1024.0);
            InfoText.Text = $"Cycle: {_cycle}, Tiles: {TilesHost.Children.Count}, Memory: {mb:F1} MB";
        }

        private static void ForceGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
