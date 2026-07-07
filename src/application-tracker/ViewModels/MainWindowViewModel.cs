using System.Collections.ObjectModel;
using application_tracker.Models;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using Microsoft.VisualBasic;
using System.Collections.Generic;

namespace application_tracker.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<ApplicationEntry> Applications { get; } = [];

    public string[] StatusOptions { get; } =
    [
        "Offen",
        "Beworben",
        "Gespräch",
        "Absage",
        "Wartet auf Rückmeldung"
    ];

    public ApplicationEntry? _selectedApplication;

    public ApplicationEntry? SelectedApplication
    {
        get => _selectedApplication;
        set
        {
            if (_selectedApplication == value)
            {
                return;
            }

            _selectedApplication = value;
            OnPropertyChanged();
        }
    }

    private void LoadApplications()
    {
        var path = @"jsons/applications.json";

        if (!File.Exists(path))
        {
            return;
        }

        var json = File.ReadAllText(path);

        if (string.IsNullOrWhiteSpace(json))
        {
            return;
        }

        var loadedApplications = JsonSerializer.Deserialize<List<ApplicationEntry>>(json);

        if (loadedApplications is null)
        {
            return;
        }

        Applications.Clear();

        foreach (var application in loadedApplications)
        {
            Applications.Add(application);
        }
    }

    public MainWindowViewModel()
    {
        LoadApplications();

        SelectedApplication = Applications.Count > 0 ? Applications[0] : null;
    }
}