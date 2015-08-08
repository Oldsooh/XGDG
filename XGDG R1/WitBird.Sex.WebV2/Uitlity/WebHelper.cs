using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Uitlity
{
    public class WebHelper
    {
        public static string GetPCPath(string url)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(WitBird.Sex.WebV2.Cache.WebConfig.DomainName))
            {
                result = "http://" + WitBird.Sex.WebV2.Cache.WebConfig.DomainName + url;
            }
            else
            {
                result = url;
            }

            return result;
        }

        public static string GetMobilePath(string url)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(WitBird.Sex.WebV2.Cache.WebConfig.DomainMobile))
            {
                result = "http://" + WitBird.Sex.WebV2.Cache.WebConfig.DomainMobile + url;
            }
            else
            {
                result = url;
            }

            return result;
        }

        public static string GetImagePath(string url)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(WitBird.Sex.WebV2.Cache.WebConfig.DomainImage))
            {
                result = "http://" + WitBird.Sex.WebV2.Cache.WebConfig.DomainImage + url;
            }
            else
            {
                result = url;
            }

            return result;
        }
    }
}