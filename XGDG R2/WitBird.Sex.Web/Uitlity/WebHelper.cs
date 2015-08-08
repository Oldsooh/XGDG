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

            if (!string.IsNullOrEmpty(WitBird.Sex.Web.Cache.WebConfig.DomainName))
            {
                result = "http://" + WitBird.Sex.Web.Cache.WebConfig.DomainName + url;
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

            if (!string.IsNullOrEmpty(WitBird.Sex.Web.Cache.WebConfig.DomainMobile))
            {
                result = "http://" + WitBird.Sex.Web.Cache.WebConfig.DomainMobile + url;
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

            if (!string.IsNullOrEmpty(WitBird.Sex.Web.Cache.WebConfig.DomainImage))
            {
                result = "http://" + WitBird.Sex.Web.Cache.WebConfig.DomainImage + url;
            }
            else
            {
                result = url;
            }

            return result;
        }

        public static string GetCatogoryNameById(string id)
        {
            string name = string.Empty;

            id = id.ToLower();
            switch (id)
            {
                case "news":
                    name = "最新资讯";
                    break;
                case "pure":
                    name = "清纯校花";
                    break;
                case "sexy":
                    name = "性感尤物";
                    break;
                case "star":
                    name = "时尚明星";
                    break;
                case "costume":
                    name = "古韵若烟";
                    break;
                case "stylishman":
                    name = "潮流型男";
                    break;
                default:
                    break;
            }

            return name;
        }
    }
}