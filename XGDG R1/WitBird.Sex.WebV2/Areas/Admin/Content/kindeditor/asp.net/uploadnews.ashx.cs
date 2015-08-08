using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using LitJson;

namespace WitBird.Sex.WebV2.Areas.Admin.Content.kindeditor.asp.net
{
    public class uploadnews : IHttpHandler
    {
        private HttpContext context;

        public void ProcessRequest(HttpContext context)
        {
            String aspxUrl = context.Request.Path.Substring(0, context.Request.Path.LastIndexOf("/") + 1);

            //文件保存目录路径/uploadfiles/content/
            String savePath = "/uploadfiles/";

            //文件保存目录URL
            String saveUrl = "/uploadfiles/";

            //定义允许上传的文件扩展名
            Hashtable extTable = new Hashtable();
            extTable.Add("content", "gif,jpg,jpeg,png,bmp");
            extTable.Add("flash", "swf,flv");
            extTable.Add("media", "swf,flv,mp3,wav,wma,wmv,mid,avi,mpg,asf,rm,rmvb");
            extTable.Add("file", "doc,docx,xls,xlsx,ppt,htm,html,txt,zip,rar,gz,bz2");

            //最大文件大小
            int maxSize = 1000000;
            this.context = context;

            HttpPostedFile imgFile = context.Request.Files["imgFile"];
            if (imgFile == null)
            {
                showError("请选择文件。");
            }

            String dirPath = context.Server.MapPath(savePath);
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            String dirName = "content";
            if (!extTable.ContainsKey(dirName))
            {
                //showError("目录名不正确。");
            }

            String fileName = imgFile.FileName;
            String fileExt = Path.GetExtension(fileName).ToLower();

            if (imgFile.InputStream == null || imgFile.InputStream.Length > maxSize)
            {
                showError("上传文件大小超过限制。");
            }

            if (String.IsNullOrEmpty(fileExt) ||
                Array.IndexOf(((String)extTable[dirName]).Split(','),
                fileExt.Substring(1).ToLower()) == -1)
            {
                showError("上传文件扩展名是不允许的扩展名。\n只允许" + ((String)extTable[dirName]) + "格式。");
            }

            //创建文件夹
            dirPath += dirName + "\\" + DateTime.Now.ToString("yyyy") + "\\";
            saveUrl += dirName + "/" + DateTime.Now.ToString("yyyy");
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }
            String mm = DateTime.Now.ToString("MM", DateTimeFormatInfo.InvariantInfo);
            String dd = DateTime.Now.ToString("dd", DateTimeFormatInfo.InvariantInfo);
            dirPath += mm + "\\" + dd + "\\";
            saveUrl += "/" + mm + "/" + dd + "/";
            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            String newFileName = DateTime.Now.ToString("yyyyMMddHHmmssffff", DateTimeFormatInfo.InvariantInfo) + fileExt;
            String filePath = dirPath + newFileName;

            imgFile.SaveAs(filePath);

            String fileUrl = saveUrl + newFileName;

            Hashtable hash = new Hashtable();
            hash["error"] = 0;
            hash["url"] = fileUrl;
            context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
            context.Response.Write(JsonMapper.ToJson(hash));
            context.Response.End();
        }

        private void showError(string message)
        {
            Hashtable hash = new Hashtable();
            hash["error"] = 1;
            hash["message"] = message;
            context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
            context.Response.Write(JsonMapper.ToJson(hash));
            context.Response.End();
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}