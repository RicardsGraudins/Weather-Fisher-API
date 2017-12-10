using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WorldTidesForecast.Data
{
    public class WorldTidesExtremes
    {
        [DataContract]
        public class Extreme
        {
            [DataMember]
            public int dt { get; set; }

            [DataMember]
            public string date { get; set; }

            [DataMember]
            public double height { get; set; }

            [DataMember]
            public string type { get; set; }
        }

        [DataContract]
        public class RootObjectExtreme
        {
            [DataMember]
            public int status { get; set; }

            [DataMember]
            public int callCount { get; set; }

            [DataMember]
            public string copyright { get; set; }

            [DataMember]
            public double requestLat { get; set; }

            [DataMember]
            public double requestLon { get; set; }

            [DataMember]
            public double responseLat { get; set; }

            [DataMember]
            public double responseLon { get; set; }

            [DataMember]
            public string atlas { get; set; }

            [DataMember]
            public string station { get; set; }

            [DataMember]
            public string county { get; set; }

            [DataMember]
            public List<Extreme> extremes { get; set; }
        }//RootObjectExtreme
    }//WorldTidesExtremes
}//WorldTidesForecast