WpfBindingMemoryOverflow
Overview
WpfBindingMemoryOverflow is a WPF (.NET Framework 4.7.2) demo application designed to illustrate a common memory leak scenario in WPF when using data bindings incorrectly. The app repeatedly adds and removes UI elements with bindings that are intentionally unsafe, causing memory usage to grow over time.


How It Works
Main Concept
The application creates and destroys many small UI tiles (LeakyTile) inside a UniformGrid on a timer. Each tile contains:

A Button bound to a property on a singleton model (LeakyModel.Text).
A Label bound to UniformGrid.Children.Count (a CLR property without proper notifications).
A Path bound to another property on the singleton model (LeakyModel.StrokeThickness).
These bindings are intentionally set up to use sources that do not implement INotifyPropertyChanged or Dependency Properties. WPF falls back to PropertyDescriptor subscriptions, which can create strong reference chains and prevent garbage collection of removed elements.
Application Flow

Start Leak button starts a DispatcherTimer.
The timer alternates between two phases: Add Phase: Adds BatchSize (default 300) LeakyTile controls to the UniformGrid.
Remove Phase: Clears all tiles from the grid and forces garbage collection.
Memory usage is displayed in the UI and grows over time, demonstrating the leak.


Why It Leaks

Binding to non-DP/non-INPC properties (e.g., Children.Count and LeakyModel.Text) causes WPF to use PropertyDescriptor.AddValueChanged internally.
These subscriptions can keep references alive even after the controls are removed from the visual tree.
Over multiple add/remove cycles, memory usage ratchets upward.


How to Run

Open the solution in Visual Studio.
Ensure the project targets .NET Framework 4.7.2.
Build and run the application.
Click Start Leak and watch memory usage increase in the UI or via Task Manager.


Observing the Leak

Use Visual Studio Diagnostic Tools or a memory profiler (e.g., dotMemory) to take snapshots.
You will see surviving instances of LeakyTile and related binding objects retained by PropertyDescriptor chains.


Fixing the Leak (Best Practices)

Implement INotifyPropertyChanged on bound models.
Use Mode="OneTime" for bindings that do not need updates.
Avoid binding to CLR properties on other UI elements.
Explicitly clear bindings on teardown:
BindingOperations.ClearAllBindings(myControl);




Purpose
This app is for educational and diagnostic purposes only. It demonstrates how subtle binding patterns can lead to memory leaks in WPF and how to identify and mitigate them in real-world applications.
