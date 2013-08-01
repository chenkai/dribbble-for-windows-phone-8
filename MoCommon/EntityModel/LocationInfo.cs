using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoCommon.EntityModel
{
    public class LocationInfo
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class LocationCityRepInfo
    {
        public string ResultCode { get; set; }
        public string ResultDescription { get; set; }
        public string ResultsTotal { get; set; }
        public List<Place> Places { get; set; }

    }

    public class Place
    {
        public string Title { get; set; }
        public string Language { get; set; }
        public Location Location { get; set; }
        public Address Address { get; set; }
    }

    public class Location
    {
        public LocationInfo Position { get; set; }
    }

    public class Address
    {
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string City { get; set; }

        public string District { get; set; }
        public Thoroghfare Thoroghfare { get; set; }
    }

    public class Thoroghfare
    {
        public string Name { get; set; }
    }


}
