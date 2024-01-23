using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetAdmin.Web.Localization.Models
{
    public class Geometry
    {
        public Bounds bounds { get; set; }
        public LocationGeometry location { get; set; }
        public string location_type { get; set; }
        public Viewport viewport { get; set; }
    }
}
