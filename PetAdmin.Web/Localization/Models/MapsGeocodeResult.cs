﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetAdmin.Web.Localization.Models
{
    public class MapsGeocodeResult
    {
        public List<Result> results { get; set; }
        public string status { get; set; }
    }
}
