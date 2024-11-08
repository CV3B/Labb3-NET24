using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using Labb3_NET24.ViewModel;

namespace Labb3_NET24.Model;

public class JsonHandle
{
    public static void SavePacksToJson(object obj)
    {
        string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Labb3NET24");
        string filePath = Path.Combine(directoryPath, "QuestionPacks.json");
        string jsonQuestionPacks = JsonSerializer.Serialize(obj);

        Directory.CreateDirectory(directoryPath);

        File.WriteAllText(filePath, jsonQuestionPacks);
    }

    public static async Task<ObservableCollection<QuestionPackViewModel>> LoadPacksFromJson()
    {
        string directoryPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "Labb3NET24");
        string filePath = Path.Combine(directoryPath, "QuestionPacks.json");

        if (File.Exists(filePath))
        {
            string jsonQuestionPacks = await File.ReadAllTextAsync(filePath);
            JsonSerializerOptions options = new JsonSerializerOptions { IncludeFields = true };

            if (string.IsNullOrEmpty(jsonQuestionPacks))
            {
                return new ObservableCollection<QuestionPackViewModel>()
                    { new QuestionPackViewModel(new QuestionPack("My Question Pack")) };
            }

            try
            {
                var questionPacks = JsonSerializer.Deserialize<List<QuestionPack>>(jsonQuestionPacks, options);

                return new ObservableCollection<QuestionPackViewModel>(
                    questionPacks.Select(qp => new QuestionPackViewModel(qp))
                );
            }
            catch (JsonException ex)
            {
                return new ObservableCollection<QuestionPackViewModel>()
                    { new QuestionPackViewModel(new QuestionPack("My Question Pack")) };
            }
        }

        return new ObservableCollection<QuestionPackViewModel>()
            { new QuestionPackViewModel(new QuestionPack("My Question Pack")) };
    }
}