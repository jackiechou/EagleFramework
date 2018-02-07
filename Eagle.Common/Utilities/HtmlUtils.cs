using System;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;

namespace Eagle.Common.Utilities
{
    public class HtmlUtils
    {
        private static readonly Regex HtmlDetectionRegex = new Regex("^\\s*<\\w*(.*\\s*)*\\w?>\\s*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        static readonly Regex HtmlRegex = new Regex("<.*?>", RegexOptions.Compiled);

        public static string Clean(string html, bool removePunctuation)
        {
            html = StripTags(html, true);
            html = HttpUtility.HtmlDecode(html);
            if (removePunctuation)
            {
                html = StripPunctuation(html, true);
            }
            html = StripWhiteSpace(html, true);
            return html;
        }
        public static string FormatEmail(string email)
        {
            return FormatEmail(email, true);
        }
        public static string FormatEmail(string email, bool cloak)
        {
            string formatEmail = "";
            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(email.Trim()))
            {
                if (email.IndexOf("@", StringComparison.Ordinal) != -1)
                {
                    formatEmail = "<a href=\"mailto:" + email + "\">" + email + "</a>";
                }
                else
                {
                    formatEmail = email;
                }
            }
            if (cloak)
            {
                formatEmail = CloakText(formatEmail);
            }
            return formatEmail;
        }

        public static string CloakText(string personalInfo)
        {
            if (personalInfo != null)
            {
                StringBuilder sb = new StringBuilder();
                char[] chars = personalInfo.ToCharArray();
                foreach (char chr in chars)
                {
                    sb.Append(((int)chr).ToString());
                }
                if (sb.Length > 0)
                    sb.Remove(sb.Length - 1, 1);
                StringBuilder sbScript = new StringBuilder();
                sbScript.Append(Environment.NewLine + "<script type=\"text/javascript\">" + Environment.NewLine);
                sbScript.Append("//<![CDATA[" + Environment.NewLine);
                sbScript.Append("   document.write(String.fromCharCode(" + sb + "))" + Environment.NewLine);
                sbScript.Append("//]]>" + Environment.NewLine);
                sbScript.Append("</script>" + Environment.NewLine);
                return sbScript.ToString();
            }
            else
            {
                return null;
            }
        }
        public static string FormatText(string html, bool retainSpace)
        {
            string brMatch = "\\s*<\\s*[bB][rR]\\s*/\\s*>\\s*";
            return Regex.Replace(html, brMatch, Environment.NewLine);
        }
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Formats String as Html by replacing linefeeds by <br/>
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="strText">Text to format</param>
        /// <returns>The formatted html</returns>
        /// <history>
        /// [cnurse]	12/13/2004	Documented
        /// </history>
        /// -----------------------------------------------------------------------------
        public static string ConvertToHtml(string strText)
        {
            string strHtml = strText;
            if (!string.IsNullOrEmpty(strHtml))
            {
                strHtml = strHtml.Replace("\n", "");
                strHtml = strHtml.Replace("\r", "<br />");
            }
            return strHtml;
        }
        /// -----------------------------------------------------------------------------
        /// <summary>
        /// Formats Html as text by removing <br/> tags and replacing by linefeeds
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="strHtml">Html to format</param>
        /// <returns>The formatted text</returns>
        /// <history>
        /// [cnurse]	12/13/2004	Documented and modified to use HtmlUtils methods
        /// </history>
        /// -----------------------------------------------------------------------------
        public static string ConvertToText(string strHtml)
        {
            string strText = strHtml;

            if (!string.IsNullOrEmpty(strText))
            {
                //First remove white space (html does not render white space anyway and it screws up the conversion to text)
                //Replace it by a single space
                strText = StripWhiteSpace(strText, true);

                //Replace all variants of <br> by Linefeeds
                strText = FormatText(strText, false);
            }
            return strText;
        }
        public static string ConvertHtmlToText(string sHtml)
        {
            string sContent = sHtml;
            sContent = sContent.Replace("&nbsp;", " ");
            sContent = sContent.Replace("<br />", Environment.NewLine);
            sContent = sContent.Replace("<br>", Environment.NewLine);
            sContent = FormatText(sContent, true);
            sContent = StripTags(sContent, true);
            return sContent;
        }
        public static string FormatWebsite(object website)
        {
            string formatWebsite = "";
            if (website != DBNull.Value)
            {
                if (!String.IsNullOrEmpty(website.ToString().Trim()))
                {
                    if (website.ToString().IndexOf(".", StringComparison.Ordinal) > -1)
                    {
                        formatWebsite = "<a href=\"" + (website.ToString().IndexOf("://", StringComparison.Ordinal) > -1 ? "" : "http://") + website + "\">" + website + "</a>";
                    }
                    else
                    {
                        formatWebsite = website.ToString();
                    }
                }
            }
            return formatWebsite;
        }
        public static string Shorten(string txt, int length, string suffix)
        {
            string results;
            if (txt.Length > length)
            {
                results = txt.Substring(0, length) + suffix;
            }
            else
            {
                results = txt;
            }
            return results;
        }         
        public static string StripTags(string html, bool retainSpace)
        {
            string pattern = "<[^>]*>"; //"&[^;]*;"
            string repString;
            if (retainSpace)
                repString = " ";
            else
                repString = "";
            return Regex.Replace(html, pattern, repString);
        }
        public static string StripPunctuation(string html, bool retainSpace)
        {
            string punctuationMatch = "[~!#\\$%\\^&*\\(\\)-+=\\{\\[\\}\\]\\|;:\\x22'<,>\\.\\?\\\\\\t\\r\\v\\f\\n]";
            Regex afterRegEx = new Regex(punctuationMatch + "\\s");
            Regex beforeRegEx = new Regex("\\s" + punctuationMatch);
            string retHtml = html + " ";
            string repString;
            if (retainSpace)
            {
                repString = " ";
            }
            else
            {
                repString = "";
            }
            while (beforeRegEx.IsMatch(retHtml))
            {
                retHtml = beforeRegEx.Replace(retHtml, repString);
            }
            while (afterRegEx.IsMatch(retHtml))
            {
                retHtml = afterRegEx.Replace(retHtml, repString);
            }
            return retHtml;
        }
        public static string StripWhiteSpace(string html, bool retainSpace)
        {
            if (string.IsNullOrEmpty(html)) return null;
            string repString = retainSpace ? " " : "";
            return Regex.Replace(html, "\\s+", repString);
        }

