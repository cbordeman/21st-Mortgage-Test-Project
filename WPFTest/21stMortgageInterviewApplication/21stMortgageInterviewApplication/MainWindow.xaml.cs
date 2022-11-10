namespace _21stMortgageInterviewApplication;

using System.ComponentModel;
using System.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        if (!DesignerProperties.GetIsInDesignMode(this))
            DataContext = new MainWindowViewModel();

        Loaded += (sender, args) => InputBox.Focus();
    }
}
