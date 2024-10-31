using System.Windows.Threading;
using Labb3_NET24.Command;

namespace Labb3_NET24.ViewModel;

public class PlayerViewModel : ViewModelBase
{
    private readonly MainWindowViewModel mainWindowViewModel;

    public DelegateCommand UpdateButtonCommand { get; }

    private DispatcherTimer timer;

    private string _TestData;
    public string TestData
    {
        get => _TestData;
        set
        {
            _TestData = value;
            RaisePropertyChanged();
        }
    }

    public PlayerViewModel(MainWindowViewModel mainWindowViewModel)
    {
        this.mainWindowViewModel = mainWindowViewModel;

        TestData = "Start value: ";
        
        timer = new DispatcherTimer();
        timer.Interval = TimeSpan.FromSeconds(1);
        timer.Tick += TimerOnTick;
        // timer.Start();

        UpdateButtonCommand = new DelegateCommand(UpdateButton);
    }

    private void UpdateButton(object obj)
    {
        TestData += "x";
    }

    private void TimerOnTick(object? sender, EventArgs e)
    {
        TestData += "x";
    }
}