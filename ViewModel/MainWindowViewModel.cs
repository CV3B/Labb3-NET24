using System.Collections.ObjectModel;
using Labb3_NET24.Model;

namespace Labb3_NET24.ViewModel;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<QuestionPackViewModel> Packs { get; set; }
    
    public PlayerViewModel PlayerViewModel { get; }
    
    public ConfigurationViewModel ConfigurationViewModel { get; }

    private QuestionPackViewModel _activePack;
    public QuestionPackViewModel ActivePack
    {
        get => _activePack;
        set
        {
            _activePack = value;
            RaisePropertyChanged();
        }
    }

    public MainWindowViewModel()
    {
        PlayerViewModel = new PlayerViewModel(this);

        ConfigurationViewModel = new ConfigurationViewModel(this);
        
        var ActivePack = new QuestionPackViewModel(new QuestionPack("My Question Pack"));

    }
}