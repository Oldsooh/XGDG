using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WitBird.Sex.WebV2.Areas.Admin.Content.swfupload
{
    /// <summary>
    /// upload 的摘要说明
    /// </summary>
    public class upload : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");

            try
            {
                HttpPostedFile file;
                for (int i = 0; i < context.Request.Files.Count; ++i)
                {
                    file = context.Request.Files[i];
                    if (file == null || file.ContentLength == 0 || string.IsNullOrEmpty(file.FileName)) continue;
                    //string filename = Path.GetFileName(file.FileName);
                    DateTime time = DateTime.Now;
                    string year = time.ToString("yyyy");
                    string month = time.ToString("MM");
                    string day = time.ToString("dd");
                    string fileUrl = "/uploadfiles/group/" + year + "/" + month + "/" + day + "/";
                    string filename = time.ToString("yyyyMMddHHmmssfff");
                    string filetrype = System.IO.Path.GetExtension(file.FileName);
                    string serverPath = context.Server.MapPath(fileUrl);
                    //如果不存在path目录
                    if (!Directory.Exists(serverPath))
                    {
                        //那么就创建它
                        Directory.CreateDirectory(serverPath);
                    }
                    file.SaveAs(HttpContext.Current.Server.MapPath(fileUrl + filename + filetrype));
                    context.Response.Write(fileUrl + filename + filetrype);
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.Write(ex.Message);
                context.Response.End();
            }
            finally
            {
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}