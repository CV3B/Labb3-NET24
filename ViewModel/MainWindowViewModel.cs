using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using Labb3_NET24.Command;
using Labb3_NET24.Model;
using Labb3_NET24.Views.Dialogs;

namespace Labb3_NET24.ViewModel;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<QuestionPackViewModel> Packs { get; set; }
    
    public PlayerViewModel PlayerViewModel { get; }
    
    public ConfigurationViewModel? ConfigurationViewModel { get; }
    
    public CommandsViewModel CommandsViewModel { get; }

    public DelegateCommand OnWindowClosingCommand;
    
    // public CreateNewPackDialogViewModel CreateNewPackDialogViewModel { get; }
    //
    // public PackOptionsDialogViewModel PackOptionsDialogViewModel { get; }

    private QuestionPackViewModel _activePack;
    public QuestionPackViewModel ActivePack
    {
        get => _activePack;
        set
        {
            _activePack = value;
            RaisePropertyChanged();
            ConfigurationViewModel?.RaisePropertyChanged();
        }
    }

    public MainWindowViewModel()
    {
 
                
        
        
        
        Packs = new ObservableCollection<QuestionPackViewModel>() {new QuestionPackViewModel(new QuestionPack("My Question Pack", Difficulty.Medium, 60))};

        LoadPacksFromJson();
        
        ActivePack = Packs.First();
        // Packs.Add(ActivePack);

        CommandsViewModel = new CommandsViewModel(this);
        
        PlayerViewModel = new PlayerViewModel(this);

        ConfigurationViewModel = new ConfigurationViewModel(this);

        // OnWindowClosingCommand = new DelegateCommand(OnWindowClosing);
        // CreateNewPackDialogViewModel = new CreateNewPackDialogViewModel(this);
        // PackOptionsDialogViewModel = new PackOptionsDialogViewModel(this);
        //ActivePack.Questions = new Question("Is dis right?", "Jaa", "no", "nej", "nain");
    }
    

    
    public void SavePacksToJson()
    {
        string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Labb3NET24");
        string filePath = Path.Combine(directoryPath, "QuestionPacks.json");
        string jsonQuestionPacks = JsonSerializer.Serialize(Packs);

        Directory.CreateDirectory(directoryPath);
        
        File.WriteAllText(filePath, jsonQuestionPacks);
        
        Console.WriteLine(jsonQuestionPacks);
    }
    
    public void LoadPacksFromJson()
    {
        string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Labb3NET24");
        string filePath = Path.Combine(directoryPath, "QuestionPacks.json");

        if (File.Exists(filePath))
        {
            string jsonQuestionPacks = File.ReadAllText(filePath);
            JsonSerializerOptions options = new JsonSerializerOptions { IncludeFields = true };

            // Deserialize JSON into a list of QuestionPack objects
            var questionPacks = JsonSerializer.Deserialize<ObservableCollection<QuestionPackViewModel>>(jsonQuestionPacks, options);

            // Clear the existing Packs collection and add the loaded data
            Packs.Clear();
            foreach (var pack in questionPacks)
            {
                
                // Packs.Add(new QuestionPackViewModel(pack));
            }
        }
    }

    public void aLoadPacksFromJson()
    {
        string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Labb3NET24");
        string filePath = Path.Combine(directoryPath, "QuestionPacks.json");

        if (File.Exists(filePath))
        { 
            JsonSerializerOptions options = new JsonSerializerOptions { IncludeFields = true };

            string jsonQuestionPacks = File.ReadAllText(filePath);
            
            var ab = JsonSerializer.Deserialize<List<QuestionPackViewModel>>(jsonQuestionPacks, options); ;
            Console.WriteLine(ab);
            
            if (ab != null)
            {
                // Clear existing packs if you want a fresh load from the file
                Packs.Clear();

                // Add each QuestionPack to Packs as a QuestionPackViewModel
                foreach (var pack in ab)
                {
                    Packs.Add((pack));
                }
            }
            
        }

        
    }
}