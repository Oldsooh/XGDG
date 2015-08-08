using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WitBird.Sex.Model;

namespace WitBird.Sex.Web.Areas.Admin.Models
{
    public class FriendlyLinkModel
    {
        public FriendlyLink FriendlyLink { get; set; }

        public List<FriendlyLink> FriendlyLinks { get; set; }
    }
}