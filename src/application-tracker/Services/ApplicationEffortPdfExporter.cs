using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using application_tracker.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace application_tracker.Services;

public static class ApplicationEffortPdfExporter
{
    public static string Export(IEnumerable<ApplicationEntry> applications)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        Directory.CreateDirectory("exports");

        var filePath = Path.Combine(
            "exports",
            $"arbeitsbemuehungen-{DateTime.Now:yyyy-MM-dd-HHmm}.pdf");

        var entries = applications.ToList();

        Document.Create(container =>
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4.Landscape());
                page.Margin(25);
                page.DefaultTextStyle(x => x.FontSize(8.5f).FontFamily("Arial"));

                page.Header().Column(column =>
                {
                    column.Item().Text("Nachweis der Arbeitsbemühungen")
                        .FontSize(18)
                        .SemiBold()
                        .FontColor("#1F2937");

                    column.Item().Text("Dominik Waldburger - Kantonsstrasse 14, 7212 Seewis-Pardisla")
                        .FontSize(10)
                        .FontColor("#4B5563");

                    column.Item().Text($"Erstellt am: {DateTime.Now:dd.MM.yyyy}")
                        .FontSize(10)
                        .FontColor("#4B5563");
                });

                page.Content().PaddingTop(14).Column(column =>
                {
                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.ConstantColumn(58);
                            columns.RelativeColumn(1.4f);
                            columns.RelativeColumn(2.0f);
                            columns.RelativeColumn(1.5f);
                            columns.RelativeColumn(1.4f);
                            columns.RelativeColumn(1.2f);
                            columns.RelativeColumn(2.4f);
                        });

                        table.Header(header =>
                        {
                            Header(header.Cell(), "Datum");
                            Header(header.Cell(), "Firma");
                            Header(header.Cell(), "Stelle / Ziel");
                            Header(header.Cell(), "Art");
                            Header(header.Cell(), "Kontakt");
                            Header(header.Cell(), "Status");
                            Header(header.Cell(), "Bemerkung");
                        });

                        foreach (var app in entries)
                        {
                            Cell(table.Cell(), app.ApplicationDate?.ToString("dd.MM.yyyy") ?? "");
                            Cell(table.Cell(), app.Company);
                            Cell(table.Cell(), app.Position);
                            Cell(table.Cell(), GetEffortType(app));
                            Cell(table.Cell(), string.IsNullOrWhiteSpace(app.ContactPerson) ? "-" : app.ContactPerson);
                            Cell(table.Cell(), ToRavStatus(app.Status));
                            Cell(table.Cell(), app.Notes);
                        }
                    });

                    column.Item().PaddingTop(12).Text(
                        "Hinweis: Die oben aufgeführten Einträge dokumentieren die unternommenen Arbeitsbemühungen als Bewerbungen. Weitere Bemühungen (Gespräche mit Vermittlern / Privatpersonen etc.) werden in einem separaten Dokument aufgeführt.")
                        .FontSize(9)
                        .FontColor("#4B5563");
                });

                page.Footer().AlignRight().Text(text =>
                {
                    text.Span("Seite ");
                    text.CurrentPageNumber();
                });
            });
        }).GeneratePdf(filePath);

        return Path.GetFullPath(filePath);
    }

    private static void Header(IContainer container, string text)
    {
        container
            .Background("#334155")
            .Border(1)
            .BorderColor("#CBD5E1")
            .Padding(5)
            .AlignCenter()
            .Text(text)
            .FontColor(Colors.White)
            .SemiBold();
    }

    private static void Cell(IContainer container, string? text)
    {
        container
            .Border(1)
            .BorderColor("#CBD5E1")
            .Padding(5)
            .Text(text ?? "");
    }

    private static string GetEffortType(ApplicationEntry app)
    {
        var company = app.Company.ToLowerInvariant();
        var position = app.Position.ToLowerInvariant();
        var notes = app.Notes.ToLowerInvariant();

        if (company.Contains("rocken"))
            return "Bewerbung über Stellenplattform";

        if (company.Contains("imt"))
            return "Abklärung über Personalvermittler";

        if (position.Contains("initiativ"))
            return "Initiativbewerbung";

        if (position.Contains("anfrage") || notes.Contains("anfrage"))
            return "Anfrage per E-Mail";

        return "Bewerbung";
    }

    private static string ToRavStatus(string status)
    {
        return status switch
        {
            "Absage" => "Absage erhalten",
            "Offen" => "Offen",
            "Beworben" => "Beworben",
            "Wartet auf Rückmeldung" => "Wartet auf Rückmeldung",
            _ => status
        };
    }
}