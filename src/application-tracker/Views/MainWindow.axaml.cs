using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia;

using application_tracker.ViewModels;
using application_tracker.Services;
using application_tracker.Models;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;

namespace application_tracker.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

    }

    private void NewApplication_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not MainWindowViewModel viewModel)
        {
            return;
        }

        var newApplication = new ApplicationEntry
        {
            Company = "",
            Position = "",
            ContactPerson = "",
            Status = "Offen",
            Link = "",
            Notes = "",
            ApplicationDate = DateTimeOffset.Now.Date
        };

        viewModel.Applications.Add(newApplication);
        viewModel.SelectedApplication = newApplication;
    }

    private void Save_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not MainWindowViewModel viewModel)
        {
            return;
        }

        var data = viewModel.Applications;
        string json = JsonSerializer.Serialize(data);
        if (File.ReadAllText(@"jsons/applications.json") != null)
        {
            File.Delete(@"jsons/applications.json");
        }
        File.WriteAllText(@"jsons/applications.json", json);
    }

    private void Delete_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not MainWindowViewModel viewModel)
        {
            return;
        }

        var appl = viewModel.SelectedApplication;
        if (appl != null)
        {
            viewModel.Applications.Remove(appl);
        } 
    }

    private void ExportPdf_Click(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not MainWindowViewModel viewModel)
        {
            return;
        }

        var path = ApplicationEffortPdfExporter.Export(viewModel.Applications);

        Console.WriteLine($"PDF erstellt: {path}");
    }
}