        public static string StripNonWord(string html, bool retainSpace)
        {
            if (string.IsNullOrEmpty(html)) return null;
            string repString = retainSpace ? " " : "";
            return Regex.Replace(html, "\\W*", repString);
        }

        /// <summary>
        /// Remove HTML tags from string using char array.
        /// </summary>
        public static string StripTagsCharArray(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            foreach (char let in source)
            {
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }
        /// <summary>
        /// Remove HTML from string with Regex.
        /// </summary>
        public static string StripTagsRegex(string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
        }
        public string RemoveUnwantedHtmltag(string str)
        {
            return Regex.Replace(str, @"<(.|\n)*?>", string.Empty);
        }
        /// <summary>
        /// Compiled regular expression for performance.
        /// </summary>
       

        /// <summary>
        /// Remove HTML from string with compiled Regex.
        /// </summary>
        public static string StripTagsRegexCompiled(string source)
        {
            return HtmlRegex.Replace(source, string.Empty);
        }

        public static string RemoveHtmlTags(string content)
        {
            string cleaned;
            try
            {
                StringBuilder textOnly = new StringBuilder();
                using (var reader = XmlReader.Create(new StringReader("<xml>" + content + "</xml>")))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Text)
                            textOnly.Append(reader.ReadContentAsString());
                    }
                }
                cleaned = textOnly.ToString();
            }
            catch
            {
                //A tag is probably not closed. fallback to regex string clean.
                Regex tagRemove = new Regex(@"<[^>]*(>|$)");
                Regex compressSpaces = new Regex(@"[\s\r\n]+");
                var textOnly = tagRemove.Replace(content, string.Empty);
                textOnly = compressSpaces.Replace(textOnly, " ");
                cleaned = textOnly;
            }

