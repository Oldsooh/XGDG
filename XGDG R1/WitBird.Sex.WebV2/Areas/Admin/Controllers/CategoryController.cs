using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WitBird.Sex.BLL;
using WitBird.Sex.Model;
using WitBird.Sex.WebV2.Areas.Admin.Models;

namespace WitBird.Sex.WebV2.Areas.Admin.Controllers
{
    public class CategoryController : Controller
    {
        //
        // GET: /Admin/Category/

        public ActionResult Index()
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            CategoryModel model = new CategoryModel();

            CategoryService categoryService = new CategoryService();
            model.Categories = categoryService.GetCategories(null);

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            CategoryModel model = new CategoryModel();

            CategoryService categoryService = new CategoryService();
            model.Category = categoryService.GetCategoryById(id);

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(string id, string t, string k, string d)
        {
            if (Session["UserId"] == null)
            {
                return Redirect("/admin/login");
            }

            CategoryService categoryService = new CategoryService();
            if (!string.IsNullOrEmpty(id))
            {
                Category category = categoryService.GetCategoryById(id);
                if (category != null)
                {
                    category.Title = string.IsNullOrEmpty(t) ? string.Empty : t;
                    category.Keywords = string.IsNullOrEmpty(k) ? string.Empty : k;
                    category.Description = string.IsNullOrEmpty(d) ? string.Empty : d;
                    categoryService.EditCategory(category);
                }
            }

            return Redirect("/admin/category/");
        }

    }
}
