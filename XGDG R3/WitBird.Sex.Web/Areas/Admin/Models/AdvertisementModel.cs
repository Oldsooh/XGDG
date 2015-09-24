using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WitBird.Sex.Model;

namespace WitBird.Sex.Web.Areas.Admin.Models
{
    public class AdvertisementModel
    {
        public Advertisement Advertisement { get; set; }

        public List<Advertisement> Advertisements { get; set; }
    }
}