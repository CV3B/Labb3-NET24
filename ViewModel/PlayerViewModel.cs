namespace Labb3_NET24.ViewModel;

public class PlayerViewModel : ViewModelBase
{
    private readonly MainWindowViewModel mainWindowViewModel;

    public PlayerViewModel(MainWindowViewModel mainWindowViewModel)
    {
        this.mainWindowViewModel = mainWindowViewModel;
    }
}