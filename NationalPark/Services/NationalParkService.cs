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
                    Name = "Yellowstone National Park",
                    State = "Wyoming, Montana, Idaho",
                    Description = "Yellowstone National Park is a nearly 3,500-sq.-mile wilderness recreation area atop a volcanic hotspot. Mostly in Wyoming, the park spreads into parts of Montana and Idaho too. Yellowstone features dramatic canyons, alpine rivers, lush forests, hot springs and gushing geysers – including its most famous, Old Faithful.",
                    ImageUrl = "https://www.nps.gov/common/uploads/structured_data/3C7D2FBB-1DD8-B71B-0BED99731011CFCE.jpg",
                    EstablishedDate = new System.DateTime(1872, 3, 1),
                    AreaInAcres = 2219791,
                    Region = "Western USA"
                },
                new NationalParkModel
                {
                    Id = 2,
                    Name = "Grand Canyon National Park",
                    State = "Arizona",
                    Description = "Located in Arizona, Grand Canyon National Park encompasses 277 river miles of the Colorado River and adjacent uplands. The park is home to much of the immense Grand Canyon; a mile (1.6 km) deep, and up to 18 miles (29 km) wide.",
                    ImageUrl = "https://www.nps.gov/common/uploads/structured_data/3C7F2DD5-1DD8-B71B-0BAA2DDC5F83B37C.jpg",
                    EstablishedDate = new System.DateTime(1919, 2, 26),
                    AreaInAcres = 1217262,
                    Region = "Western USA"
                },
                new NationalParkModel
                {
                    Id = 3,
                    Name = "Yosemite National Park",
                    State = "California",
                    Description = "Yosemite National Park is in California's Sierra Nevada mountains. It's famed for its giant, ancient sequoia trees, and for Tunnel View, the iconic vista of towering Bridalveil Fall and the granite cliffs of El Capitan and Half Dome.",
                    ImageUrl = "https://www.nps.gov/common/uploads/structured_data/3C84C3C0-1DD8-B71B-0BFF90B64283C3D8.jpg",
                    EstablishedDate = new System.DateTime(1890, 10, 1),
                    AreaInAcres = 761747,
                    Region = "Western USA"
                },
                new NationalParkModel
                {
                    Id = 4,
                    Name = "Great Smoky Mountains National Park",
                    State = "Tennessee, North Carolina",
                    Description = "Great Smoky Mountains National Park straddles the border between North Carolina and Tennessee. The park encompasses 816 square miles of pristine wilderness area and is home to a diverse population of plants and animals.",
                    ImageUrl = "https://www.nps.gov/common/uploads/structured_data/3C80C2AA-1DD8-B71B-0BFB8EA8EDD5ADF3.jpg",
                    EstablishedDate = new System.DateTime(1934, 6, 15),
                    AreaInAcres = 522427,
                    Region = "Eastern USA"
                },
                new NationalParkModel
                {
                    Id = 5,
                    Name = "Zion National Park",
                    State = "Utah",
                    Description = "Zion National Park is a southwest Utah desert dominated by the Zion Canyon's steep red cliffs. The canyon is a mix of white, pink, and red Navajo sandstone cliffs, with forested plateau tops and numerous canyons and tributaries.",
                    ImageUrl = "https://www.nps.gov/common/uploads/structured_data/3C7F0E7E-1DD8-B71B-0B5EA38821F4F752.jpg",
                    EstablishedDate = new System.DateTime(1919, 11, 19),
                    AreaInAcres = 147242,
                    Region = "Western USA"
                },
                new NationalParkModel
                {
                    Id = 6,
                    Name = "Rocky Mountain National Park",
                    State = "Colorado",
                    Description = "Rocky Mountain National Park encompasses 415 square miles of pristine wilderness areas in the Colorado Rockies. With elevations from 7,500 to over 12,000 feet, the park has spectacular mountain environments.",
                    ImageUrl = "https://www.nps.gov/common/uploads/structured_data/3C7FEAD2-1DD8-B71B-0B4CB7141F67B763.jpg",
                    EstablishedDate = new System.DateTime(1915, 1, 26),
                    AreaInAcres = 265461,
                    Region = "Western USA"
                },
                new NationalParkModel
                {
                    Id = 7,
                    Name = "Arches National Park",
                    State = "Utah",
                    Description = "Arches National Park lies north of Moab in the state of Utah. Bordered by the Colorado River in the southeast, it's known for its multitude of natural sandstone arches. The park contains more than 2,000 arches, at least 3 feet (0.91 m) wide.",
                    ImageUrl = "https://www.nps.gov/common/uploads/structured_data/3C79836C-1DD8-B71B-0BC4A88BA85DE6B0.jpg",
                    EstablishedDate = new System.DateTime(1971, 11, 12),
                    AreaInAcres = 76519,
                    Region = "Western USA"
                },
                new NationalParkModel
                {
                    Id = 8,
                    Name = "Acadia National Park",
                    State = "Maine",
                    Description = "Acadia National Park is located in the U.S. state of Maine, southwest of Bar Harbor. The park preserves about half of Mount Desert Island, many adjacent smaller islands, and part of the Schoodic Peninsula on the coast of Maine.",
                    ImageUrl = "https://www.nps.gov/common/uploads/structured_data/3C7C0089-1DD8-B71B-0B0A5CF98361C3B4.jpg",
                    EstablishedDate = new System.DateTime(1916, 8, 25),
                    AreaInAcres = 47390,
                    Region = "Eastern USA"
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