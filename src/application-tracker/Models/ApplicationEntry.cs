using System;
namespace application_tracker.Models;

public class ApplicationEntry : application_tracker.ViewModels.ViewModelBase
{
    private string _company = "";
    private string _position = "";
    private string _contactPerson = "";
    private string _status = "";
    private string _link = "";
    private string _notes = "";
    private DateTimeOffset? _applicationDate;

    public string Company
    {
        get => _company;
        set
        {
            _company = value;
            OnPropertyChanged();
        }
    }

    public string Position
    {
        get => _position;
        set
        {
            _position = value;
            OnPropertyChanged();
        }
    }

    public string ContactPerson
    {
        get => _contactPerson;
        set
        {
            _contactPerson = value;
            OnPropertyChanged();
        }
    }

    public string Status
    {
        get => _status;
        set
        {
            _status = value;
            OnPropertyChanged();
        }
    }

    public string Link
    {
        get => _link;
        set
        {
            _link = value;
            OnPropertyChanged();
        }
    }

    public string Notes
    {
        get => _notes;
        set
        {
            _notes = value;
            OnPropertyChanged();
        }
    }

    public DateTimeOffset? ApplicationDate
    {
        get => _applicationDate;
        set
        {
            _applicationDate = value;
            OnPropertyChanged();
        }
    }
}