            return cleaned;
        }
        public static bool IsValidSearchWord(string searchWord)
        {
            return (Regex.IsMatch(searchWord, @"^[\p{L}\p{Zs}\p{Lu}\p{Ll}\']{1,40}$"));
        }
        /// <summary>
        /// Determines wether or not the passed in string contains any HTML tags
        /// </summary>
        /// <param name="text">Text to be inspected</param>
        /// <returns>True for HTML and False for plain text</returns>
        /// <remarks></remarks>
        public static bool IsHtml(string text)
        {

            if ((string.IsNullOrEmpty(text)))
            {
                return false;
            }

            return HtmlDetectionRegex.IsMatch(text);
        }
        public static string FixHtmlCode(string html)
        {
            html = html.Replace("  ", "");
            html = html.Replace("  ", "");
            html = html.Replace("\t", "");
            html = html.Replace("[", "");
            html = html.Replace("]", "");
            html = html.Replace("<", "");
            html = html.Replace(">", "");
            html = html.Replace("\r\n", "");
            return html;
        }
        public static void WriteError(HttpResponse response, string file, string message)
        {
            response.Write("<h2>Error Details</h2>");
            response.Write("<table cellspacing=0 cellpadding=0 border=0>");
            response.Write("<tr><td><b>File</b></td><td><b>" + file + "</b></td></tr>");
            response.Write("<tr><td><b>Error</b>&nbsp;&nbsp;</td><td><b>" + message + "</b></td></tr>");
            response.Write("</table>");
            response.Write("<br><br>");
            response.Flush();
        }
        //public static void WriteFeedback(HttpResponse response, Int32 indent, string message)
        //{
        //    WriteFeedback(response, indent, message, true);
        //}
        //public static void WriteFeedback(HttpResponse response, Int32 indent, string message, bool showtime)
        //{
        //    bool showInstallationMessages = true;
        //    string ConfigSetting = Config.GetSetting("ShowInstallationMessages");
        //    if (ConfigSetting != null)
        //    {
        //        showInstallationMessages = bool.Parse(ConfigSetting);
        //    }
        //    if (showInstallationMessages)
        //    {
        //        TimeSpan timeElapsed = Upgrade.RunTime;
        //        string strMessage = "";
        //        if (showtime == true)
        //        {
        //            strMessage += timeElapsed.ToString().Substring(0, timeElapsed.ToString().LastIndexOf(".") + 4) + " -";
        //        }
        //        for (int i = 0; i <= indent; i++)
        //        {
        //            strMessage += "&nbsp;";
        //        }
        //        strMessage += message;
        //        response.Write(strMessage);
        //        response.Flush();
        //    }
        //}
        public static void WriteFooter(HttpResponse response)
        {
            response.Write("</body>");
            response.Write("</html>");
            response.Flush();
        }
        //public static void WriteHeader(HttpResponse response, string mode)
        //{
        //    response.Buffer = false;
        //    if (!File.Exists(System.Web.HttpContext.Current.Server.MapPath("~/Install/Install.htm")))
        //    {
        //        if (File.Exists(System.Web.HttpContext.Current.Server.MapPath("~/Install/Install.template.htm")))
        //        {
        //            File.Copy(System.Web.HttpContext.Current.Server.MapPath("~/Install/Install.template.htm"), System.Web.HttpContext.Current.Server.MapPath("~/Install/Install.htm"));
        //        }
        //    }
        //    if (File.Exists(System.Web.HttpContext.Current.Server.MapPath("~/Install/Install.htm")))
        //    {
        //        response.Write(FileSystemUtils.ReadFile(System.Web.HttpContext.Current.Server.MapPath("~/Install/Install.htm")));
        //    }
        //    switch (mode)
        //    {
        //        case "install":
        //            response.Write("<h1>Installing DotNetNuke</h1>");
        //            break;
        //        case "upgrade":
        //            response.Write("<h1>Upgrading DotNetNuke</h1>");
        //            break;
        //        case "addPortal":
        //            response.Write("<h1>Adding New Portal</h1>");
        //            break;
        //        case "installResources":
        //            response.Write("<h1>Installing Resources</h1>");
        //            break;
        //        case "executeScripts":
        //            response.Write("<h1>Executing Scripts</h1>");
        //            break;
        //        case "none":
        //            response.Write("<h1>Nothing To Install At This Time</h1>");
        //            break;
        //        case "noDBVersion":
        //            response.Write("<h1>New DotNetNuke Database</h1>");
        //            break;
        //        case "error":
        //            response.Write("<h1>Error Installing DotNetNuke</h1>");
        //            break;
        //        default:
        //            response.Write("<h1>" + mode + "</h1>");
        //            break;
        //    }
        //    response.Flush();
        //}
        //public static void WriteSuccessError(HttpResponse response, bool bSuccess)
        //{
        //    if (bSuccess)
        //    {
        //        WriteFeedback(response, 0, "<font color='green'>Success</font><br>", false);
        //    }
        //    else
        //    {
        //        WriteFeedback(response, 0, "<font color='red'>Error!</font><br>", false);
        //    }
        //}
        //public static void WriteScriptSuccessError(HttpResponse response, bool bSuccess, string strLogFile)
        //{
        //    if (bSuccess)
        //    {
        //        WriteFeedback(response, 0, "<font color='green'>Success</font><br>", false);
        //    }
        //    else
        //    {
        //        WriteFeedback(response, 0, "<font color='red'>Error! (see " + strLogFile + " for more information)</font><br>", false);
        //    }
        //}
        public static string ConvertHtml2PlainText(string source)
        {
            try
            {
                string result;

                // Remove HTML Development formatting
                // Replace line breaks with space because browsers inserts space
                result = source.Replace("\r", " ");
                // Replace line breaks with space because browsers inserts space
                result = result.Replace("\n", " ");
                // Remove step-formatting
                result = result.Replace("\t", string.Empty);
                // Remove &nbsp;
                result = Regex.Replace(result, "nbsp;", " ");
                // Remove repeating spaces because browsers ignore them
                result = Regex.Replace(result, @"( )+", " ");

                // Remove the header (prepare first by clearing attributes)
                result = Regex.Replace(result,
                         @"<( )*head([^>])*>", "<head>",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"(<( )*(/)( )*head( )*>)", "</head>",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         "(<head>).*(</head>)", string.Empty,
                         RegexOptions.IgnoreCase);

                // remove all scripts (prepare first by clearing attributes)
                result = Regex.Replace(result,
                         @"<( )*script([^>])*>", "<script>",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"(<( )*(/)( )*script( )*>)", "</script>",
                         RegexOptions.IgnoreCase);
                //result = System.Text.RegularExpressions.Regex.Replace(result,
                //         @"(<script>)([^(<script>\.</script>)])*(</script>)",
                //         string.Empty,
                //         System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"(<script>).*(</script>)", string.Empty,
                         RegexOptions.IgnoreCase);

                // remove all styles (prepare first by clearing attributes)
                result = Regex.Replace(result,
                         @"<( )*style([^>])*>", "<style>",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"(<( )*(/)( )*style( )*>)", "</style>",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         "(<style>).*(</style>)", string.Empty,
                         RegexOptions.IgnoreCase);

                // insert tabs in spaces of <td> tags
                result = Regex.Replace(result,
                         @"<( )*td([^>])*>", "\t",
                         RegexOptions.IgnoreCase);

                // insert line breaks in places of <BR> and <LI> tags
                result = Regex.Replace(result,
                         @"<( )*br( )*>", "\r",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"<( )*li( )*>", "\r",
                         RegexOptions.IgnoreCase);

                // insert line paragraphs (double line breaks) in place
                // if <P>, <DIV> and <TR> tags
                result = Regex.Replace(result,
                         @"<( )*div([^>])*>", "\r\r",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"<( )*tr([^>])*>", "\r\r",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"<( )*p([^>])*>", "\r\r",
                         RegexOptions.IgnoreCase);

                // Remove remaining tags like <a>, links, images,
                // comments etc - anything that's enclosed inside < >
                result = Regex.Replace(result,
                         @"<[^>]*>", string.Empty,
                         RegexOptions.IgnoreCase);

                // replace special characters:
                result = Regex.Replace(result,
                         @" ", " ",
                         RegexOptions.IgnoreCase);

                result = Regex.Replace(result,
                         @"&bull;", " * ",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&lsaquo;", "<",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&rsaquo;", ">",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&trade;", "(tm)",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&frasl;", "/",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&lt;", "<",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&gt;", ">",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&copy;", "(c)",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         @"&reg;", "(r)",
                         RegexOptions.IgnoreCase);
                // Remove all others
                result = Regex.Replace(result,
                         @"&(.{2,6});", string.Empty,
                         RegexOptions.IgnoreCase);

                // for testing
                //System.Text.RegularExpressions.Regex.Replace(result,
                //       this.txtRegex.Text,string.Empty,
                //       System.Text.RegularExpressions.RegexOptions.IgnoreCase);

                // make line breaking consistent
                result = result.Replace("\n", "\r");

                // Remove extra line breaks and tabs:
                // replace over 2 breaks with 2 and over 4 tabs with 4.
                // Prepare first to remove any whitespaces in between
                // the escaped characters and remove redundant tabs in between line breaks
                result = Regex.Replace(result,
                         "(\r)( )+(\r)", "\r\r",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         "(\t)( )+(\t)", "\t\t",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         "(\t)( )+(\r)", "\t\r",
                         RegexOptions.IgnoreCase);
                result = Regex.Replace(result,
                         "(\r)( )+(\t)", "\r\t",
                         RegexOptions.IgnoreCase);
                // Remove redundant tabs
                result = Regex.Replace(result,
                         "(\r)(\t)+(\r)", "\r\r",
                         RegexOptions.IgnoreCase);
                // Remove multiple tabs following a line break with just one tab
                result = Regex.Replace(result,
                         "(\r)(\t)+", "\r\t",
                         RegexOptions.IgnoreCase);
                // Initial replacement target string for line breaks
                string breaks = "\r\r\r";
                // Initial replacement target string for tabs
                string tabs = "\t\t\t\t\t";
                for (int index = 0; index < result.Length; index++)
                {
                    result = result.Replace(breaks, "\r\r");
                    result = result.Replace(tabs, "\t\t\t\t");
                    breaks = breaks + "\r";
                    tabs = tabs + "\t";
                }

                // That's it.
                return result;
            }
            catch (Exception)
            {
                return source;
            }
        }

        public static string GetSubDomain(HttpContext context)
        {
            var domainRequested = context.Request.Url.Host;
            var startIndexOfDomain = domainRequested.IndexOf("." + context.Request.Url.Host, StringComparison.Ordinal);

            if (startIndexOfDomain == -1) return string.Empty;

            var subDomain = domainRequested.Substring(0, startIndexOfDomain);
            return subDomain;
        }

        public static string GetDomain(HttpContext context)
        {
            return context.Request.Url.Host;
        }

        public static string GetFullDomain(HttpContext context)
        {
            var uri = new Uri(context.Request.Url.AbsoluteUri);
            return $"{uri.Scheme}://{uri.Authority}";
        }
    }
}
