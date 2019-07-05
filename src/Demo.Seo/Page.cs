using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Demo.Seo
{
    public class Page
    {
        [BsonId(IdGenerator = typeof (CombGuidGenerator))]
        internal Guid Id { get; set; }

        /// <summary>
        ///     Contenu de la page
        /// </summary>
        [BsonElement("Text")]
        public string Text { get; set; }

        /// <summary>
        ///     Http StatusCode
        /// </summary>
        [BsonElement("StatusCode")]
        public int StatusCode { get; set; }

        /// <summary>
        ///     URL de la page
        /// </summary>
        [BsonElement("Url")]
        public string Url { get; set; }

        /// <summary>
        ///     URL de la page
        /// </summary>
        [BsonElement("SiteId")]
        public string SiteId { get; set; }

        /// <summary>
        ///     URL de la page
        /// </summary>
        [BsonElement("Date")]
        public DateTime Date { get; set; }
    }
}