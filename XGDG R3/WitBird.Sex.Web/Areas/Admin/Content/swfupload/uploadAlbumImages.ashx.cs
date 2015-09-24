using WitBird.Sex.Web.Uitlity;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace WitBird.Sex.Web.Areas.Admin.Content.swfupload
{
    /// <summary>
    /// uploadAlbumImages 的摘要说明
    /// </summary>
    public class uploadAlbumImages : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            try
            {
                HttpPostedFile file;
                for (int i = 0; i < context.Request.Files.Count; ++i)
                {
                    file = context.Request.Files[i];
                    if (file == null || file.ContentLength == 0 || string.IsNullOrEmpty(file.FileName)) continue;

                    DateTime time = DateTime.Now;

                    //存放图片的目录 转换为物理路径 如: /uploadfiles/2014/08/16/
                    string folderName = "/uploadfiles/images/" + time.Year + "/" + time.ToString("MM") + "/" + time.ToString("dd") + "/";

                    //服务器存处物理地址
                    string uploadPath = context.Server.MapPath("~" + folderName);
                    //如果不存在path目录
                    if (!Directory.Exists(uploadPath))
                    {
                        //那么就创建它
                        Directory.CreateDirectory(uploadPath);
                    }

                    //文件名
                    string fileName = time.ToString("yyyyMMddHHmmssfff");
                    //后缀名称
                    string filetrype = System.IO.Path.GetExtension(file.FileName);

                    //保存文件的物理路径
                    string originaluploadFile = uploadPath + file.FileName;
                    string originalWaterFile = uploadPath + fileName + filetrype;
                    string thumbnailWidth102x102 = uploadPath + fileName + "-102x102" + filetrype;
                    string thumbnailWidth235x350 = uploadPath + fileName + "-235x350" + filetrype;
                    string thumbnailWidth490x350 = uploadPath + fileName + "-490x350" + filetrype;
                    string thumbnailHeight200 = uploadPath + fileName + "-big" + filetrype;

                    //保存原始图片
                    file.SaveAs(originaluploadFile);
                    //服务器端水印图片路径(此处用的相对路径)
                    string webFilePath_sypf = context.Server.MapPath("/content/images/water.png");
                    //ImageUtility.AddWaterPic(originaluploadFile, originalWaterFile, webFilePath_sypf);
                    ImageUtility.ZoomAuto(originaluploadFile, originalWaterFile, 940, 700, string.Empty, webFilePath_sypf);
                    ImageUtility.CutForCustom(originaluploadFile, thumbnailWidth102x102, 102, 102);
                    ImageUtility.CutForCustom(originaluploadFile, thumbnailWidth235x350, 235, 350);
                    ImageUtility.CutForCustom(originaluploadFile, thumbnailWidth490x350, 490, 350);
                    //ImageUtility.CutForCustom(originaluploadFile, thumbnailHeight200, 200, 200);
                    //重新保存一张原图
                    ImageUtility.ZoomAuto(originaluploadFile, thumbnailHeight200, 0, 0, string.Empty, string.Empty);
                    //删除原始图片
                    if (File.Exists(originaluploadFile))
                    {
                        File.Delete(originaluploadFile);
                    }

                    context.Response.Write(folderName + fileName + filetrype);
                }
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.Write(ex.Message);
                context.Response.End();
                WitBird.Sex.Common.LogService.Log("上传相册发生异常", ex.ToString());
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