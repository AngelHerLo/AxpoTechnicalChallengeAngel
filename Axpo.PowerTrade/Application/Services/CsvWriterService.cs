using Axpo.PowerTradeForecast.Application.DTOs;
using Axpo.PowerTradeForecast.Application.Interfaces;
using CsvHelper;
using CsvHelper.Configuration;
using PowerTradeApp.Application.Validators;
using System.Globalization;

namespace Axpo.PowerTradeForecast.Application.Services
{
    public class CsvWriterService : ICsvWriterService
    {
        private readonly HourlyVolumeValidator _validator;

        public CsvWriterService()
        {
            _validator = new HourlyVolumeValidator();
        }
        public void WriteToCsv(string filePath, IEnumerable<HourlyVolume> data)
        {
            foreach (var hourlyVolume in data)
            {
                var result = _validator.Validate(hourlyVolume);
                if (!result.IsValid)
                {
                    throw new FluentValidation.ValidationException($"Validation failed for HourlyVolume: {string.Join(", ", result.Errors.Select(e => e.ErrorMessage))}");
                }
            }

            EnsureDirectoryExists(filePath);
            var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";"
            };
            using var writer = new StreamWriter(filePath);
            using var csv = new CsvWriter(writer, csvConfig);

            csv.WriteHeader<HourlyVolume>();
            csv.NextRecord();
            csv.WriteRecords(data);
        }
        private static void EnsureDirectoryExists(string filePath)
        {
            string directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

    }
}