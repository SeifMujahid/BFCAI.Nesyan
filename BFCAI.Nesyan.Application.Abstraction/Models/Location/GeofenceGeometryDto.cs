using System.Collections.Generic;

namespace BFCAI.Nesyan.Application.Abstraction.Models.Location
{
    public class CenterPoint
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }

    public class GeofenceGeometryDto
    {
        public CenterPoint? Center { get; set; }
        public double? Radius { get; set; }
        public List<CenterPoint>? Points { get; set; }
    }
}
