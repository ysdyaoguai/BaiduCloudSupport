﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaiduCloudSupport
{
    class Setting
    {
        /// <summary>
        /// Program base path,with "\" at the last.
        /// </summary>
        public static readonly string BasePath = System.AppDomain.CurrentDomain.BaseDirectory;

        public static readonly string Baidu_CookiePath = BasePath + @"Cookie\BaiduCloud.txt";

        public enum APIMODE { PCS, BDC };

        /// <summary>
        /// API mode selector, 0:PCS, 1:BDC
        /// </summary>
        public static APIMODE APIMode = APIMODE.BDC;

        /// <summary>
        /// Program text language
        /// </summary>
        public static string MainLanguage = ConfigurationManager.AppSettings["MainLanguage"];

        public static string Baidu_client_id = ConfigurationManager.AppSettings["Baidu_client_id"];

        public static string Baidu_redirect_uri = ConfigurationManager.AppSettings["Baidu_redirect_uri"];

        public static string Baidu_Access_Token = ConfigurationManager.AppSettings["Baidu_Access_Token"];

        public static string Baidu_Expires_In = ConfigurationManager.AppSettings["Baidu_Expires_In"];

        public static string Baidu_Session_Secret = ConfigurationManager.AppSettings["Baidu_Session_Secret"];

        public static string Baidu_Session_Key = ConfigurationManager.AppSettings["Baidu_Session_Key"];

        public static string Baidu_Scope = ConfigurationManager.AppSettings["Baidu_Scope"];

        public static string Baidu_uid = ConfigurationManager.AppSettings["Baidu_uid"];

        public static string Baidu_uname = ConfigurationManager.AppSettings["Baidu_uname"];

        public static string Baidu_portrait = ConfigurationManager.AppSettings["Baidu_portrait"];

        public static string UserPortraitFilePath = ConfigurationManager.AppSettings["UserPortraitFilePath"];

        public static string Baidu_Quota_Total = ConfigurationManager.AppSettings["Baidu_Quota_Total"];

        public static string Baidu_Quota_Used = ConfigurationManager.AppSettings["Baidu_Quota_Used"];

        public static string DownloadPath = ConfigurationManager.AppSettings["DownloadPath"];

        public static string DownloadSegment = ConfigurationManager.AppSettings["DownloadSegment"];

        /// <summary>
        /// Reload setting data
        /// </summary>
        public static void Reload()
        {
            ConfigurationManager.RefreshSection("appSettings");
            MainLanguage = ConfigurationManager.AppSettings["MainLanguage"];
            Baidu_client_id = ConfigurationManager.AppSettings["Baidu_client_id"];
            Baidu_redirect_uri = ConfigurationManager.AppSettings["Baidu_redirect_uri"];
            Baidu_Access_Token = ConfigurationManager.AppSettings["Baidu_Access_Token"];
            Baidu_Expires_In = ConfigurationManager.AppSettings["Baidu_Expires_In"];
            Baidu_Session_Secret = ConfigurationManager.AppSettings["Baidu_Session_Secret"];
            Baidu_Session_Key = ConfigurationManager.AppSettings["Baidu_Session_Key"];
            Baidu_Scope = ConfigurationManager.AppSettings["Baidu_Scope"];
            Baidu_uid = ConfigurationManager.AppSettings["Baidu_uid"];
            Baidu_uname = ConfigurationManager.AppSettings["Baidu_uname"];
            Baidu_portrait = ConfigurationManager.AppSettings["Baidu_portrait"];
            UserPortraitFilePath = ConfigurationManager.AppSettings["UserPortraitFilePath"];
            Baidu_Quota_Total = ConfigurationManager.AppSettings["Baidu_Quota_Total"];
            Baidu_Quota_Used = ConfigurationManager.AppSettings["Baidu_Quota_Used"];
            DownloadPath = ConfigurationManager.AppSettings["DownloadPath"];
            DownloadSegment = ConfigurationManager.AppSettings["DownloadSegment"];
        }

        /// <summary>
        /// Read app setting from app.config
        /// </summary>
        /// <param name="keyword">Resource keyword</param>
        /// <returns>Resource</returns>
        public static string ReadAppSetting(string keyword)
        {
            return System.Configuration.ConfigurationManager.AppSettings[keyword];
        }

        /// <summary>
        /// Set app setting to app.config
        /// </summary>
        /// <param name="keyword">Resource keyword</param>
        /// <param name="value">Resource</param>
        /// <param name="needReloadSetting">Reload Setting after set config</param>
        /// <returns></returns>
        public static bool WriteAppSetting(string keyword, string value, bool needReloadSetting = true)
        {
            try
            {
                System.Configuration.Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
                config.AppSettings.Settings[keyword].Value = value;
                config.Save();
                if (needReloadSetting)
                {
                    Setting.Reload();
                }
                config = null;
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog("", ex);
                return false;
            }
        }

        public static bool CheckApiMode(APIMODE needMode, bool autoChange = true)
        {
            if (Setting.APIMode == needMode)
            {
                return true;
            }else
            {
                if (autoChange)
                {
                    switch (needMode)
                    {
                        case APIMODE.PCS:
                            if (Setting.Baidu_Access_Token != null && !Setting.Baidu_Access_Token.Equals(""))
                            {
                                Setting.APIMode = APIMODE.PCS;
                                return true;
                            }
                            break;
                        case APIMODE.BDC:
                            if (API.BDC.IsCookieFileExist(Setting.Baidu_CookiePath))
                            {
                                if (!API.BDC.CheckCookie())
                                {
                                    API.BDC.LoadCookie(Setting.Baidu_CookiePath);
                                }
                                Setting.APIMode = APIMODE.BDC;
                                return true;
                            }
                            break;
                    }
                }
            }
            return false;
        }

        public static Task<bool> CheckApiModeAsync(APIMODE needMode, bool autoChange = true)
        {
            return Task.Factory.StartNew(()=> {
                return CheckApiMode(needMode, autoChange);
            });
        }
    }
}
