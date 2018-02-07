namespace Eagle.Data.DefaultData
{
    public class HtmlConstants
    {
        #region HTML_CONTENT_TEMPLATE1

        public static string HTML_CONTENT_TEMPLATE1 = @"<!DOCTYPE html>
            <html>
            <head>
                  <link rel=""icon"" href=""/favicon.png"" >
                <link rel=""apple-touch-icon"" href=""/favicon.png"">
                <meta charset=""utf-8""/>
                  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                     <title>{{Page_Title}} - {{WebsiteName}}</title>
                {#cssFiles#}
                {{css}}
            </head>
            <body>
                {%HeaderModule%}
                <div class=""wrapper page-home pre-login"">
                    {%HighLightModule%}
                    <div class=""block-favourite"">
                        <div class=""container"">
                            {%RichText%}
                            {%TilesModule%}
                        </div>
                    </div>
                </div>

                <div class=""modal fade apr-modal"" id=""modal-timeout"" tabindex=""-1"" role=""dialog"" aria-labelledby=""modalSigninLable"" data-backdrop=""static"" data-keyboard=""false"" aria-hidden=""true"">
                <div class=""modal-dialog"" role=""document"">
                  <div class=""apr-modal-top"">
                    <div class=""modal-head text-center"">
                      <div class=""modal-title"">Your Session</div>
                      <div class=""modal-headline"">Inactive Session</div>
                    </div>
                    <img src=""/ClientSites/global/theme/images/signin-cover-bg.jpg"" alt="""" class=""img-responsive fluid-width"">
                    </div>
                    <div class=""modal-content"">
                      <div class=""modal-body"">
                        <form action=""index-logged-in.html"" id=""signInForm"" class=""apr-form"" method=""post"">
                          <div class=""row pdt-10 text-center"">
                            <b>You are about to be logged out.</b>
                          </div>
                          <div class=""row timeout-text pdt-10 text-center"">
                            You have been inactive for a while. For your security, we will log you out in approximately 1 minute. Click Continue to remain signed in.
                          </div>
                          <div class=""row pdt-10 text-center"">
                            <button id=""btnSessionContinue"" type=""button"" data-dismiss=""modal"" class=""btn btn-apr btn-primary"">CONTINUE</button>
                          </div>
                        </form>
                      </div>
                    </div>
                  </div>
                </div>
                <script type=""text/javascript"">
                    var SESSION_TIMEOUT_MINUTES = @System.Web.HttpContext.Current.Session.Timeout;
                </script>

                {%FooterModule%}
                {#jsFiles#}
                {#gaTrackingCode#}
                {{js}}
            </body>
            </html>";

        #endregion

        #region HTML_STATIC_TEMPLATE

        public static string HTML_STATIC_TEMPLATE = @"<!DOCTYPE html>
            <html>
            <head>
                <link rel=""icon"" href=""/favicon.png"" >
                <link rel=""apple-touch-icon"" href=""/favicon.png"">
                <meta charset=""utf-8""/>
                  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                     <title>{{Page_Title}} - {{WebsiteName}}</title>
                {#cssFiles#}
                {{css}}
            </head>
            <body>
                {%HeaderModule%}
                <div class=""wrapper page-faq"">
                <div class=""header-gap-holder hidden-xs""></div>
                {%Breadcrumb%}
                {%RichText%}
                {%TilesModule%}
                </div>
                <div class=""modal fade apr-modal"" id=""modal-timeout"" tabindex=""-1"" role=""dialog"" aria-labelledby=""modalSigninLable"" data-backdrop=""static"" data-keyboard=""false"" aria-hidden=""true"">
                <div class=""modal-dialog"" role=""document"">
                  <div class=""apr-modal-top"">
                    <div class=""modal-head text-center"">
                      <div class=""modal-title"">Your Session</div>
                      <div class=""modal-headline"">Inactive Session</div>
                    </div>
                    <img src=""/ClientSites/global/theme/images/signin-cover-bg.jpg"" alt="""" class=""img-responsive fluid-width"">
                    </div>
                    <div class=""modal-content"">
                      <div class=""modal-body"">
                        <form action=""index-logged-in.html"" id=""signInForm"" class=""apr-form"" method=""post"">
                          <div class=""row pdt-10 text-center"">
                            <b>You are about to be logged out.</b>
                          </div>
                          <div class=""row timeout-text pdt-10 text-center"">
                            You have been inactive for a while. For your security, we will log you out in approximately 1 minute. Click Continue to remain signed in.
                          </div>
                          <div class=""row pdt-10 text-center"">
                            <button id=""btnSessionContinue"" type=""button"" data-dismiss=""modal"" class=""btn btn-apr btn-primary"">CONTINUE</button>
                          </div>
                        </form>
                      </div>
                    </div>
                  </div>
                </div>
                <script type=""text/javascript"">
                    var SESSION_TIMEOUT_MINUTES = @System.Web.HttpContext.Current.Session.Timeout;
                </script>

                {%FooterModule%}
                {#jsFiles#}
                {#gaTrackingCode#}
                {{js}}
            </body>
            </html>";

        #endregion

        #region HTML_CONTENT_TEMPLATE2

        public static string HTML_CONTENT_TEMPLATE2 = @"
                <!DOCTYPE html>
                <html>
            <head>
                <link rel=""icon"" href=""/favicon.png"" >
                <link rel=""apple-touch-icon"" href=""/favicon.png"">
                <meta charset=""utf-8""/>
                  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                     <title>{{Page_Title}} - {{WebsiteName}}</title>
                {#cssFiles#}
                {{css}}
            </head>
            <body>
                {%HeaderModule%}
                <div class=""wrapper page-home pre-login"">
                    {%HighLightModule%}
                    {%Breadcrumb%}
                    <div class=""block-favourite"">
                        <div class=""container"">
                            {%RichText%}
                            {%TilesModule%}
                        </div>
                    </div>
                </div>
                <hr/>
                {%MapModule%}
                {%PromoterScoreFormModule%}
                <div>
                {%FreeFormModule%}
                </div>
                {%FooterModule%}
                {#jsFiles#}
                {#gaTrackingCode#}
                {{js}}
                @{
                    Html.AddStyle(""/ClientSites/global/modules/style/jquery-ui.css"");
                    Html.AddStyle(""/ClientSites/global/modules/style/style.css"");
                }
                {#AddModuleCSSBlock#}
                {#AddModuleScriptBlock#}
            </body>
            </html>";

        #endregion

        #region HTML_CONTENT_TEMPLATE1

        public static string HTML_CONTENT_SessionTimeoutTEMPLATE = @"
<!DOCTYPE html>
<html>
<head>
    <link rel=""icon"" href=""/favicon.png"" >
    <link rel=""apple-touch-icon"" href=""/favicon.png"">
    <meta charset=""utf-8""/>
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>{{Page_Title}} - {{WebsiteName}}</title>
    {#cssFiles#}
    {{css}}
</head>
<body>
    <div class=""header"">
        <a href = ""javscript:void(0)"" class=""btn-nav"">
            <span>toggle menu</span>
        </a>
        <div class=""top-nav"">
            <div class=""container"">
            <a class=""logo-aspire"" href=""http://ubs.com"">
                <img class=""logo-header"" src=""/ClientSites/global/theme/images/ubs_logo.svg"" alt=""Aspire"">
                <span>UBS Concierge</span>
            </a>
            </div>
        </div>
    </div>

    {%RichText%}

    <div class=""page-overlay""></div>
    <div class=""lt-overlay""></div>
    {#jsFiles#}
    {#gaTrackingCode#}
    {{js}}
</body>
</html>";

        #endregion

        #region HTML_CONTENT_TEMPLATE_DINING

        public static string HTML_CONTENT_TEMPLATE_DINING = @"
                <!DOCTYPE html>
                <html>
            <head>
                <link rel=""icon"" href=""/favicon.png"" >
                <link rel=""apple-touch-icon"" href=""/favicon.png"">
                <meta charset=""utf-8""/>
                  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                     <title>{{Page_Title}} - {{WebsiteName}}</title>
                {#cssFiles#}
                {{css}}
            </head>
            <body>
                {%HeaderModule%}
                <div class=""wrapper page-home pre-login"">
                    <div class=""block-profile"">
                        <div class=""container"">
                            {%DiningFormModule%}
                        </div>
                    </div>
                </div>
                <script type=""text/javascript"">
                    var SESSION_TIMEOUT_MINUTES = @System.Web.HttpContext.Current.Session.Timeout;
                </script>
                {%FooterModule%}
                {#jsFiles#}
                {#gaTrackingCode#}
                {{js}}
                {#AddModuleCSSBlock#}
                {#AddModuleScriptBlock#}
            </body>
            </html>";

        #endregion

        #region HTML_CONTENT_TEMPLATE_TRAVEL_CRUISE

        public static string HTML_CONTENT_TEMPLATE_TRAVEL_CRUISE = @"
                <!DOCTYPE html>
                <html>
            <head>
                <link rel=""icon"" href=""/favicon.png"" >
                <link rel=""apple-touch-icon"" href=""/favicon.png"">
                <meta charset=""utf-8""/>
                  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                     <title>{{Page_Title}} - {{WebsiteName}}</title>
                {#cssFiles#}
                {{css}}
            </head>
            <body>
                {%HeaderModule%}
                <div class=""wrapper page-home pre-login"">
                    <div class=""block-profile"">
                        <div class=""container"">
                            {%TravelCruiseFormModule%}
                        </div>
                    </div>
                </div>
                <script type=""text/javascript"">
                    var SESSION_TIMEOUT_MINUTES = @System.Web.HttpContext.Current.Session.Timeout;
                </script>
                {%FooterModule%}
                {#jsFiles#}
                {#gaTrackingCode#}
                {{js}}
                {#AddModuleCSSBlock#}
                {#AddModuleScriptBlock#}
            </body>
            </html>";

        #endregion

        #region HTML_CONTENT_TEMPLATE_TRAVEL_FLIGHT

        public static string HTML_CONTENT_TEMPLATE_TRAVEL_FLIGHT = @"
                <!DOCTYPE html>
                <html>
            <head>
                <link rel=""icon"" href=""/favicon.png"" >
                <link rel=""apple-touch-icon"" href=""/favicon.png"">
                <meta charset=""utf-8""/>
                  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                     <title>{{Page_Title}} - {{WebsiteName}}</title>
                {#cssFiles#}
                {{css}}
            </head>
            <body>
                {%HeaderModule%}
                <div class=""wrapper page-home pre-login"">
                    <div class=""block-profile"">
                        <div class=""container"">
                            {%TravelFlightFormModule%}
                        </div>
                    </div>
                </div>
                <script type=""text/javascript"">
                    var SESSION_TIMEOUT_MINUTES = @System.Web.HttpContext.Current.Session.Timeout;
                </script>
                {%FooterModule%}
                {#jsFiles#}
                {#gaTrackingCode#}
                {{js}}
                {#AddModuleCSSBlock#}
                {#AddModuleScriptBlock#}
            </body>
            </html>";

        #endregion

        #region HTML_CONTENT_TEMPLATE_TRAVEL_ACCOMMODATION

        public static string HTML_CONTENT_TEMPLATE_TRAVEL_ACCOMMODATION = @"
                <!DOCTYPE html>
                <html>
            <head>
                <link rel=""icon"" href=""/favicon.png"" >
                <link rel=""apple-touch-icon"" href=""/favicon.png"">
                <meta charset=""utf-8""/>
                  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                     <title>{{Page_Title}} - {{WebsiteName}}</title>
                {#cssFiles#}
                {{css}}
            </head>
            <body>
                {%HeaderModule%}
                <div class=""wrapper page-home pre-login"">
                    <div class=""block-profile"">
                        <div class=""container"">
                            {%TravelAccommodationFormModule%}
                        </div>
                    </div>
                </div>
                <script type=""text/javascript"">
                    var SESSION_TIMEOUT_MINUTES = @System.Web.HttpContext.Current.Session.Timeout;
                </script>
                {%FooterModule%}
                {#jsFiles#}
                {#gaTrackingCode#}
                {{js}}
                {#AddModuleCSSBlock#}
                {#AddModuleScriptBlock#}
            </body>
            </html>";

        #endregion

        #region HTML_CONTENT_TEMPLATE_OTHER_REQUEST_FORM

        public static string HTML_CONTENT_TEMPLATE_OTHER_REQUEST_FORM = @"
                <!DOCTYPE html>
                <html>
            <head>
                <link rel=""icon"" href=""/favicon.png"" >
                <link rel=""apple-touch-icon"" href=""/favicon.png"">
                <meta charset=""utf-8""/>
                  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                     <title>{{Page_Title}} - {{WebsiteName}}</title>
                {#cssFiles#}
                {{css}}
            </head>
            <body>
                {%HeaderModule%}
                <div class=""wrapper page-home pre-login"">
                    <div class=""block-profile"">
                        <div class=""container"">
                            {%OtherRequestFormModule%}
                        </div>
                    </div>
                </div>
                <script type=""text/javascript"">
                    var SESSION_TIMEOUT_MINUTES = @System.Web.HttpContext.Current.Session.Timeout;
                </script>
                {%FooterModule%}
                {#jsFiles#}
                {#gaTrackingCode#}
                {{js}}
                {#AddModuleCSSBlock#}
                {#AddModuleScriptBlock#}
            </body>
            </html>";

        #endregion

        #region HTML_CONTENT_TEMPLATE_TRANSPORTATION_FORM

        public static string HTML_CONTENT_TEMPLATE_TRANSPORTATION_FORM = @"
                <!DOCTYPE html>
                <html>
            <head>
                <link rel=""icon"" href=""/favicon.png"" >
                <link rel=""apple-touch-icon"" href=""/favicon.png"">
                <meta charset=""utf-8""/>
                  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                     <title>{{Page_Title}} - {{WebsiteName}}</title>
                {#cssFiles#}
                {{css}}
            </head>
            <body>
                {%HeaderModule%}
                <div class=""wrapper page-home pre-login"">
                    <div class=""block-profile"">
                        <div class=""container"">
                            {%TransportationFormModule%}
                        </div>
                    </div>
                </div>
                <script type=""text/javascript"">
                    var SESSION_TIMEOUT_MINUTES = @System.Web.HttpContext.Current.Session.Timeout;
                </script>
                {%FooterModule%}
                {#jsFiles#}
                {#gaTrackingCode#}
                {{js}}
                {#AddModuleCSSBlock#}
                {#AddModuleScriptBlock#}
            </body>
            </html>";

        #endregion

        #region HTML_CONTENT_TEMPLATE_LIFE_STYLE_GOLF_FORM

        public static string HTML_CONTENT_TEMPLATE_LIFE_STYLE_GOLF_FORM = @"
                <!DOCTYPE html>
                <html>
            <head>
                <link rel=""icon"" href=""/favicon.png"" >
                <link rel=""apple-touch-icon"" href=""/favicon.png"">
                <meta charset=""utf-8""/>
                  <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                     <title>{{Page_Title}} - {{WebsiteName}}</title>
                {#cssFiles#}
                {{css}}
            </head>
            <body>
                {%HeaderModule%}
                <div class=""wrapper page-home pre-login"">
                    <div class=""block-profile"">
                        <div class=""container"">
                            {%LifeStyleGolfFormModule%}
                        </div>
                    </div>
                </div>
                <script type=""text/javascript"">
                    var SESSION_TIMEOUT_MINUTES = @System.Web.HttpContext.Current.Session.Timeout;
                </script>
                {%FooterModule%}
                {#jsFiles#}
                {#gaTrackingCode#}
                {{js}}
                {#AddModuleCSSBlock#}
                {#AddModuleScriptBlock#}
            </body>
            </html>";

        #endregion

        //#region HTML_CONTENT_PROMOTER_SCORE_FORM

        //public static string HTML_CONTENT_PROMOTER_SCORE_FORM = "@{var formData = Html.JsonObject(ViewData[\"" + Constants.PROMOTER_SCORE_FORM_MODULE_ALIAS + "\"]); }"
        //     + @"<div class=""container"">
        //        <form id = ""promoterScoreForm"" method=""POST"" action=""FormModule/PromoterScoreFormAction"" class=""@Html.JsonFieldVal(formData, ""FormClass"")"">
        //          <div class=""row"">
        //            <div class=""col-md-12 block-terms-list"">
        //              <div class=""content-book"">
        //                <h3>@Html.JsonFieldVal(formData, ""Title"")</h3><hr/>
        //                <p>@Html.JsonFieldVal(formData, ""Description"")</p>
        //                <div class=""item-group"">
        //                    @Html.ParseDynamicFormGroups(Html.JsonFieldVal(formData, ""FormGroups""))
        //                </div>
        //              </div>
        //            </div>
        //          </div>
        //          <div class=""row col-sm-12 text-center"">
        //                <input type = ""submit"" value=""Submit"" class=""btn btn-red top-gap"" style=""height:40px; margin-top:10px; width:150px""/>
        //          </div>
        //        </form>
        //      </div>";
        //#endregion

//        #region HTML_CONTENT_FREE_FORM

//        public static string HTML_CONTENT_FREE_FORM = "@{var formData = Html.JsonObject(ViewData[\"" + Constants.FREE_FORM_MODULE_ALIAS + "\"]); }"
//             + @"<div class=""container"">
//                <form id = ""freelForm"" method=""POST"" action=""Admin/PageModules/FreeFormAction"" class=""@Html.JsonFieldVal(formData, ""FormClass"")"">
//                  <div class=""row"">
//                    <div class=""col-md-12"">
//                      <div class=""content-book"">
//                        <h3>@Html.JsonFieldVal(formData, ""Title"")</h3>
//                        <p>@Html.JsonFieldVal(formData, ""Description"")</p>
//                        <div class=""item-group"">
//                            @Html.ParseDynamicFormGroups(Html.JsonFieldVal(formData, ""FormGroups""))
//                        </div>
//                      </div>
//                    </div>
//                  </div>
//                  <div class=""row"">
//                        <input type=""submit"" value=""Submit"" class=""btn btn-primary""/>
//                  </div>
//                </form>
//              </div>
//               @{
//                    Html.AddStyle(""http://netdna.bootstrapcdn.com/bootstrap/3.1.1/css/bootstrap.min.css"");
//                    Html.AddJavaScript(""http://maxcdn.bootstrapcdn.com/bootstrap/3.1.1/js/bootstrap.min.js"");
//                }

//            ";
//        #endregion

//        #region HTML_CONTENT_DINING_FORM
//        public static string HTML_CONTENT_DINING_FORM
//        {
//            get
//            {
//                return "@{var formData = Html.JsonObject(ViewData[\"" + Constants.DINING_FORM_MODULE_ALIAS + "\"]); }"
//            + @"<div class=""container"">
//                <form id = ""diningForm"" method=""POST"" action=""/FormModule_Submit"" class=""fr-booking"">
//                  <div class=""row"">
//                    <div class=""col-md-9"">
//                      <div class=""content-book"">
//                        <h3>@Html.JsonFieldVal(formData, ""Title"")</h3>
//                        <p>@Html.JsonFieldVal(formData, ""Description"")</p>
//                        <label class=""col-gray""><span class=""txt-red"">*</span> indicates mandatory</label>
//                            <div class=""row"">
//                                <a href = ""#"" class=""btn-clearall"" data-spy=""btnClear"">Clear all</a>
//                            </div>
//                         <div class=""item-group"">
//                            @Html.ParseFormModule(formData, TempData[""FormDataModel""])
//                        </div>
//                      </div>
//                    <div class=""col-md-3 col-sm-12"">
//                        <div class=""info-book"" sticky-on=""md, lg"" sticky-top-md=""25""sticky-top-lg=""108"">
//                        <div class=""title"">
//                            <div class=""sprites icon-printer-red hidden-sm hidden-xs""></div>
//                            selection summary
//                        </div>
//                        <div class=""content"">
//                            <div class=""item"">
//                            <p>
//                                <i class=""sprites icon-maps""></i>
//                                <span class=""infoRestaurant""></span>
//                                <span class=""infoAddressRestaurant""></span>
//                            </p>

//                            <p>
//                                <i class=""sprites icon-calendar""></i>
//                                <span class=""infoDate""></span>
//                            </p>
//                            </div>
//                            <div class=""item"">
//                            <p>
//                                <i class=""sprites icon-time""></i>
//                                <span class=""infoTime""></span>
//                            </p>
//                            <p>
//                                <i class=""sprites icon-user""></i>
//                                <span class=""infoAdults"">1 adult</span>, <span class=""infoKids"">0 kid</span>
//                            </p>
//                            </div>
//                        </div>
//                        <button type=""submit"" class=""btn btn-red"">book now</button>
//                        </div>
//                    </div>
//                    </div>
//                  </div>
//                  <input type=""hidden"" value=""@formData[""PageSettingId""]"" name=""pageModuleSettingId""/>
//                  <input type=""hidden"" value=""@ViewData[""PagePath""]"" name=""PagePath""/>
//                </form>
//              </div>
//            ";
//            }
//        }
//        #endregion
        
//        #region PERSONAL_INFO_FORM
//        public static string PERSONAL_INFO_FORM = @"
//				<div class=""item-group"">
//                  <div class=""row"">
//                    <div class=""col-md-12"">
//                      <div class=""title"">
//                        Personal Information
//                      </div>
//                    </div>
//                  </div>
//                  <div class=""row"">
//                    <div class=""col-md-6 col-sm-5 col-xs-12"">
//                      <div class=""form-group"">
//                        <label for="""">Guest Name<span class=""txt-red""> *</span></label>
//                        <div class=""box-group"">
//                          <label class=""input-group-before"" for="""">
//                            <span class=""ico ico-name""></span>
//                          </label>
//                          <input type=""text"" class=""form-control"" name=""GuestName"" id=""GuestName"" placeholder=""How should we address you?"" required>
//                        </div>
//                      </div>
//                    </div>
//                  </div>
//                  <div class=""row"">
//                    <div class=""col-md-6 col-sm-12 col-xs-12"">
//                      <div class=""mb-5"">
//                        <label for="""" class=""mb-5"">I would like to be contacted via<span class=""txt-red""> *</span></label>
//						<br/>
//                        <div class=""row"">
//                        </div>
//                      </div>
//                    </div>
//                  </div>
//                  <div class=""row"">
//                    <div class=""col-md-6 col-sm-5 col-xs-12"">
//                      <div class=""form-group"">
//                        <label for="""">Mobile Number <span class=""txt-red"">*</span></label>
//                        <div class=""box-group-checkbox"">
//                        <div class=""checkbox-member"">
//                        <input type=""checkbox"" value=""true"" id=""prefResponseViaMobile"" name=""PrefResponseViaMobile"" data-error=""contact-via-error"" checked/>
//                        <label for=""moblie""></label>
//                        </div>
//                        <div class=""box-group select-contry"">
//                          <div class=""input-group-before"">
//                            <span class=""ico ico-globe""></span>
//                          </div>
//                          <select class=""form-control"" id=""NameOfCountry"" data-spy=""singleSelectBox"" data-placeholder=""Country"" name=""NameOfCountry"" required>
//                            <option value="""">Please select</option>
//                            <option value=""1"">Country 1</option>
//                            <option value=""2"">Country 2</option>
//                            <option value=""3"">Country 3</option>
//                            <option value=""4"">Country 4</option>
//                            <option value=""5"">Country 5</option>
//                            <option value=""6"">Country 6</option>
//                          </select>
//                        </div>
//                        <div class=""box-group input-phone"">
//                          <div class=""input-group-before"">
//                            <span class=""ico ico-contact""></span>
//                          </div>
//                          <input type=""text"" class=""form-control"" name=""MobileNumber"" id=""MobileNumber"" placeholder=""Please enter a valid number"" value=""+84 8 3547 1411"" required>
//                        </div>
//						</div>
//                      </div>
//                    </div>
//                    <div class=""col-md-6 hidden-sm hidden-xs col-mask"">
//                      <div class=""form-group mb-25"">
//                        <label for=""select-name"">Mask</label>
//                        <div class=""box-group"" id=""div_masked_input"">
//                          <label class=""input-group-before"" for="""">
//                            <span class=""sprites icon-mail""></span>
//                          </label>
//                          <input type=""text"" class=""form-control""  aria-label=""Masked-Input"" aria-labelledby=""div_masked_input"">
//                        </div>
//                      </div>
//                    </div>
//                   <div class=""col-md-6 col-sm-5 col-xs-12"">
//                      <div class=""form-group"">
//                        <label for="""">Email Address <span class=""txt-red"">*</span></label>
//                        <div class=""box-group-checkbox"">
//                        <div class=""checkbox-member"">
//                        <input type=""checkbox"" value=""true"" name=""PrefResponseViaEmail"" id=""prefResponseViaEmail"" data-error=""contact-via-error"" checked/>
//                        <label for=""email""></label>
//                        </div>
//                        <div class=""box-group"">
//                          <div class=""input-group-before"">
//                            <span class=""ico ico-email""></span>
//                          </div>
//                          <input type=""text"" class=""form-control"" name=""EmailAddress"" id=""EmailAddress"" placeholder=""Please enter a valid email"" required>
//                        </div>
//						</div>
//                      </div>
//                    </div>
//                  </div>
//                </div>
//";

//        #endregion

//        #region PERSONAL_INFOR_FORM_TEMPLATE
//        public static string PERSONAL_INFOR_FORM_TEMPLATE = @"
//            </div><div class=""item-group"">
//              <div class=""row"">
//	            <div class=""col-md-12"">
//	              <div class=""title"">
//		            {0}
//	              </div>
//	            </div>
//              </div>
//              <div class=""row"">
//	            <div class=""col-md-6 col-sm-5 col-xs-12"">
//	              <div class=""form-group"">
//		            <label for="""">{1} <span class=""txt-red"">*</span></label>
//		            <div class=""box-group"">
//		              <label class=""input-group-before"" for="""">
//			            <span class=""sprites icon-user""></span>
//		              </label>
//		              {2}
//		            </div>
//	              </div>
//	            </div>
//              </div>
//              <div class=""row group-checkbox-choice box-contact"">
//	            <div class=""form-group"">
//	              <label for="""" class=""mb-20"">{3} <span class=""txt-red"">*</span></label>
//	            </div>
//                <div class=""contact-phone clearfix mb-20"">
//	                <div class=""col-md-6 col-sm-5 col-xs-12"">
//	                  <div class=""form-group"">
//		                <div class=""mb-5"">
//		                  <label for="""">{4}</label>
//		                </div>
//		                <div class=""col-md-2 col-sm-2 col-xs-2"">
//		                  <div class=""box-group"">
//                            {5}
//			                <label for=""cbxMobile""></label>
//		                  </div>
//		                </div>
//		                <div class=""col-md-10 col-sm-10 col-xs-10 phone-field"">
//		                  <div class=""col-md-4 col-sm-4 col-xs-4"">
//			                <div class=""form-group"">
//			                  <div class=""box-group"">
//				                {6}
//				                <label class=""input-group-after"" for="""">
//				                  <span class=""sprites icon-arrow-black""></span>
//				                </label>
//			                  </div>
//			                </div>
//		                  </div>
//		                  <div class=""col-md-8 col-sm-8 col-xs-8"">
//			                <div class=""form-group"">
//			                  <div class=""box-group box-mobile"">
//				                {7}
//			                  </div>
//			                </div>
//		                  </div>
//		                </div>
//	                  </div>
//	                </div>
//                </div>
//                <div class=""contact-email clearfix"">
//	                <div class=""col-md-6 col-sm-5 col-xs-12"">
//	                  <div class=""form-group"">
//		                <div class=""mb-5"">
//		                  <label for="""">{8}</label>
//		                </div>
//		                <div class=""col-md-2 col-sm-2 col-xs-2"">
//		                  <div class=""box-group"">
//                            {9}
//			                <label for=""cbxEmail""></label>
//		                  </div>
//		                </div>
//		                <div class=""col-md-10 col-sm-10 col-xs-10"">
//		                  <div class=""form-group"">
//			                <div class=""box-group"">
//			                  {10}
//			                </div>
//		                  </div>
//		                </div>
//	                  </div>
//	                </div>
//                </div>
//              </div>
//            </div>";
//        #endregion

//        #region HTML_CONTENT_TRAVEL_CRUISE_FORM
//        public static string HTML_CONTENT_TRAVEL_CRUISE_FORM
//        {
//            get
//            {
//                return "@{var formData = Html.JsonObject(ViewData[\"" + Constants.TRAVEL_CRUISE_FORM_MODULE_ALIAS + "\"]); }"
//            + @"<div class=""container"">
//                <form id = ""diningForm"" method=""POST"" action=""FormModule_Submit"" class=""fr-booking"">
//                  <div class=""row"">
//                    <div class=""col-md-9"">
//                      <div class=""content-book"">
//                        <h3>@Html.JsonFieldVal(formData, ""Title"")</h3>
//                        <p>@Html.JsonFieldVal(formData, ""Description"")</p>
//                        <div class=""item-group"">
//                            <div class=""row"">
//                                <a href = ""#"" class=""btn-clearall"" data-spy=""btnClear"">Clear all</a>
//                            </div>
//                            @Html.ParseFormModule(formData)
//                        </div>
//                      </div>
//                    <div class=""col-md-3 col-sm-12"">
//                        <div class=""info-book"" sticky-on=""md, lg"" sticky-top-md=""25""sticky-top-lg=""108"">
//                        <div class=""title"">
//                            <div class=""sprites icon-printer-red hidden-sm hidden-xs""></div>
//                            selection summary
//                        </div>
//                        <div class=""content"">
//                            <div class=""item"">
//                            <p>
//                                <i class=""sprites icon-maps""></i>
//                                <span class=""infoRestaurant""></span>
//                                <span class=""infoAddressRestaurant""></span>
//                            </p>

//                            <p>
//                                <i class=""sprites icon-calendar""></i>
//                                <span class=""infoDate""></span>
//                            </p>
//                            </div>
//                            <div class=""item"">
//                            <p>
//                                <i class=""sprites icon-time""></i>
//                                <span class=""infoTime""></span>
//                            </p>
//                            <p>
//                                <i class=""sprites icon-user""></i>
//                                <span class=""infoAdults"">1 adult</span>, <span class=""infoKids"">0 kid</span>
//                            </p>
//                            </div>
//                        </div>
//                        <button type=""submit"" class=""btn btn-red"">book now</button>
//                        </div>
//                    </div>
//                    </div>
//                  </div>
//                  <input type=""hidden"" value=""@formData[""PageSettingId""]"" name=""pageModuleSettingId""/>
//                  <input type=""hidden"" value=""@ViewData[""PagePath""]"" name=""PagePath""/>
//                </form>
//              </div>
//            ";
//            }
//        }
//        #endregion

//        #region HTML_CONTENT_TRAVEL_ACCOMMODATION_FORM
//        public static string HTML_CONTENT_TRAVEL_ACCOMMODATION_FORM
//        {
//            get
//            {
//                return "@{var formData = Html.JsonObject(ViewData[\"" + Constants.TRAVEL_ACCOMMODATION_MODULE_ALIAS + "\"]); }"
//            + @"<div class=""container"">
//                <form id = ""diningForm"" method=""POST"" action=""FormModule_Submit"" class=""fr-booking"">
//                  <div class=""row"">
//                    <div class=""col-md-9"">
//                      <div class=""content-book"">
//                        <h3>@Html.JsonFieldVal(formData, ""Title"")</h3>
//                        <p>@Html.JsonFieldVal(formData, ""Description"")</p>
//                        <div class=""item-group"">
//                            <div class=""row"">
//                                <a href = ""#"" class=""btn-clearall"" data-spy=""btnClear"">Clear all</a>
//                            </div>
//                            @Html.ParseFormModule(formData)
//                        </div>
//                      </div>
//                    <div class=""col-md-3 col-sm-12"">
//                        <div class=""info-book"" sticky-on=""md, lg"" sticky-top-md=""25""sticky-top-lg=""108"">
//                        <div class=""title"">
//                            <div class=""sprites icon-printer-red hidden-sm hidden-xs""></div>
//                            selection summary
//                        </div>
//                        <div class=""content"">
//                            <div class=""item"">
//                            <p>
//                                <i class=""sprites icon-maps""></i>
//                                <span class=""infoRestaurant""></span>
//                                <span class=""infoAddressRestaurant""></span>
//                            </p>

//                            <p>
//                                <i class=""sprites icon-calendar""></i>
//                                <span class=""infoDate""></span>
//                            </p>
//                            </div>
//                            <div class=""item"">
//                            <p>
//                                <i class=""sprites icon-time""></i>
//                                <span class=""infoTime""></span>
//                            </p>
//                            <p>
//                                <i class=""sprites icon-user""></i>
//                                <span class=""infoAdults"">1 adult</span>, <span class=""infoKids"">0 kid</span>
//                            </p>
//                            </div>
//                        </div>
//                        <button type=""submit"" class=""btn btn-red"">book now</button>
//                        </div>
//                    </div>
//                    </div>
//                  </div>
//                  <input type=""hidden"" value=""@formData[""PageSettingId""]"" name=""pageModuleSettingId""/>
//                  <input type=""hidden"" value=""@ViewData[""PagePath""]"" name=""PagePath""/>
//                </form>
//              </div>
//            ";
//            }
//        }
//        #endregion

//        #region HTML_CONTENT_OTHER_REQUEST_FORM
//        public static string HTML_CONTENT_OTHER_REQUEST_FORM
//        {
//            get
//            {
//                return "@{var formData = Html.JsonObject(ViewData[\"" + Constants.OTHER_REQUEST_MODULE_ALIAS + "\"]); }"
//            + @"<div class=""container"">
//                <form id = ""diningForm"" method=""POST"" action=""/FormModule_Submit"" class=""fr-booking page-book-tours"">
//                  <div class=""row"">
//	                <div class=""col-md-9"">
//	                  <div class=""content-book"">
//		                <h3>@Html.JsonFieldVal(formData, ""Title"")</h3>
//		                <p>@Html.JsonFieldVal(formData, ""Description"")</p>
//		                <div class=""item-group"">
//		                  @Html.ParseFormModule(formData, TempData[""FormDataModel""])
//		                <div class=""row"">
//		                  <div class=""btn-book"">
//                               <button type=""submit"" id=""btnBook"" name=""btnBook"" class=""btn btn-red"">Place Request Now</button>                               
//		                  </div>
//		                </div>
//	                  </div>
//	                </div>
//                  </div>
//                  <input type=""hidden"" value=""@formData[""PageSettingId""]"" name=""pageModuleSettingId""/>
//                  <input type=""hidden"" value=""@ViewData[""PagePath""]"" name=""PagePath""/>
//                </form>
//              </div>
//            ";
//            }
//        }
//        #endregion

//        #region HTML_CONTENT_TRANSPORTATION_FORM
//        public static string HTML_CONTENT_TRANSPORTATION_FORM
//        {
//            get
//            {
//                return "@{var formData = Html.JsonObject(ViewData[\"" + Constants.TRANSPORTATION_CAR_RENTAL_MODULE_ALIAS + "\"]); }"
//            + @"<div class=""container"">
//                <form id = ""diningForm"" method=""POST"" action=""FormModule_Submit"" class=""fr-booking"">
//                  <div class=""row"">
//                    <div class=""col-md-9"">
//                      <div class=""content-book"">
//                        <h3>@Html.JsonFieldVal(formData, ""Title"")</h3>
//                        <p>@Html.JsonFieldVal(formData, ""Description"")</p>
//                        <div class=""item-group"">
//                            <div class=""row"">
//                                <a href = ""#"" class=""btn-clearall"" data-spy=""btnClear"">Clear all</a>
//                            </div>
//                            @Html.ParseFormModule(formData)
//                        </div>
//                      </div>
//                    <div class=""col-md-3 col-sm-12"">
//                        <div class=""info-book"" sticky-on=""md, lg"" sticky-top-md=""25""sticky-top-lg=""108"">
//                        <div class=""title"">
//                            <div class=""sprites icon-printer-red hidden-sm hidden-xs""></div>
//                            selection summary
//                        </div>
//                        <div class=""content"">
//                            <div class=""item"">
//                            <p>
//                                <i class=""sprites icon-maps""></i>
//                                <span class=""infoRestaurant""></span>
//                                <span class=""infoAddressRestaurant""></span>
//                            </p>

//                            <p>
//                                <i class=""sprites icon-calendar""></i>
//                                <span class=""infoDate""></span>
//                            </p>
//                            </div>
//                            <div class=""item"">
//                            <p>
//                                <i class=""sprites icon-time""></i>
//                                <span class=""infoTime""></span>
//                            </p>
//                            <p>
//                                <i class=""sprites icon-user""></i>
//                                <span class=""infoAdults"">1 adult</span>, <span class=""infoKids"">0 kid</span>
//                            </p>
//                            </div>
//                        </div>
//                        <button type=""submit"" class=""btn btn-red"">book now</button>
//                        </div>
//                    </div>
//                    </div>
//                  </div>
//                  <input type=""hidden"" value=""@formData[""PageSettingId""]"" name=""pageModuleSettingId""/>
//                  <input type=""hidden"" value=""@ViewData[""PagePath""]"" name=""PagePath""/>
//                </form>
//              </div>
//            ";
//            }
//        }
//        #endregion

//        #region HTML_CONTENT_LIFE_STYLE_GOLF_FORM
//        public static string HTML_CONTENT_LIFE_STYLE_GOLF_FORM
//        {
//            get
//            {
//                return "@{var formData = Html.JsonObject(ViewData[\"" + Constants.LIFE_STYLE_GOLF_MODULE_ALIAS + "\"]); }"
//            + @"<div class=""container"">
//                <form id = ""diningForm"" method=""POST"" action=""FormModule_Submit"" class=""fr-booking"">
//                  <div class=""row"">
//                    <div class=""col-md-9"">
//                      <div class=""content-book"">
//                        <h3>@Html.JsonFieldVal(formData, ""Title"")</h3>
//                        <p>@Html.JsonFieldVal(formData, ""Description"")</p>
//                        <div class=""item-group"">
//                            <div class=""row"">
//                                <a href = ""#"" class=""btn-clearall"" data-spy=""btnClear"">Clear all</a>
//                            </div>
//                            @Html.ParseFormModule(formData)
//                        </div>
//                      </div>
//                    <div class=""col-md-3 col-sm-12"">
//                        <div class=""info-book"" sticky-on=""md, lg"" sticky-top-md=""25""sticky-top-lg=""108"">
//                        <div class=""title"">
//                            <div class=""sprites icon-printer-red hidden-sm hidden-xs""></div>
//                            selection summary
//                        </div>
//                        <div class=""content"">
//                            <div class=""item"">
//                            <p>
//                                <i class=""sprites icon-maps""></i>
//                                <span class=""infoRestaurant""></span>
//                                <span class=""infoAddressRestaurant""></span>
//                            </p>

//                            <p>
//                                <i class=""sprites icon-calendar""></i>
//                                <span class=""infoDate""></span>
//                            </p>
//                            </div>
//                            <div class=""item"">
//                            <p>
//                                <i class=""sprites icon-time""></i>
//                                <span class=""infoTime""></span>
//                            </p>
//                            <p>
//                                <i class=""sprites icon-user""></i>
//                                <span class=""infoAdults"">1 adult</span>, <span class=""infoKids"">0 kid</span>
//                            </p>
//                            </div>
//                        </div>
//                        <button type=""submit"" class=""btn btn-red"">book now</button>
//                        </div>
//                    </div>
//                    </div>
//                  </div>
//                  <input type=""hidden"" value=""@formData[""PageSettingId""]"" name=""pageModuleSettingId""/>
//                  <input type=""hidden"" value=""@ViewData[""PagePath""]"" name=""PagePath""/>
//                </form>
//              </div>
//            ";
//            }
//        }
//        #endregion

//        #region HTML_CONTENT_TRAVEL_FLIGTH_FORM
//        public static string HTML_CONTENT_TRAVEL_FLIGTH_FORM
//        {
//            get
//            {
//                return "@{var formData = Html.JsonObject(ViewData[\"" + Constants.TRAVEL_FLIGTH_MODULE_ALIAS + "\"]); }"
//            + @"<div class=""container"">
//                <form id = ""diningForm"" method=""POST"" action=""FormModule_Submit"" class=""fr-booking"">
//                  <div class=""row"">
//                    <div class=""col-md-9"">
//                      <div class=""content-book"">
//                        <h3>@Html.JsonFieldVal(formData, ""Title"")</h3>
//                        <p>@Html.JsonFieldVal(formData, ""Description"")</p>
//                        <div class=""item-group"">
//                            <div class=""row"">
//                                <a href = ""#"" class=""btn-clearall"" data-spy=""btnClear"">Clear all</a>
//                            </div>
//                            @Html.ParseFormModule(formData)
//                        </div>
//                      </div>
//                    <div class=""col-md-3 col-sm-12"">
//                        <div class=""info-book"" sticky-on=""md, lg"" sticky-top-md=""25""sticky-top-lg=""108"">
//                        <div class=""title"">
//                            <div class=""sprites icon-printer-red hidden-sm hidden-xs""></div>
//                            selection summary
//                        </div>
//                        <div class=""content"">
//                            <div class=""item"">
//                            <p>
//                                <i class=""sprites icon-maps""></i>
//                                <span class=""infoRestaurant""></span>
//                                <span class=""infoAddressRestaurant""></span>
//                            </p>

//                            <p>
//                                <i class=""sprites icon-calendar""></i>
//                                <span class=""infoDate""></span>
//                            </p>
//                            </div>
//                            <div class=""item"">
//                            <p>
//                                <i class=""sprites icon-time""></i>
//                                <span class=""infoTime""></span>
//                            </p>
//                            <p>
//                                <i class=""sprites icon-user""></i>
//                                <span class=""infoAdults"">1 adult</span>, <span class=""infoKids"">0 kid</span>
//                            </p>
//                            </div>
//                        </div>
//                        <button type=""submit"" class=""btn btn-red"">book now</button>
//                        </div>
//                    </div>
//                    </div>
//                  </div>
//                  <input type=""hidden"" value=""@formData[""PageSettingId""]"" name=""pageModuleSettingId""/>
//                  <input type=""hidden"" value=""@ViewData[""PagePath""]"" name=""PagePath""/>
//                </form>
//              </div>
//            ";
//            }
//        }
//        #endregion

//        #region HTML_CONTENT_RICH_TEXT_MODULE

//        public static string HTML_CONTENT_RICH_TEXT_MODULE = "@{ var richTextModuleConfiguration = Html.JsonObject(ViewData[\"" + Constants.RICHTEXT_MODULE_ALIAS + "\"]); }" +
//            "@{ var dataRichText = Html.JsonObject(ViewData[\"" + Constants.RICHTEXT_MODULE_DATA + "\"]); }" +
//            "@{ var isHideRichTextModule = (string)richTextModuleConfiguration[\"IsHide\"] == \"True\" ? \"display: none;\" : \"\"; }" +
//            @"<section class=""block-filter-top"" style=""@isHideRichTextModule"">
//                            <div class=""container""> 
//                            @if(dataRichText != null)
//                            {
//                                var contentRichText = Html.JsonObject(dataRichText[""Content""]);
//                                @Html.Raw(contentRichText[""Content""])
//                            }
//                            </div>
//              </section>";
//        #endregion

//        #region HTML_CONTENT_LOGIN_MODULE

//        public static string HTML_CONTENT_LOGIN_MODULE = @"
//            <h2>@ViewData[""Module_Title""]</h2>
//            <div class=""row"">
//                <div class=""col-md-8"">
//                    <section id=""loginForm"">
//                        @using(Html.BeginForm(""Login"", ""Account"", new { ReturnUrl = ""/Home"" }, FormMethod.Post, new { @class = ""form-horizontal"", role = ""form"" }))
//                        {
//                            <h4>@ViewData[""Module_SubTitle""]</h4>
//                            <hr />
//                            <div class=""form-group"">
//                                <label class=""col-md-2 control-label"" for=""Email"">@ViewData[""Module_EmailLabel""]</label>
//                                <div class=""col-md-10"">
//                                    <input name=""Email"" type=""text"" class=""form-control"" />
//                                </div>
//                            </div>
//                            <div class=""form-group"">
//                                <label class=""col-md-2 control-label"" for=""Password"">@ViewData[""Module_PasswordLabel""]</label>
//                                <div class=""col-md-10"">
//                                    <input name=""Password"" type=""password"" class=""form-control"" />
//                                </div>
//                            </div>
//                            <div class=""form-group"">
//                                <div class=""col-md-offset-2 col-md-10"">
//                                    <input type=""submit"" value=""@ViewData[""Module_Login_ButtonText""]"" class=""btn btn-default"" />
//                                </div>
//                            </div>
//                        }
//                    </section>
//                </div>
//            </div>
//            ";

//        #endregion

//        #region HTML_CONTENT_HIGHLIGHT_MODULE

//        public static string HTML_CONTENT_HIGHLIGHT_MODULE = "@{ var highLightModuleConfiguration = Html.JsonObject(ViewData[\"" + Constants.HIGHLIGHT_MODULE_ALIAS + "\"]); }" +
//            "@{ var dataHighLightArr = Html.JsonArray((string)ViewData[\"" + Constants.HIGHLIGHT_MODULE_DATA + "\"]); }" +
//            "@{ var isHideHighLightModule = (string)highLightModuleConfiguration[\"IsHide\"] == \"True\" ? \"display: none;\" : \"\"; }" +
//            @"<div class=""top-slider"" style=""@isHideHighLightModule"">
//              <div class=""block-intro"">
//                <div class=""slides-intro"">
//                  @foreach(var item in dataHighLightArr)
//	                {
//                        var content = Html.JsonObject(item[""Content""]);
//                        var image = content[""Image""];
//                          <div class=""slide"" style=""background-image: url('@image')"">
//                            <div class=""content"">
//                              <div class=""inner"">
//                                @if(!String.IsNullOrEmpty(content[""Category""].ToString()))
//                                {
//                                    <div class=""sub-title"">@content[""Category""]</div>
//                                }
//                                <div class=""title"">@content[""Title""]</div>
//                                <div class=""hide-xs"" style=""overflow-wrap: break-word;"">@content[""Description""]</div>
//                                  @if(!String.IsNullOrEmpty(content[""LinkText""].ToString()))
//                                  {
//                                    <a href=""@content[""LinkURL""]"" title=""@content[""Title""]"" class=""btn btn-red"">@content[""LinkText""]</a>
//                                  }
//                              </div>
//                           </div>
//                         </div>
//                    }
//                    </div>
//                </div>
//              </div>";

//        #endregion

//        #region HTML_CONTENT_TILES_MODULE

//        public static string HTML_CONTENT_TILES_MODULE =
//@"@{ " +
//    "var tilesModuleConfiguration = Html.JsonObject(ViewData[\"" + Constants.TILES_MODULE_ALIAS + "\"]);" +
//    "var tilesModuleDataArr = Html.JsonArray((string)ViewData[\"" + Constants.TILES_MODULE_DATA + "\"]); }" +
//@"
//@if (tilesModuleDataArr != null && tilesModuleDataArr.Count > 0)
//{
//    <div class=""slides-favourite clearfix"">
//        @foreach (var item in tilesModuleDataArr)
//        {
//            var content = Html.JsonObject(item[""Content""]);
//            <div class=""slide"">
//                <div title=""@content[""Title""]"" class=""figure"">
//                    <div class=""wrap-slide-img"">
//                        <a href=""@content[""ItemURL""]"">
//                            <img src=""@content[""ImageURL""]"" alt=""@content[""Title""]"" class=""img-responsive"">
//                        </a>
//                    </div>
//                </div>

//                <div class=""content"">
//                    <div class=""sub"">@content[""Title""]</div>
//                    <div class=""desc"">@content[""Description""]</div>
//                </div>
//            </div>
//        }
//    </div>
//}
//";


//        #endregion

//        #region HTML_CONTENT_MAP_MODULE

//        public static string HTML_CONTENT_MAP_MODULE = "{@mapSetting;{{" + Constants.MAP_MODULE_ALIAS + "}}@}" +
//            "@{ var mapData = Html.JsonArray((string)ViewData[\"" + Constants.MAP_MODULE_DATA + "\"]); } " +
//@"<section id="""" class=""block-article-style"">
//    <h3>
//    {:mapSetting.Title:}
//    </h3>
//    <div class=""toggle-box"">
//    <div class=""article-desc"">
//        <!--Show Description-->
//    </div>
//    <div class=""guide-map"">
//        <!--Map content-->
//        <div class=""wrap-map"">
//    	<div id=""map_canvas"" style=""position: relative;overflow: hidden;height: 100%;width: 100%;width: 800px;height: 350px;border: 1px;""></div>
//        </div>
//@{
//    var markerJson = new List<object>();
//    foreach (var marker in mapData)
//    {
//        var markerObject = Html.JsonObject(marker);
//        markerJson.Add(new { title = markerObject[""MarkerTitle""], lat = markerObject[""MarkerLatitude""], longTitude = markerObject[""MarkerLongtitude""], link = markerObject[""MarkerLink""] });
//    }

//    var markerJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(markerJson);
//}
//        <!--Guide Map-->
//      <h1>Guide Map Marker here</h1>
//    </div>
//    </div>
//</section>
//<script src=""https://maps.googleapis.com/maps/api/js?key=AIzaSyBJYcj7x5uQO9coMFO_9Ob-SeEtp7YvRGM""></script>
//<script>
//    var displayCenterMarker = '{:mapSetting.DisplayCenterMarker:}';
//    var mapData2 = '@Html.Raw(markerJsonString)';
//    var maps = JSON.parse(mapData2);
//    var locations = [];
//    for (i = 0; i < maps.length; i++) {
//        locations.push([maps[i].title, maps[i].lat, maps[i].longTitude]);
//    }
//    var mapCenter = new Object();
//    if (displayCenterMarker == 'true' || displayCenterMarker == 'True') {
//        mapCenter.Lat = {:mapSetting.Latitude:};
//        mapCenter.Long = {:mapSetting.Longtitude:};
//        mapCenter.Zoom = {:mapSetting.ZoomLevel:};
//        mapCenter.Title = '{:mapSetting.Title:}';
//        mapCenter.Link = '';
//    } else {
//        mapCenter.Title = locations[0][0];
//        mapCenter.Lat = locations[0][1];
//        mapCenter.Long = locations[0][2];
//        mapCenter.Zoom = 12;
//        mapCenter.Link = '';
//    }

//    var posotionCenter = new google.maps.LatLng(mapCenter.Lat, mapCenter.Long);
//    var map = new google.maps.Map(document.getElementById('map_canvas'), {
//        zoom: mapCenter.Zoom,
//        center: posotionCenter
//    });

//    var markerCenter = new google.maps.Marker({
//        position: posotionCenter,
//        map: map
//    });

//    var infowindow = new google.maps.InfoWindow();
//    var marker, i;
//    for (i = 0; i < locations.length; i++) {
//        marker = new google.maps.Marker({
//            position: new google.maps.LatLng(locations[i][1], locations[i][2]),
//            map: map
//        });

//        google.maps.event.addListener(marker, 'click', (function(marker, i) {
//            return function() {
//                var infowincontent = document.createElement('div');
//                var strong = document.createElement('strong');
//                strong.textContent = locations[i][0];

//                infowincontent.appendChild(strong);
//                infowincontent.appendChild(document.createElement('br'));
//                var text = document.createElement('text');
//                text.textContent = '';
//                infowincontent.appendChild(text);
//                infowindow.setContent(infowincontent);
//                infowindow.open(map, marker);
//            }
//        })(marker, i));

//        var infoWindowCenter = '<p>' + mapCenter.Title +'</p>';
//        infowindow.setContent(infoWindowCenter);
//        infowindow.open(map, markerCenter);
//    }

//</script>
//            ";

//        #endregion

//        #region HTML_CONTENT_HEADER_MODULE
//        public static string HTML_CONTENT_HEADER_MODULE = @"
//              <div class=""header"">
//    <a href=""javscript:void(0)"" class=""btn-nav"">
//      <span>toggle menu</span>
//    </a>
//    <div class=""top-nav"">
//      <div class=""container"">
//        <a class=""logo-aspire"" href=""http://ubs.com"">
//          <img class=""logo-header"" src=""~/ClientSites/global/theme/images/ubs_logo.svg"" alt=""Aspire"">
//          <span>UBS Concierge</span>
//        </a>
//        <div class=""info-location"">
//          <span class=""wrap-breaking-news"">
//            <span id=""timeZone"">{{Timezone}}</span>
//          </span>
//          <span class=""btn-location"">
//            <span class=""name-country"" id=""countryNameTop"">{{CityTop}}</span>
//            <i class=""sprites icon-location""></i>
//          </span>
//        </div>
//        <a class=""btn-close-window"" href=""~/ssologout"">Sign out concierge</a>
//      </div>
//    </div>
//    <div class=""aspire-nav"">
//      <div class=""container"">
//        <ul class=""menu-aspire float-left"">
//          <li>
//            <a href=""#"" title=""Home"">
//              <span class=""menu-icon""><i class=""sprites icon-sign-in-white""></i></span>
//              <span class=""menu-text"">Home</span>
//            </a>
//          </li>
//          <li>
//            <a href=""#"" title=""Profile"">
//              <span class=""menu-icon""><i class=""sprites icon-sign-up-white""></i></span>
//              <span class=""menu-text"">Profile</span>
//              <i class=""sprites icon-arrow-black""></i>
//            </a>
//            <ul class=""sub-list"">
//              <li><a href="""">My Profile</a></li>
//              <li><a href="""">My Preferences</a></li>
//            </ul>
//          </li>
//        </ul>
//        <ul class=""menu-aspire"">
//          <li>
//            <a href=""gourmet.html"">
//              <span class=""menu-icon""><i class=""sprites icon-disk""></i></span>
//              <span class=""menu-text"">Dining</span>
//            </a>
//          </li>
//          <li>
//            <a href=""travel.html"">
//              <span class=""menu-icon""><i class=""sprites icon-global""></i></span>
//              <span class=""menu-text"">Travel</span>
//              <i class=""sprites icon-arrow-black""></i>
//            </a>
//            <ul class=""sub-list"">
//              <li><a href=""city-guide.html"">City Guides</a></li>
//              <li><a href=""hotel-subcat.html"">Hotels</a></li>
//              <li><a href=""#"">Cruises</a></li>
//              <li><a href=""travel-packages.html"">Vacation Packages</a></li>
//              <li><a href=""#"">Sightseeing &amp; Tours</a></li>
//              <li><a href=""#"">Traveler Benefits &amp; Services</a></li>
//            </ul>
//          </li>
//          <li class="""">
//            <a href=""#"">
//              <span class=""menu-icon""><i class=""sprites icon-transportation""></i></span>
//              <span class=""menu-text"">Transportation</span>
//              <i class=""sprites icon-arrow-black""></i>
//            </a>
//            <ul class=""sub-list"">
//              <li><a href=""#"">Car Rental</a></li>
//              <li><a href=""#"">Limo Service</a></li>
//              <li><a href=""#"">Private Jet</a></li>
//              <li><a href=""#"">Airport Services</a></li>
//            </ul>
//          </li>
//          <li>
//            <a href=""#"">
//             <span class=""menu-icon""><i class=""sprites icon-mask""></i></span>
//              <span class=""menu-text"">Lifestyle & <br/>Entertainment</span>
//              <i class=""sprites icon-arrow-black""></i>
//            </a>
//            <ul class=""sub-list"">
//              <li><a href=""entertainment.html"">Entertainment</a></li>
//              <li><a href=""#"">Experiences</a></li>
//              <li><a href=""#"">Golf</a></li>
//            </ul>
//          </li>
//          <li>
//            <a href=""#"">
//              <span class=""menu-icon""><i class=""sprites icon-bag""></i></span>
//              <span class=""menu-text"">Shopping</span>
//              <i class=""sprites icon-arrow-black""></i>
//            </a>
//            <ul class=""sub-list"">
//              <li><a href=""entertainment.html"">Flowers</a></li>
//              <li><a href=""#"">Shopper Protection &amp; Security</a></li>
//            </ul>
//          </li>
//        </ul>
//        <ul class=""menu-aspire float-right"">
//          <li>
//            <a href=""#"">
//              <span class=""menu-icon""><i class=""sprites icon-service""></i></span>
//              <span class=""menu-text"">Place your request</span>
//              <i class=""sprites icon-arrow-black""></i>
//            </a>
//            <ul class=""sub-list"">
//              <li><a href="""">Dining</a></li>
//              <li>
//                <a href="""">Travel <i class=""sprites icon-arrow-black""></i></a>
//                <ul class=""sub-list"">
//                  <li><a href="""">Hotels</a></li>
//                  <li><a href="""">Cruises</a></li>
//                  <li><a href="""">Sightseeing & Tours</a></li>
//                </ul>
//              </li>
//              <li>
//                <a href="""">Transportation <i class=""sprites icon-arrow-black""></i></a>
//                <ul class=""sub-list"">
//                  <li><a href="""">Car Rental</a></li>
//                  <li><a href="""">Limo Service</a></li>
//                  <li><a href="""">Private Jet</a></li>
//                </ul>
//              </li>
//              <li>
//                <a href="""">Lifestyles & Entertainment <i class=""sprites icon-arrow-black""></i></a>
//                <ul class=""sub-list"">
//                  <li><a href="""">Entertainment</a></li>
//                  <li><a href="""">Golf</a></li>
//                </ul>
//              </li>
//              <li>
//                <a href="""">Shopping <i class=""sprites icon-arrow-black""></i></a>
//                <ul class=""sub-list"">
//                  <li><a href="""">Flowers</a></li>
//                </ul>
//              </li>
//              <li><a href="""">Other</a></li>
//            </ul>
//          </li>
//        </ul>
//      </div>
//    </div>
//    <div class=""box-location"">
//      <div class=""btn-close-location"">
//        <i class=""sprites icon-close-circle-white""></i>
//      </div>
//      <label title="""" class=""btn btn-back""><i class=""sprites icon-arrow""></i>BACK</label>
//      <div class=""inner"">
//        We would like to know your location
//        <div class=""location-name"">
//          <span class=""city-name"" id=""cityName"">{{City}}</span>
//          ,
//          <span class=""country-name"" id=""countryName"">{{Country}}</span>
//          <i class=""ico-nav-location""></i>
//        </div>
//        or search
//        <div class=""location-select-box"">
//          <div class=""form-group"">
//            <div class=""box-group"">
//                    <input type=""text"" id=""txtLocation_header"" placeholder=""City, Country"" class=""form-control"" data-spy=""autocomplete"" />
//                    <input type = ""hidden"" id=""hdfSelectedLocation"" value="""" />
//            </div>
//          </div>
//        </div>
//        <a href=""#"" class=""btn btn-red btn-confirm-location"" onclick=""GetLocation()"">CONFIRM</a>
//      </div>
//    </div>
//  </div>
//<div class=""lt-overlay""></div>
//<script type=""text/javascript"">
//    function GetLocation() {
//            var location = $('#hdfSelectedLocation').val();
//            if (location)
//            {
//                var countryCode = location.split(',')[1];
//                var cityCode = location.split(',')[2];
//                $.ajax({
//                    type: 'POST',
//                    dataType: 'json',
//                    url: 'api/Location/SetLocation',
//                    data: { countryCode: countryCode, cityCode: cityCode},
//                    cache: false,
//                    success: function(data)
//                    {
//                        var jsonData = data;
//                        $('#countryName').text(jsonData.Rss.CountryName);
//                        $('#countryNameTop').text(jsonData.Rss.City);
//                        $('#cityName').text(jsonData.Rss.City);
//                        $('#timeZone').text(jsonData.Rss.ClientDateTime);
//                        $('#hdfSelectedLocation').val('');
//                        $('#txtLocation_header').val('');

//                    },
//                    error: function(jqXHR, textStatus, errorThrown) {
//                        alert('An error occurred... Look at the console (F12 or Ctrl+Shift+I, Console tab) for more information!');
//                        console.log('jqXHR:');
//                        console.log(jqXHR);
//                        console.log('textStatus:');
//                        console.log(textStatus);
//                        console.log('errorThrown:');
//                        console.log(errorThrown);
//                    }
//                });
//                return true;
//            }
//            else
//                return false;
//        }
//</script>
//                    ";
//        #endregion

//        #region HTML_CONTENT_FOOTER_MODULE
//        public static string HTML_CONTENT_FOOTER_MODULE = @"
//                <footer id=""footer"">
//                <div class=""container"">
//                  <div class=""footer-menu font-12"">
//                    <div class=""row"">
//                      <div class=""col-lg-12 col-md-12 col-sm-12 col-xs-12 customer-service"">
//                        <ul class=""list-unstyled footer-list-child font-bold"">
//                          <li><a href=""~/Home/CustomerService"">CUSTOMER SERVICE</a></li>
//                          <li><a href=""~/Home/Feedback"">FEEDBACK</a></li>
//                          <li><a href=""~/Home/TermOfUse"">SITE TERMS & CONDITIONS</a></li>
//                          <li><a href=""~/Home/FAQs"">FAQs</a></li>
//                          <li><a href=""~/Home/Privacy"">PRIVACY</a></li>
//                        </ul>
//                      </div>
//                    </div>
//                  </div>
//                  <div class=""footer-bottom font-11"">
//                    <div class=""copyright-text"">
//                      <div>Copyright &copy; 1998-2017 VIPdesk.com, Inc. (d.b.a. Aspire Lifestyles). All Rights Reserved.  </div>
//                      <div>Use of this Web site constitutes acceptance of the Aspire Lifestyles Terms of Use and Privacy Policy</div>
//                    </div>
//                  </div>
//                </div>
//              </footer>
//                ";
//        #endregion

    }
}
