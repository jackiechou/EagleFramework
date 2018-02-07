using System;
using System.Web.Optimization;

namespace Eagle.WebApp
{
    public class BundleConfig
    {
        public static void ConfigureIgnoreList(IgnoreList ignoreList)
        {
            if (ignoreList == null) throw new ArgumentNullException("ignoreList");

            ignoreList.Clear(); // Clear the list, then add the new patterns.

            ignoreList.Ignore("*.intellisense.js");
            ignoreList.Ignore("*-vsdoc.js");
            ignoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
            ignoreList.Ignore("*.min.js", OptimizationMode.WhenDisabled);
            ignoreList.Ignore("*.min.css", OptimizationMode.WhenDisabled);
        }

        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            // allow minified files in debug mode.
            bundles.IgnoreList.Clear();
            //prevent the unobtrusive ajax script from loading twice
            //bundles.IgnoreList.Ignore("*.unobtrusive-ajax.min.js", OptimizationMode.WhenDisabled);

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui.js"
            ));



            bundles.Add(new ScriptBundle("~/bundles/jqueryajax").Include(
               "~/Scripts/jquery.unobtrusive-ajax.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate.js",
                "~/Scripts/jquery.validate.unobtrusive.js",
                "~/Scripts/jquery.validate.custom.js"
            ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/Site.css"));

            #region Error Theme ===============================================================

            bundles.Add(new StyleBundle("~/Content/BaseStyle").Include(
                "~/Content/bootstrap.css",
                "~/Content/error-page.css"
            ));

            #endregion ===========================================================================


            #region elFinder bundles
            bundles.Add(new StyleBundle("~/Content/elfinder").Include(
                            "~/Scripts/plugins/elfinder/css/elfinder.full.css",
                            "~/Scripts/plugins/elfinder/css/theme.css"
                            ));

            bundles.Add(new ScriptBundle("~/Scripts/elfinder").Include(
                             "~/Scripts/plugins/elfinder/js/elfinder.full.js"
                             //"~/Content/elfinder/js/i18n/elfinder.pt_BR.js"
                             ));
            #endregion

            #region desktop header css and javacript ==============================
            //bundles.Add(new StyleBundle("~/Themes/Default/DesktopStyle").Include(
            //    "~/Themes/Default/Content/assets/jquery-ui.css",
            //    "~/Themes/Default/Content/assets/bootstrap.css",
            //    "~/Themes/Default/Content/assets/style_icon.css",
            //    "~/Scripts/plugins/bootstrap-plugins/bootstrap-datetimepicker/css/bootstrap-datetimepicker.css",
            //    "~/Scripts/plugins/superfish/css/superfish.css",
            //    "~/Scripts/plugins/superfish/css/superfish-vertical.css",
            //    "~/Scripts/plugins/superfish/css/superfish-navbar.css",
            //    "~/Scripts/plugins/select2/css/select2.css",
            //    "~/Scripts/plugins/select2/css/select2-bootstrap.css",
            //    "~/Scripts/plugins/bootstrap-plugins/bootstrap-datetimepicker/css/bootstrap-datetimepicker.css",
            //    "~/Themes/Default/Content/styles/default.css",
            //    "~/Themes/Default/Content/styles/home-responsive.css",
            //    "~/Themes/Default/Content/styles/dark.css"
            //  ));


            // //DESKTOP SCRIPTS
            // bundles.Add(new ScriptBundle("~/Themes/Default/DesktopFooterScript").Include(
            //     "~/Scripts/plugins/superfish/js/hoverIntent.js",
            //     "~/Scripts/plugins/superfish/js/superfish.js",
            //     "~/Scripts/plugins/superfish/js/supersubs.js",
            //     "~/Scripts/plugins/select2/js/select2.full.js",
            //     "~/Scripts/plugins/colorbox/jquery.colorbox.js",
            //     "~/Scripts/jquery.blockUI.js",
            //     "~/Scripts/app/common/AjaxGlobalHandler.js",
            //     "~/Scripts/app/common/loading.js",
            //     "~/Scripts/app/common/lib.js",
            //     "~/Scripts/app/common/message.js",
            //     "~/Scripts/app/common/validates.js",
            //     "~/Scripts/back-to-top.js",
            //     "~/Scripts/app/theme/default/site.js"
            //));
            #endregion ========================================================================== 
            
            #region booking theme header css and javacript ==============================
            bundles.Add(new StyleBundle("~/Themes/Booking/DesktopStyle").Include(
                "~/Themes/Booking/Content/plugins/bootstrap/css/bootstrap.css",
                "~/Themes/Booking/Content/plugins/superfish/css/superfish.css",
                "~/Themes/Booking/Content/plugins/superfish/css/superfish-vertical.css",
                "~/Themes/Booking/Content/plugins/css/superfish-navbar.css",
                "~/Themes/Booking/Content/plugins/select2/css/select2.css",
                "~/Themes/Booking/Content/plugins/select2/css/select2-bootstrap.css",
                "~/Themes/Booking/Content/css/style_icon.css",
                "~/Themes/Booking/Content/css/validate.css",
                "~/Themes/Booking/Content/css/app.css",
                "~/Themes/Booking/Content/css/shop.plugins.css",
                "~/Themes/Booking/Content/css/shop.blocks.css",
                "~/Themes/Booking/Content/css/shop.style.css",
                "~/Themes/Booking/Content/css/headers/header-v5.css",
                "~/Themes/Booking/Content/css/footers/footer-v4.css",
                "~/Themes/Booking/Content/plugins/animate.css",
                "~/Themes/Booking/Content/plugins/line-icons/line-icons.css",
                "~/Themes/Booking/Content/plugins/scrollbar/css/jquery.mCustomScrollbar.css",
                "~/Themes/Booking/Content/plugins/owl-carousel/owl-carousel/owl.carousel.css",
                "~/Themes/Booking/Content/plugins/owl-carousel/owl-carousel/owl.theme.css",
                "~/Themes/Booking/Content/plugins/revolution-slider/rs-plugin/css/settings.css",
                "~/Themes/Booking/Content/css/pages/profile.css",
                "~/Themes/Booking/Content/css/theme-colors/default.css",
                "~/Themes/Booking/Content/css/custom.css",
                "~/Themes/Booking/Content/css/custom-responsive.css"
              ));


            //DESKTOP SCRIPTS
            bundles.Add(new ScriptBundle("~/Themes/Booking/DesktopFooterScript").Include(
                "~/Themes/Booking/Content/plugins/superfish/js/hoverIntent.js",
                "~/Themes/Booking/Content/plugins/superfish/js/superfish.js",
                "~/Themes/Booking/Content/plugins/superfish/js/supersubs.js",
                "~/Themes/Booking/Content/plugins/select2/js/select2.full.js",
                "~/Themes/Booking/Content/plugins/dotdotdot/jquery.dotdotdot.js",
                "~/Themes/Booking/Content/plugins/smoothScroll.js",
                "~/Themes/Booking/Content/plugins/jquery.parallax.js",
                "~/Themes/Booking/Content/plugins/owl-carousel/owl-carousel/owl.carousel.js",
                "~/Themes/Booking/Content/plugins/revolution-slider/rs-plugin/js/jquery.themepunch.tools.min.js",
                "~/Themes/Booking/Content/plugins/revolution-slider/rs-plugin/js/jquery.themepunch.revolution.min.js",
                "~/Themes/Booking/Content/plugins/colorbox/jquery.colorbox.js",
                "~/Themes/Booking/Content/plugins/jquery.blockUI.js",
                "~/Themes/Booking/Content/plugins/jquery.lazyload.js",
                "~/Themes/Booking/Content/js/plugins/revolution-slider.js",
                "~/Themes/Booking/Content/js/app/AjaxGlobalHandler.js",
                "~/Themes/Booking/Content/js/app/back-to-top.js",
                "~/Themes/Booking/Content/js/app/lib.js",
                "~/Themes/Booking/Content/js/app/message.js",
                "~/Themes/Booking/Content/js/app/validates.js",
                "~/Themes/Booking/Content/js/app/facebook.js",
                "~/Themes/Booking/Content/js/app/social-link.js",
                "~/Themes/Booking/Content/js/app/chatbox.js",
                "~/Themes/Booking/Content/js/app/rating.js",
                "~/Themes/Booking/Content/js/app/cart.js",
                "~/Themes/Booking/Content/js/main.js",
                "~/Scripts/plugins/loadingoverlay/src/loadingoverlay.js",
                "~/Scripts/plugins/loadingoverlay/extras/loadingoverlay_progress.js"
           ));
            
            #endregion ========================================================================== 

            #region ADMIN STYLE ==========================================================
            //LOGIN STYLE
            bundles.Add(new StyleBundle("~/Themes/Default/LoginStyle").Include(
                "~/Themes/Default/Content/assets/bootstrap.css",
                "~/Themes/Default/Content/assets/bootstrap-theme.css",
                "~/Themes/Default/Content/assets/style_icon.css",
                "~/Themes/Default/Content/login.css"
           ));

            //MAIN LAYOUT TYPE
            bundles.Add(new StyleBundle("~/Themes/Default/TwoColumnAdminStyle").Include(
                "~/Themes/Default/Content/assets/jquery-ui.css",
                "~/Themes/Default/Content/assets/bootstrap.css",
                "~/Themes/Default/Content/assets/bootstrap-theme.css",
                "~/Themes/Default/Content/assets/style_icon.css",

                "~/Scripts/ckeditor/contents.css",
                "~/Scripts/plugins/bootstrap-plugins/bootstrap-datetimepicker/css/bootstrap-datetimepicker.css",
                "~/Scripts/plugins/bootstrap-plugins/bootstrap-select/css/bootstrap-select.css",
                "~/Scripts/plugins/bootstrap-plugins/bootstrap-tagsinput/css/bootstrap-tagsinput.css",
                "~/Scripts/plugins/bootstrap-plugins/bootstrap-toggle/bootstrap-toggle.css",

                "~/Scripts/plugins/superfish/css/megafish.css",
                "~/Scripts/plugins/superfish/css/superfish.css",
                "~/Scripts/plugins/superfish/css/superfish-vertical.css",
                "~/Scripts/plugins/superfish/css/superfish-navbar.css",

                "~/Scripts/plugins/datatables/css/jquery.dataTables_themeroller.css",
                "~/Scripts/plugins/datatables/css/jquery.dataTables.css",
                "~/Scripts/plugins/tablednd/tablednd_custom.css",
                 "~/Scripts/plugins/contextmenu/jquery.contextMenu.css",
                "~/Scripts/plugins/easyui/themes/icon.css",
                "~/Scripts/plugins/select2/css/select2.css",
                "~/Scripts/plugins/select2/css/select2-bootstrap.css",
                "~/Scripts/plugins/jquerysteps/jquery.steps.css",

                "~/Themes/Default/Content/style.css",
                "~/Themes/Default/Content/style_main_layout.css",
                "~/Themes/Default/Content/style_custom.css",
                "~/Themes/Default/Content/style-validate.css"
            ));

            //FULL LAYOUT TYPE
            bundles.Add(new StyleBundle("~/Themes/Default/FullScreenAdminStyle").Include(
                "~/Themes/Default/Content/assets/jquery-ui.css",
                "~/Themes/Default/Content/assets/bootstrap.css",
                "~/Themes/Default/Content/assets/bootstrap-theme.css",
                "~/Themes/Default/Content/assets/style_icon.css",

                "~/Scripts/ckeditor/contents.css",
                "~/Scripts/plugins/bootstrap-plugins/bootstrap-datetimepicker/css/bootstrap-datetimepicker.css",
                "~/Scripts/plugins/bootstrap-plugins/bootstrap-select/css/bootstrap-select.css",
                "~/Scripts/plugins/bootstrap-plugins/bootstrap-tagsinput/css/bootstrap-tagsinput.css",
                "~/Scripts/plugins/bootstrap-plugins/bootstrap-toggle/bootstrap-toggle.css",

                "~/Scripts/plugins/superfish/css/megafish.css",
                "~/Scripts/plugins/superfish/css/superfish.css",
                "~/Scripts/plugins/superfish/css/superfish-vertical.css",
                "~/Scripts/plugins/superfish/css/superfish-navbar.css",

                "~/Scripts/plugins/datatables/css/jquery.dataTables_themeroller.css",
                "~/Scripts/plugins/datatables/css/jquery.dataTables.css",
                "~/Scripts/plugins/tablednd/tablednd_custom.css",
                "~/Scripts/plugins/contextmenu/jquery.contextMenu.css",
                "~/Scripts/plugins/select2/css/select2.css",
                "~/Scripts/plugins/select2/css/select2-bootstrap.css",
                "~/Scripts/plugins/jquerysteps/jquery.steps.css",
                "~/Scripts/plugins/fileinput/fileinput.css",

                "~/Themes/Default/Content/style.css",
                "~/Themes/Default/Content/style_full_layout.css",
                "~/Themes/Default/Content/style_custom.css",
                "~/Themes/Default/Content/style-validate.css"
            ));


            //REPORT STYLE
            bundles.Add(new StyleBundle("~/Themes/Default/ReportStyle").Include(
                "~/Themes/Default/Content/assets/bootstrap.css",
                "~/Themes/Default/Content/assets/bootstrap-theme.css",
                "~/Themes/Default/Content/assets/bootstrap-datepicker.css",
                "~/Themes/Default/Content/assets/style_icon.css",

                "~/Scripts/plugins/superfish/css/megafish.css",
                "~/Scripts/plugins/superfish/css/superfish.css",
                "~/Scripts/plugins/superfish/css/superfish-vertical.css",
                "~/Scripts/plugins/superfish/css/superfish-navbar.css",

                "~/Scripts/plugins/datatables/css/jquery.dataTables_themeroller.css",
                "~/Scripts/plugins/datatables/css/jquery.dataTables.css",
                "~/Scripts/plugins/tablednd/tablednd_custom.css",
                "~/Scripts/plugins/easyui/themes/bootstrap/easyui.css",
                "~/Scripts/plugins/easyui/themes/icon.css",
                "~/Scripts/plugins/contextmenu/jquery.contextMenu.css",
                "~/Scripts/plugins/select2/css/select2.css",
                "~/Scripts/plugins/select2/css/select2-bootstrap.css",
                "~/Scripts/plugins/colorbox/colorbox.css",

                 "~/Scripts/plugins/bootstrap-plugins/bootstrap-select/css/bootstrap-select.css",
                 "~/Scripts/plugins/bootstrap-plugins/bootstrap-tagsinput/bootstrap-tagsinput.css",
                 "~/Scripts/plugins/bootstrap-plugins/bootstrap-toggle/bootstrap-toggle.css",

                "~/Themes/Default/Content/report.css",
                "~/Themes/Default/Content/style_custom.css"
                ));
            #endregion ======================================================================

            #region Application Scripts =============================================
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
               "~/Scripts/bootstrap.js",
               "~/Scripts/plugins/bootstrap-plugins/bootbox.js",
               "~/Scripts/plugins/bootstrap-plugins/bootstrap-contextmenu.js",
               "~/Scripts/plugins/bootstrap-plugins/bootstrap-select/js/bootstrap-select.js",
               "~/Scripts/plugins/bootstrap-plugins/bootstrap-tagsinput/bootstrap-tagsinput.js",
               "~/Scripts/plugins/bootstrap-plugins/bootstrap-toggle/bootstrap-toggle.js",
               "~/Scripts/plugins/bootstrap-plugins/bootstrap-datetimepicker/js/bootstrap-datetimepicker.js",
                "~/Scripts/moment.js"
           ));


            bundles.Add(new ScriptBundle("~/Scripts/ckeditor").Include(
                "~/Scripts/ckeditor/ckeditor.js",
                "~/Scripts/ckeditor/ckfinder/ckfinder.js"
            ));

            //login javacript
            bundles.Add(new ScriptBundle("~/Scripts/App/LoginScript").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/js/bootstrap.js",
                "~/Scripts/plugins/colorbox/jquery.colorbox.js",
                "~/Scripts/jquery.blockUI.js",
                "~/Scripts/app/common/AjaxGlobalHandler.js"
            ));

            //footer javacript
            bundles.Add(new ScriptBundle("~/Scripts/FooterScript").Include(
                "~/Scripts/plugins/superfish/js/hoverIntent.js",
                "~/Scripts/plugins/superfish/js/superfish.js",
                "~/Scripts/plugins/superfish/js/supersubs.js",
                "~/Scripts/plugins/select2/js/select2.full.js",
                "~/Scripts/plugins/jquerysteps/jquery.steps.js",
                "~/Scripts/plugins/datatables/js/jquery.dataTables.js",
                "~/Scripts/plugins/tablednd/js/jquery.tablednd.js",
                "~/Scripts/plugins/contextmenu/jquery.contextMenu.js",
                 //"~/Scripts/plugins/monthpicker/jquery.mtz.monthpicker.js",
                 //"~/Scripts/plugins/monthpicker/jquery.mtz.monthpicker.custom.js",
                 //"~/Scripts/jquery.cookie.js",
                 "~/Scripts/app/common/message.js",
                "~/Scripts/plugins/colorbox/jquery.colorbox.js",
                "~/Scripts/jquery.blockUI.js",
                "~/Scripts/app/common/AjaxGlobalHandler.js",
                "~/Scripts/app/common/loading.js",
                "~/Scripts/app/common/lib.js",
                "~/Scripts/app/common/message.js",
                "~/Scripts/app/common/validates.js",
                "~/Scripts/app/common/apps.js",
                "~/Scripts/app/common/footer.js",
                "~/Scripts/back-to-top.js",
                "~/Scripts/plugins/fileinput/fileinput.js"
            ));
          

            #endregion =============================================================

            bundles.Add(new StyleBundle("~/Content/themes/ui-lightness/css").Include(
                                      "~/Content/themes/ui-lightness/jquery.ui.all.css"));
#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
            BundleTable.EnableOptimizations = true;
#endif
        }



    }
}