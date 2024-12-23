﻿using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using Labb3_NET24.Command;
using Labb3_NET24.Model;
using Labb3_NET24.Views.Dialogs;

namespace Labb3_NET24.ViewModel;

public class QuestionPackViewModel : ViewModelBase
{
    private readonly QuestionPack model;

    public string Name
    {
        get => model.Name;
        set
        {
            model.Name = value;
            RaisePropertyChanged();
        }
    }

    public Difficulty Difficulty
    {
        get => model.Difficulty;
        set
        {
            model.Difficulty = value;
            RaisePropertyChanged();
        }
    }

    public int TimeLimitInSeconds
    {
        get => model.TimeLimitInSeconds;
        set
        {
            model.TimeLimitInSeconds = value;
            RaisePropertyChanged();
        }
    }

    public ObservableCollection<Question> Questions { get; }

    [JsonConstructor]
    public QuestionPackViewModel()
    {
        this.model = new QuestionPack();
        Questions = new ObservableCollection<Question>(model.Questions);
    }

    public QuestionPackViewModel(QuestionPack model)
    {
        this.model = model;
        this.Questions = new ObservableCollection<Question>(model.Questions);
    }
}