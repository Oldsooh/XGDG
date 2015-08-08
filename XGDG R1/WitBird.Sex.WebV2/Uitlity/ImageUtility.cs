using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace WitBird.Sex.WebV2.Uitlity
{
    public class ImageUtility
    {
        #region 自定义裁剪并缩放

        /// <summary>
        /// 指定长宽裁剪
        /// 按模版比例最大范围的裁剪图片并缩放至模版尺寸
        /// </summary>
        /// <remarks>吴剑 2012-08-08</remarks>
        /// <param name="fromFile">原图路径</param>
        /// <param name="fileSaveUrl">保存路径</param>
        /// <param name="maxWidth">最大宽(单位:px)</param>
        /// <param name="maxHeight">最大高(单位:px)</param>
        /// <param name="quality">质量（范围0-100）</param>
        public static void CutForCustom(string fromFile, string fileSaveUrl, int maxWidth, int maxHeight, int quality = 0, bool setQuality = false)
        {
            //从文件获取原始图片，并使用流中嵌入的颜色管理信息
            using (System.Drawing.Image initImage = System.Drawing.Image.FromFile(fromFile, true))
            {
                Image cloneImage = initImage.Clone() as Image;
                //原图宽高均小于模版，不作处理，直接保存
                if (cloneImage.Width <= maxWidth && cloneImage.Height <= maxHeight)
                {
                    cloneImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                else
                {
                    //模版的宽高比例
                    double templateRate = (double)maxWidth / maxHeight;
                    //原图片的宽高比例
                    double initRate = (double)cloneImage.Width / cloneImage.Height;

                    //原图与模版比例相等，直接缩放
                    if (templateRate == initRate)
                    {
                        //按模版大小生成最终图片
                        System.Drawing.Image templateImage = new System.Drawing.Bitmap(maxWidth, maxHeight);
                        System.Drawing.Graphics templateG = System.Drawing.Graphics.FromImage(templateImage);
                        templateG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                        templateG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        templateG.Clear(Color.White);
                        templateG.DrawImage(cloneImage, new System.Drawing.Rectangle(0, 0, maxWidth, maxHeight), new System.Drawing.Rectangle(0, 0, initImage.Width, initImage.Height), System.Drawing.GraphicsUnit.Pixel);
                        templateImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    //原图与模版比例不等，裁剪后缩放
                    else
                    {
                        //裁剪对象
                        System.Drawing.Image pickedImage = null;
                        System.Drawing.Graphics pickedG = null;

                        //定位
                        Rectangle fromR = new Rectangle(0, 0, 0, 0);//原图裁剪定位
                        Rectangle toR = new Rectangle(0, 0, 0, 0);//目标定位

                        //宽为标准进行裁剪
                        if (templateRate > initRate)
                        {
                            //裁剪对象实例化
                            pickedImage = new System.Drawing.Bitmap(cloneImage.Width, (int)System.Math.Floor(cloneImage.Width / templateRate));
                            pickedG = System.Drawing.Graphics.FromImage(pickedImage);

                            //裁剪源定位
                            fromR.X = 0;
                            fromR.Y = (int)System.Math.Floor((cloneImage.Height - cloneImage.Width / templateRate) / 2);
                            fromR.Width = cloneImage.Width;
                            fromR.Height = (int)System.Math.Floor(cloneImage.Width / templateRate);

                            //裁剪目标定位
                            toR.X = 0;
                            toR.Y = 0;
                            toR.Width = cloneImage.Width;
                            toR.Height = (int)System.Math.Floor(cloneImage.Width / templateRate);
                        }
                        //高为标准进行裁剪
                        else
                        {
                            pickedImage = new System.Drawing.Bitmap((int)System.Math.Floor(cloneImage.Height * templateRate), cloneImage.Height);
                            pickedG = System.Drawing.Graphics.FromImage(pickedImage);

                            fromR.X = (int)System.Math.Floor((cloneImage.Width - cloneImage.Height * templateRate) / 2);
                            fromR.Y = 0;
                            fromR.Width = (int)System.Math.Floor(cloneImage.Height * templateRate);
                            fromR.Height = cloneImage.Height;

                            toR.X = 0;
                            toR.Y = 0;
                            toR.Width = (int)System.Math.Floor(cloneImage.Height * templateRate);
                            toR.Height = cloneImage.Height;
                        }

                        //设置质量
                        pickedG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        pickedG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                        //裁剪
                        pickedG.DrawImage(cloneImage, toR, fromR, System.Drawing.GraphicsUnit.Pixel);

                        //按模版大小生成最终图片
                        System.Drawing.Image templateImage = new System.Drawing.Bitmap(maxWidth, maxHeight);
                        System.Drawing.Graphics templateG = System.Drawing.Graphics.FromImage(templateImage);
                        templateG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                        templateG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        templateG.Clear(Color.White);
                        templateG.DrawImage(pickedImage, new System.Drawing.Rectangle(0, 0, maxWidth, maxHeight), new System.Drawing.Rectangle(0, 0, pickedImage.Width, pickedImage.Height), System.Drawing.GraphicsUnit.Pixel);

                        //关键质量控制
                        //获取系统编码类型数组,包含了jpeg,bmp,png,gif,tiff
                        if (setQuality)
                        {
                            ImageCodecInfo[] icis = ImageCodecInfo.GetImageEncoders();
                            ImageCodecInfo ici = null;
                            foreach (ImageCodecInfo i in icis)
                            {
                                if (i.MimeType == "image/jpeg" || i.MimeType == "image/bmp" || i.MimeType == "image/png" || i.MimeType == "image/gif")
                                {
                                    ici = i;
                                }
                            }
                            EncoderParameters ep = new EncoderParameters(1);
                            ep.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)quality);

                            //保存缩略图
                            templateImage.Save(fileSaveUrl, ici, ep);
                        }
                        else
                        {
                            templateImage.Save(fileSaveUrl, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }

                        //释放资源
                        templateG.Dispose();
                        templateImage.Dispose();

                        pickedG.Dispose();
                        pickedImage.Dispose();
                    }
                }

                //释放资源
                cloneImage.Dispose();
            }
        }
        #endregion

        #region 等比缩放

        /// <summary>
        /// 图片等比缩放
        /// </summary>
        /// <remarks>吴剑 2012-08-08</remarks>
        /// <param name="fromFile">原图路径</param>
        /// <param name="savePath">缩略图存放地址</param>
        /// <param name="targetWidth">指定的最大宽度, 0表示原图宽度</param>
        /// <param name="targetHeight">指定的最大高度, 0表示原图高度</param>
        /// <param name="watermarkText">水印文字(为""表示不使用水印)</param>
        /// <param name="watermarkImage">水印图片路径(为""表示不使用水印)</param>
        public static void ZoomAuto(string fromFile, string savePath, System.Double targetWidth, System.Double targetHeight, string watermarkText, string watermarkImage)
        {
            //原始图片（获取原始图片创建对象，并使用流中嵌入的颜色管理信息）
            using (System.Drawing.Image initImage = System.Drawing.Image.FromFile(fromFile, true))
            {
                Image cloneImage = initImage.Clone() as Image;
                targetWidth = targetWidth == 0 ? cloneImage.Width : targetWidth;
                targetHeight = targetHeight == 0 ? cloneImage.Height : targetHeight;
                //原图宽高均小于模版，不作处理，直接保存
                if (cloneImage.Width <= targetWidth && cloneImage.Height <= targetHeight)
                {
                    //文字水印
                    if (watermarkText != "")
                    {
                        using (System.Drawing.Graphics gWater = System.Drawing.Graphics.FromImage(cloneImage))
                        {
                            System.Drawing.Font fontWater = new Font("黑体", 10);
                            System.Drawing.Brush brushWater = new SolidBrush(Color.White);
                            gWater.DrawString(watermarkText, fontWater, brushWater, 10, 10);
                            gWater.Dispose();
                        }
                    }

                    //透明图片水印
                    if (watermarkImage != "")
                    {
                        if (File.Exists(watermarkImage))
                        {
                            //获取水印图片
                            using (System.Drawing.Image wrImage = System.Drawing.Image.FromFile(watermarkImage))
                            {
                                //水印绘制条件：原始图片宽高均大于或等于水印图片
                                if (cloneImage.Width >= wrImage.Width && cloneImage.Height >= wrImage.Height)
                                {
                                    Graphics gWater = Graphics.FromImage(cloneImage);

                                    //透明属性
                                    ImageAttributes imgAttributes = new ImageAttributes();
                                    ColorMap colorMap = new ColorMap();
                                    colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                                    colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                                    ColorMap[] remapTable = { colorMap };
                                    imgAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

                                    float[][] colorMatrixElements = 
                                    { 
                                        new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                        new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                        new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                        new float[] {0.0f,  0.0f,  0.0f,  0.5f, 0.0f},//透明度:0.5
                                        new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                    };

                                    ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);
                                    imgAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                                    gWater.DrawImage(wrImage, new Rectangle(cloneImage.Width - wrImage.Width,
                                        cloneImage.Height - wrImage.Height, wrImage.Width, wrImage.Height), 0, 0, wrImage.Width,
                                        wrImage.Height, GraphicsUnit.Pixel, imgAttributes);

                                    gWater.Dispose();
                                }
                                wrImage.Dispose();
                            }
                        }
                    }

                    //保存
                    cloneImage.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    cloneImage.Dispose();
                }
                else
                {
                    //缩略图宽、高计算
                    double newWidth = cloneImage.Width;
                    double newHeight = cloneImage.Height;

                    //宽大于高或宽等于高（横图或正方）
                    if (cloneImage.Width > cloneImage.Height || cloneImage.Width == cloneImage.Height)
                    {
                        //如果宽大于模版
                        if (cloneImage.Width > targetWidth)
                        {
                            //宽按模版，高按比例缩放
                            newWidth = targetWidth;
                            newHeight = cloneImage.Height * (targetWidth / cloneImage.Width);
                        }
                    }
                    //高大于宽（竖图）
                    else
                    {
                        //如果高大于模版
                        if (cloneImage.Height > targetHeight)
                        {
                            //高按模版，宽按比例缩放
                            newHeight = targetHeight;
                            newWidth = cloneImage.Width * (targetHeight / cloneImage.Height);
                        }
                    }

                    //生成新图
                    //新建一个bmp图片
                    System.Drawing.Image newImage = new System.Drawing.Bitmap((int)newWidth, (int)newHeight);
                    //新建一个画板
                    System.Drawing.Graphics newG = System.Drawing.Graphics.FromImage(newImage);

                    //设置质量
                    newG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    newG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    //置背景色
                    newG.Clear(Color.White);
                    //画图
                    newG.DrawImage(cloneImage, new System.Drawing.Rectangle(0, 0, newImage.Width, newImage.Height), new System.Drawing.Rectangle(0, 0, cloneImage.Width, cloneImage.Height), System.Drawing.GraphicsUnit.Pixel);

                    //文字水印
                    if (watermarkText != "")
                    {
                        using (System.Drawing.Graphics gWater = System.Drawing.Graphics.FromImage(newImage))
                        {
                            System.Drawing.Font fontWater = new Font("宋体", 10);
                            System.Drawing.Brush brushWater = new SolidBrush(Color.White);
                            gWater.DrawString(watermarkText, fontWater, brushWater, 10, 10);
                            gWater.Dispose();
                        }
                    }

                    //透明图片水印
                    if (watermarkImage != "")
                    {
                        if (File.Exists(watermarkImage))
                        {
                            //获取水印图片
                            using (System.Drawing.Image wrImage = System.Drawing.Image.FromFile(watermarkImage))
                            {
                                //水印绘制条件：原始图片宽高均大于或等于水印图片
                                if (newImage.Width >= wrImage.Width && newImage.Height >= wrImage.Height)
                                {
                                    Graphics gWater = Graphics.FromImage(newImage);

                                    //透明属性
                                    ImageAttributes imgAttributes = new ImageAttributes();
                                    ColorMap colorMap = new ColorMap();
                                    colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
                                    colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);
                                    ColorMap[] remapTable = { colorMap };
                                    imgAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

                                    float[][] colorMatrixElements = { 
                                   new float[] {1.0f,  0.0f,  0.0f,  0.0f, 0.0f},
                                   new float[] {0.0f,  1.0f,  0.0f,  0.0f, 0.0f},
                                   new float[] {0.0f,  0.0f,  1.0f,  0.0f, 0.0f},
                                   new float[] {0.0f,  0.0f,  0.0f,  0.5f, 0.0f},//透明度:0.5
                                   new float[] {0.0f,  0.0f,  0.0f,  0.0f, 1.0f}
                                };

                                    ColorMatrix wmColorMatrix = new ColorMatrix(colorMatrixElements);
                                    imgAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
                                    gWater.DrawImage(wrImage, new Rectangle(newImage.Width - wrImage.Width, newImage.Height - wrImage.Height, wrImage.Width, wrImage.Height), 0, 0, wrImage.Width, wrImage.Height, GraphicsUnit.Pixel, imgAttributes);
                                    gWater.Dispose();
                                }
                                wrImage.Dispose();
                            }
                        }
                    }

                    //保存缩略图
                    newImage.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);

                    //释放资源
                    newG.Dispose();
                    newImage.Dispose();
                    cloneImage.Dispose();
                }

                initImage.Dispose();
            }
        }

        #endregion

        #region 其它

        /// <summary>
        /// 判断文件类型是否为WEB格式图片
        /// (注：JPG,GIF,BMP,PNG)
        /// </summary>
        /// <param name="contentType">HttpPostedFile.ContentType</param>
        /// <returns></returns>
        public static bool IsWebImage(string contentType)
        {
            if (contentType == "image/pjpeg" || contentType == "image/jpeg" || contentType == "image/gif" || contentType == "image/bmp" || contentType == "image/png")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

    }
}