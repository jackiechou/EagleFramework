using System.Collections.Concurrent;
using System.Web;
using Eagle.Core.Session;
using Eagle.Services.Dtos.Business;

namespace Eagle.Services.Common
{
    public class SessionExtension
    {
        private readonly HttpContext _current;
        public SessionExtension(HttpContext context)
        {
            _current = context;
            if (CurrentPageViews == null)
            {
                CurrentPageViews = new ConcurrentDictionary<string, dynamic>();
            }
        }
        public ConcurrentDictionary<string, dynamic> CurrentPageViews
        {
            get
            {
                return _current.Session[SessionKey.CurrentPageView] as dynamic;
            }
            set
            {
                _current.Session[SessionKey.CurrentPageView] = value;
            }
        }
        public bool IsAuthenticated
        {
            get
            {
                if (_current.Session[SessionKey.IsAuthenticated] == null)
                    return false;
                return (bool)_current.Session[SessionKey.IsAuthenticated];
            }
            set
            {
                _current.Session[SessionKey.IsAuthenticated] = value;
            }
        }
        public string CurrentTimeZone
        {
            get
            {
                return _current.Session[SessionKey.CurrentTimeZone] as string;
            }
            set
            {
                _current.Session[SessionKey.CurrentTimeZone] = value;
            }
        }

        public static string DomainName
        {
            get
            {
                if (HttpContext.Current.Session != null)
                {
                    if (HttpContext.Current.Session[SessionKey.DomainName] != null)
                    {
                        return HttpContext.Current.Session[SessionKey.DomainName].ToString();
                    }
                }

                return string.Empty;
            }
            set
            {
                HttpContext.Current.Session[SessionKey.DomainName] = value;
            }
        }

        public static string DesiredUrl
        {
            get
            {
                if (HttpContext.Current.Session != null)
                {
                    if (HttpContext.Current.Session[SessionKey.DesiredUrl] != null)
                    {
                        return HttpContext.Current.Session[SessionKey.DesiredUrl].ToString();
                    }
                }

                return string.Empty;
            }
            set
            {
                HttpContext.Current.Session[SessionKey.DesiredUrl] = value;
            }
        }

        public static string UserName
        {
            get
            {
                return HttpContext.Current.Session[SessionKey.UserName].ToString();
            }
            set
            {
                HttpContext.Current.Session[SessionKey.UserName] = value;
            }
        }
       
        public static CustomerInfoDetail CustomerInfo
        {
            get
            {
                return (CustomerInfoDetail)HttpContext.Current.Session[SessionKey.CustomerInfo];
            }
            set
            {
                HttpContext.Current.Session[SessionKey.CustomerInfo] = value;
            }
        }
        //public string CurrentCity
        //{
        //    get
        //    {
        //        return _current.Session["CurrentCity"] as string;
        //    }
        //    set
        //    {
        //        _current.Session["CurrentCity"] = value;
        //    }
        //}

        //public ProfileUserModel CurrentUser
        //{
        //    get
        //    {
        //        return _current.Session["CurrentUser"] as dynamic;
        //    }
        //    set
        //    {
        //        _current.Session["CurrentUser"] = value;
        //    }
        //}
    }
}
