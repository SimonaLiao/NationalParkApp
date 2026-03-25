using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using NationalPark.Models;

namespace NationalPark.Services
{
    // define a service to manage national park data, including how the parks are loaded when initialized, how can they be retrieved (e.g. by id, by region).
    public class NationalParkService
    {
        private static List<NationalParkModel> _nationalParks = new List<NationalParkModel>();
        private static List<VisitRecord> _visitRecords = new List<VisitRecord>();
        private static int _nextVisitId = 1;
        private static bool _isInitialized = false;
        private static readonly object _initLock = new object();

        public List<NationalParkModel> GetAllParks()
        {
            EnsureInitialized();
            
            // Update IsVisited status based on visit records
            foreach (var park in _nationalParks)
            {
                park.IsVisited = _visitRecords.Any(v => v.NationalParkId == park.Id);
                park.VisitRecords = _visitRecords.Where(v => v.NationalParkId == park.Id).ToList();
            }
            return _nationalParks;
        }

        public NationalParkModel? GetParkById(int id)
        {
            EnsureInitialized();
            
            var park = _nationalParks.FirstOrDefault(p => p.Id == id);
            if (park != null)
            {
                park.IsVisited = _visitRecords.Any(v => v.NationalParkId == park.Id);
                park.VisitRecords = _visitRecords.Where(v => v.NationalParkId == park.Id).ToList();
            }
            return park;
        }

        //get park based on which region the user specifies
        public List<NationalParkModel> GetParksByRegion(string region)
        {
            EnsureInitialized();
            
            var parks = _nationalParks.Where(p => p.Region.Equals(region, System.StringComparison.OrdinalIgnoreCase)).ToList();
            foreach (var park in parks)
            {
                park.IsVisited = _visitRecords.Any(v => v.NationalParkId == park.Id);
                park.VisitRecords = _visitRecords.Where(v => v.NationalParkId == park.Id).ToList();
            }
            return parks;
        }

        public List<NationalParkModel> GetVisitedParks()
        {
            EnsureInitialized();
            
            var parks = _nationalParks.Where(p => _visitRecords.Any(v => v.NationalParkId == p.Id)).ToList();
            foreach (var park in parks)
            {
                park.IsVisited = true;
                park.VisitRecords = _visitRecords.Where(v => v.NationalParkId == park.Id).ToList();
            }
            return parks;
        }

        public List<string> GetAllRegions()
        {
            EnsureInitialized();
            return _nationalParks.Select(p => p.Region).Distinct().OrderBy(r => r).ToList();
        }

        public void AddVisitRecord(VisitRecord visitRecord)
        {
            visitRecord.Id = _nextVisitId++;
            _visitRecords.Add(visitRecord);
        }

        private static void EnsureInitialized()
        {
            if (_isInitialized) return;

            lock (_initLock)
            {
                if (_isInitialized) return;

                try
                {
                    // Try to load from JSON file first synchronously
                    var jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "nationalparks.json");
                    
                    if (File.Exists(jsonPath))
                    {
                        var jsonContent = File.ReadAllText(jsonPath);
                        var parks = JsonSerializer.Deserialize<List<NationalParkModel>>(jsonContent, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        if (parks != null && parks.Count > 0)
                        {
                            _nationalParks = parks;
                            _isInitialized = true;
                            System.Diagnostics.Debug.WriteLine($"Successfully loaded {parks.Count} parks from JSON");
                            return;
                        }
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"JSON file not found at: {jsonPath}");
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception (in a real app, you'd use proper logging)
                    System.Diagnostics.Debug.WriteLine($"Failed to load parks from JSON: {ex.Message}");
                }

                // Fallback to hardcoded data if JSON loading fails
                System.Diagnostics.Debug.WriteLine("Using hardcoded park data as fallback");
                LoadHardcodedParks();
                _isInitialized = true;
            }
        }

        private static void LoadHardcodedParks()
        {
            _nationalParks = new List<NationalParkModel>
            {
                new NationalParkModel
                {
                    Id = 1,
                    Name = "Acadia National Park",
                    State = "Maine",
                    Description = "Acadia National Park protects the natural beauty of the highest rocky headlands along the Atlantic coastline of the United States, an abundance of habitats, and a rich cultural heritage.",
                    ImageUrl = "https://www.nps.gov/common/uploads/structured_data/3C7B477B-1DD8-B71B-0BCB48E009241BAA.jpg",
                    Region = "Northeast"
                },
                new NationalParkModel
                {
                    Id = 2,
                    Name = "Grand Canyon National Park",
                    State = "Arizona",
                    Description = "Located in Arizona, Grand Canyon National Park encompasses 277 miles of the Colorado River and adjacent uplands. One of the most spectacular examples of erosion anywhere in the world.",
                    ImageUrl = "https://www.nps.gov/common/uploads/structured_data/3C7B12D1-1DD8-B71B-0BCE0712F9CEA155.jpg",
                    Region = "Southwest"
                },
                new NationalParkModel
                {
                    Id = 3,
                    Name = "Yellowstone National Park",
                    State = "Idaho, Montana, Wyoming",
                    Description = "On March 1, 1872, Yellowstone became the first national park for all to enjoy the unique hydrothermal and geologic features. Within Yellowstone's 2.2 million acres, visitors have unparalleled opportunities to observe wildlife in an intact ecosystem.",
                    ImageUrl = "https://www.nps.gov/common/uploads/structured_data/3C7D2FBB-1DD8-B71B-0BED99731011CFCE.jpg",
                    Region = "Rocky Mountain"
                },
                new NationalParkModel
                {
                    Id = 4,
                    Name = "Yosemite National Park",
                    State = "California",
                    Description = "Not just a great valley, but a shrine to human foresight, the strength of granite, the power of glaciers, the persistence of life, and the tranquility of the High Sierra.",
                    ImageUrl = "https://www.nps.gov/common/uploads/structured_data/05383E91-AA28-2DDC-AB517507594F9FA6.jpg",
                    Region = "Pacific West"
                },
                new NationalParkModel
                {
                    Id = 5,
                    Name = "Great Smoky Mountains National Park",
                    State = "North Carolina, Tennessee",
                    Description = "Ridge upon ridge of forest straddles the border between North Carolina and Tennessee in Great Smoky Mountains National Park. World renowned for its diversity of plant and animal life and the beauty of its ancient mountains.",
                    ImageUrl = "https://www.nps.gov/common/uploads/structured_data/3C80E3F4-1DD8-B71B-0BFF4F2280EF1B52.jpg",
                    Region = "Southeast"
                }
            };
        }

        // Method to reload parks from JSON (useful for testing or future updates)
        public async Task ReloadParksAsync()
        {
            _isInitialized = false;
            await Task.Run(() => EnsureInitialized());
        }
    }
}