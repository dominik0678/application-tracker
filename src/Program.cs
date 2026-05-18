using System.Net;
using ApplicationTracker.Models;


Console.WriteLine("Application Tracker");

JobApplication application = new JobApplication
{
    Id = Guid.NewGuid(),
    Company = "Example AG",
    Title = "Junior Software Dev",
    Location = "Chur",
    Date = DateTime.Now,
    Status = Status.Interested,
    Contact = "Max Muster",
    Link = "https://www.example.com/jobs/xy",
    Notes = "Example application"
};

List<JobApplication> applications = [];
//Console.WriteLine(application.Date);
