using System.Web.Mvc;

namespace WitBird.Sex.Web.Areas.T
{
    public class TAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "T";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "T_default",
                "T/{controller}/{action}/{id}",
                new { controller = "Index", action = "Index", id = UrlParameter.Optional },
                constraints: null,
                namespaces: new string[] { "WitBird.Sex.Web.Areas.T.Controllers" }
            );
        }
    }
}
