using Newtonsoft.Json;
using OfficeWopi_YunstormLib.Models;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace OfficeWopi_YunstormLib
{
    public class WaterMarkHelper : IWaterMarkHelper
    {
        /// <summary>
        /// 生成水印图片
        /// </summary>
        /// <param name="token">水印文字</param>
        /// <param name="repeat">是否平铺</param>
        /// <returns>Base64</returns>
        public string GetBase64Png(string token, out bool repeat)
        {
            try
            {
                // 水印信息
                var markReq = GetWaterMarkInfo(token);
                // 解析信息 
                var dynamicObj = JsonConvert.DeserializeObject<dynamic>(markReq.WaterInfo);

                // 无水印
                if (dynamicObj["watermarktype"].Value == 0)
                {
                    repeat = true;
                    return string.Empty;
                }

                // 水印配置信息
                var watermarkconfig = JsonConvert.DeserializeObject<dynamic>(dynamicObj["watermarkconfig"].Value);
                var dateInfo = watermarkconfig["date"];
                var textInfo = watermarkconfig["text"];
                var userInfo = watermarkconfig["user"];

                bool textEnable = textInfo["enabled"].Value; // 特殊字体水印
                string waterWords = textInfo["content"].Value; // 特殊字体文字
                long fontSizeText = textInfo["fontSize"].Value;// 特殊字体大小

                bool userEnable = userInfo["enabled"].Value; // 用户水印
                long fontSizeUser = userInfo["fontSize"].Value; //用户水印大小

                bool dateEnable = dateInfo["enabled"].Value;  // 日期水印
                long fontSizeDate = dateInfo["fontSize"].Value; // 日期大小

                // 图片属性
                int width = 400;
                int height = 360;
                int startX = -35;
                int markY = -40;

                // 是否平铺（平铺：ture，居中：false）
                repeat = dateInfo["layout"].Value == 1 || textInfo["layout"].Value == 1 || userInfo["layout"].Value == 1;
                if (!repeat)
                {
                    width = 800;
                    height = 600;
                    startX = -70;
                    markY = -100;
                }

                // 特殊字体
                if (textEnable)
                {
                    if (waterWords.Length > 10 ||
                       (waterWords.Length > 8 && fontSizeText >= 32) ||
                       (waterWords.Length > 7 && fontSizeText >= 36) ||
                       (waterWords.Length > 6 && fontSizeText >= 40) ||
                       (waterWords.Length > 5 && fontSizeText >= 48))
                    {
                        width = 800;
                        height = 600;
                        startX = -70;
                        markY = -100;
                    }
                }

                // 时间
                if (dateEnable && fontSizeDate > 32)
                {
                    width = 800;
                    height = 600;
                    startX = -70;
                    markY = -100;
                }

                // 画图设置
                Bitmap bm = new Bitmap(width, height, PixelFormat.Format32bppPArgb);
                Graphics g = Graphics.FromImage(bm);
                Color bgcolor = ColorTranslator.FromHtml("#fff0");
                g.FillRectangle(new SolidBrush(bgcolor), new Rectangle(0, 0, width, height));
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias; //去掉字体黑边
                bool isRotateTransform = false;  // 是否已旋转

                // 特殊字体水印
                if (textEnable)
                {
                    if (!string.IsNullOrEmpty(waterWords))
                    {
                        long transparency = textInfo["transparency"].Value;
                        string fontColor = textInfo["color"].Value;

                        Color color = ColorTranslator.FromHtml(fontColor);
                        Font crFont = new Font("SimSun", fontSizeText, FontStyle.Regular);
                        SizeF crSize = g.MeasureString(waterWords, crFont);
                        int strX = (int)((float)width - crSize.Width) / 2;
                        int strY = (int)((float)height - crSize.Height) / 2;
                        if (!isRotateTransform)
                        {
                            g.TranslateTransform(startX, strY); //设置旋转中心为文字中心
                            g.RotateTransform((float)(-35)); //旋转
                            isRotateTransform = true;
                        }
                        g.DrawString(waterWords, crFont, new SolidBrush(Color.FromArgb((int)transparency, color)), strX - 20, strY + markY);
                        markY += (int)crSize.Height;
                    }
                }

                // 用户水印
                if (userEnable)
                {
                    long transparency = userInfo["transparency"].Value;
                    string fontColor = userInfo["color"].Value;

                    Color color = ColorTranslator.FromHtml(fontColor);
                    Font crFont = new Font("SimSun", fontSizeUser, FontStyle.Regular);

                    // 用户名
                    SizeF crSize = g.MeasureString(markReq.UserName, crFont);
                    int strX = (int)((float)width - crSize.Width) / 2;
                    int strY = (int)((float)height - crSize.Height) / 2;
                    if (!isRotateTransform)
                    {
                        g.TranslateTransform(startX, strY); //设置旋转中心为文字中心
                        g.RotateTransform((float)(-35)); //旋转
                        isRotateTransform = true;
                    }
                    g.DrawString(markReq.UserName, crFont, new SolidBrush(Color.FromArgb((int)transparency, color)), strX - 20, strY + markY);
                    markY += (int)crSize.Height;

                    // 账号
                    SizeF crSize2 = g.MeasureString(markReq.UserAccount, crFont);
                    int strX2 = (int)((float)width - crSize2.Width) / 2;
                    int strY2 = (int)((float)height - crSize2.Height) / 2;
                    g.DrawString(markReq.UserAccount, crFont, new SolidBrush(Color.FromArgb((int)transparency, color)), strX2 - 20, strY2 + markY);
                    markY += (int)crSize2.Height;
                }

                // 日期水印
                if (dateEnable)
                {
                    string dateStr = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                    long transparency = dateInfo["transparency"].Value;
                    string fontColor = dateInfo["color"].Value;

                    Color color = ColorTranslator.FromHtml(fontColor);
                    Font crFont = new Font("SimSun", fontSizeDate, FontStyle.Regular);
                    SizeF crSize = g.MeasureString(dateStr, crFont);
                    int strX = (int)((float)width - crSize.Width) / 2;
                    int strY = (int)((float)height - crSize.Height) / 2;
                    if (!isRotateTransform)
                    {
                        g.TranslateTransform(startX, strY); //设置旋转中心为文字中心
                        g.RotateTransform((float)(-35)); //旋转
                    }
                    g.DrawString(dateStr, crFont, new SolidBrush(Color.FromArgb((int)transparency, color)), strX - 20, strY + markY);
                }

                // 保存图片
                MemoryStream ms = new MemoryStream();
                bm.Save(ms, ImageFormat.Png);
                return $"data:image/png;base64,{Convert.ToBase64String(ms.GetBuffer())}";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get Watermark Info
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>WaterMarkReq</returns>
        private WaterMarkReq GetWaterMarkInfo(string token)
        {
            WaterMarkReq markReq = null;
            try
            {
                if (CommonHelper.IsShareFile(token))
                {
                    IEASLinkAPIHelper eASAPI = new EASLinkAPIHelper();

                    // 水印配置
                    string waterInfo = eASAPI.GetShareFileCheckWaterMark(token);

                    // 分享文件信息
                    var fileInfo = eASAPI.GetShareFileInfo(token);
                    var dynamicObj = JsonConvert.DeserializeObject<dynamic>(fileInfo);
                    string name = dynamicObj["usrdisplayname"].Value;
                    string account = dynamicObj["usrloginname"].Value;

                    markReq = new WaterMarkReq()
                    {
                        WaterInfo = waterInfo,
                        UserAccount = account,
                        UserName = name
                    };
                }
                else
                {
                    IEASAPIHelper eASAPI = new EASAPIHelper();

                    // 水印配置
                    string waterInfo = eASAPI.DirCheckwatermarkPost(token);

                    // Get User info
                    var user = eASAPI.UserGetPost(token);

                    markReq = new WaterMarkReq()
                    {
                        WaterInfo = waterInfo,
                        UserAccount = user.Account,
                        UserName = user.Name
                    };
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return markReq;
        }
    }
}
