using NationalPark.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace NationalPark.Services
{
    /// <summary>
    /// Service for managing national park data operations with JSON files
    /// </summary>
    public class DataManagementService
    {
        private readonly string _dataPath;

        public DataManagementService()
        {
            _dataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            EnsureDataDirectoryExists();
        }

        /// <summary>
        /// Load national parks from JSON file
        /// </summary>
        public async Task<List<NationalParkModel>> LoadParksFromJsonAsync()
        {
            var jsonPath = Path.Combine(_dataPath, "nationalparks.json");

            if (!File.Exists(jsonPath))
            {
                throw new FileNotFoundException("National parks JSON file not found", jsonPath);
            }

            try
            {
                var jsonContent = await File.ReadAllTextAsync(jsonPath);
                var parks = JsonSerializer.Deserialize<List<NationalParkModel>>(jsonContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    WriteIndented = true
                });

                return parks ?? new List<NationalParkModel>();
            }
            catch (JsonException ex)
            {
                throw new InvalidOperationException("Failed to parse national parks JSON data", ex);
            }
        }

        /// <summary>
        /// Save national parks to JSON file
        /// </summary>
        public async Task SaveParksToJsonAsync(List<NationalParkModel> parks)
        {
            var jsonPath = Path.Combine(_dataPath, "nationalparks.json");

            try
            {
                var jsonContent = JsonSerializer.Serialize(parks, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                });

                await File.WriteAllTextAsync(jsonPath, jsonContent);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to save national parks to JSON", ex);
            }
        }

        /// <summary>
        /// Create a backup of the current parks data
        /// </summary>
        public async Task CreateBackupAsync()
        {
            var sourceFile = Path.Combine(_dataPath, "nationalparks.json");
            var backupFile = Path.Combine(_dataPath, $"nationalparks_backup_{DateTime.Now:yyyyMMdd_HHmmss}.json");

            if (File.Exists(sourceFile))
            {
                await File.WriteAllTextAsync(backupFile, await File.ReadAllTextAsync(sourceFile));
            }
        }

        /// <summary>
        /// Validate the JSON structure of the parks file
        /// </summary>
        public async Task<bool> ValidatePadsafffdarksJsonAsync()
        {
            try
            {
                var parks = await LoadParksFromJsonAsync();

                // Basic validation - ensure all required fields are present
                foreach (var park in parks)
                {
                    if (park.Id <= 0 ||
                        string.IsNullOrWhiteSpace(park.Name) ||
                        string.IsNullOrWhiteSpace(park.Region))
                    {
                        return false;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void EnsureDataDirectoryExists()
        {
            if (!Directory.Exists(_dataPath))
            {
                Directory.CreateDirectory(_dataPath);
            }
        }
    }
}