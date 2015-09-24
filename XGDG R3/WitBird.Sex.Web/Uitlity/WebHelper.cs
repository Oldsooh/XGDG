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
                case "damimi":
                    name = "大咪咪美女";
                    break;
                case "dapigu":
                    name = "性感大屁股";
                    break;
                case "nenmeimei":
                    name = "清纯嫩妹妹";
                    break;
                case "meironghufu":
                    name = "美容护肤";
                    break;
                case "qingganliangxing":
                    name = "情感两性";
                    break;
                case "yulebagua":
                    name = "娱乐八卦";
                    break;
                case "jiankangyangshen":
                    name = "健康养身";
                    break;
                case "jianfeijianshen":
                    name = "减肥健身";
                    break;
                case "shishangchaoliu":
                    name = "时尚潮流";
                    break;
                default:
                    break;
            }

            return name;
        }
    }
}