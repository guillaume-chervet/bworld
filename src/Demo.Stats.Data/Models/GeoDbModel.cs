using MongoDB.Bson.Serialization.Attributes;

namespace Demo.Data.Stat.Models
{
    public class GeoDbModel
    {
        [BsonElement("As")]
        public string As { get; set; }

        [BsonElement("City")]
        public string City { get; set; }

        [BsonElement("Country")]
        public string Country { get; set; }

        [BsonElement("CountryCode")]
        public string CountryCode { get; set; }

        [BsonElement("Isp")]
        public string Isp { get; set; }

        [BsonElement("Lat")]
        public double Lat { get; set; }

        [BsonElement("Lon")]
        public double Lon { get; set; }

        [BsonElement("Org")]
        public string Org { get; set; }

        [BsonElement("Query")]
        public string Query { get; set; }

        [BsonElement("Region")]
        public string Region { get; set; }

        [BsonElement("RegionName")]
        public string RegionName { get; set; }

        [BsonElement("Status")]
        public string Status { get; set; }

        [BsonElement("Timezone")]
        public string Timezone { get; set; }

        [BsonElement("Zip")]
        public string Zip { get; set; }
    }
}