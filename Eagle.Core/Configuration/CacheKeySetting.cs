using System;
using System.Collections.Generic;

namespace Eagle.Core.Configuration
{
    public class CacheKeySetting
    {
        public const string MenuByRole = "EAGLE-Menu-MenuByRole";
        public const string MenuDesktop = "EAGLE-Menu-DesktopMenu";
        public const string SiteMap = "EAGLE-SiteMap-AdminSiteMap";
        public const string SiteMapDesktop = "EAGLE-SiteMap-DesktopSiteMap";
        public const string CurrencyCode = "EAGLE-CurrencyCode";
        public const string LanguageCode = "EAGLE-LanguageCode";
        public const string DateTimeFormat = "EAGLE-DateTimeFormat";
        public const string VendorId = "EAGLE-VendorId";
        public const string ThemeName = "EAGLE-Theme-ThemeName";
        public const string ThemeSrc = "EAGLE-Theme-ThemeSrc";
        public const string Customer = "EAGLE-Customer";
        public const string TopBanner = "EAGLE-TopBanner";
        public const string LeftBanner = "EAGLE-LeftBanner";
        public const string RightBanner = "EAGLE-RightBanner";
        public const string FooterBanner = "EAGLE-FooterBanner";
        public const string Vendor = "EAGLE-Vendor";

        //Config path to save cache folder
        public const string PathToCache = "/Cache";
        public List<string> Caches = new List<string>();

        public CacheKeySetting()
        {
            Caches.Add(MenuByRole);
            Caches.Add(MenuDesktop);
            Caches.Add(SiteMap);
            Caches.Add(SiteMapDesktop);
            Caches.Add(CurrencyCode);
            Caches.Add(LanguageCode);
            Caches.Add(DateTimeFormat);
            Caches.Add(VendorId);
            Caches.Add(ThemeName);
            Caches.Add(ThemeSrc);
        }
    }

    public class CacheBaseSetting
    {
        public CacheBaseSetting(string key, string value)
        {
            Key = key;
            Value = value;
        }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
