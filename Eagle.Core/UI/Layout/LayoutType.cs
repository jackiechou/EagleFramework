namespace Eagle.Core.UI.Layout
{
    public enum LayoutSetting
    {
        MasterLayout=1,
        FullMainLayout = 2,
        MainLayout =3,
        FormLayout=4,
        AjaxLayout=5,
        PopUpLayout = 6,
        ReportLayout = 7,
        LoginLayout = 8
    }

    public static class LayoutType
    {
        #region  TemplateName ======================================================================================

        public static string MainTemplateName { get; set; } = "Main Layout";

        public static string FullTemplateName { get; set; } = "Full Main Layout";

        public static string LoginTemplateName { get; set; } = "Login Layout";

        public static string ErrorTemplateName { get; set; } = "Error Layout";

        public static string FormTemplateName { get; set; } = "Form Layout";

        public static string PopUpTemplateName { get; set; } = "PopUp Layout";

        #endregion ================================================================================================

        #region  TemplateKey ======================================================================================

        public static string MainTemplateKey { get; set; } = "MAIN-LAYOUT";

        public static string FullTemplateKey { get; set; } = "FULL-MAIN-LAYOUT";

        public static string LoginTemplateKey { get; set; } = "LOGIN-LAYOUT";

        public static string ErrorTemplateKey { get; set; } = "ERROR-LAYOUT";

        public static string FormTemplateKey { get; set; } = "FORM-LAYOUT";

        public static string PopUpTemplateKey { get; set; } = "POPUP-LAYOUT";

        #endregion ================================================================================================

        #region Layout ======================================================================================
        public static string AjaxLayout { get; set; } = "~/Themes/Default/Views/Shared/AdminLayouts/AjaxLayout.cshtml";
        public static string MasterLayout { get; set; } = "~/Themes/Default/Views/Shared/AdminLayouts/MasterLayout.cshtml";

        public static string SharedLayout { get; set; } = "~/Themes/Default/Views/Shared/AdminLayouts/SharedLayout.cshtml";

        public static string MainLayout { get; set; } = "~/Themes/Default/Views/Shared/AdminLayouts/MainLayout.cshtml";

        public static string FullMainLayout { get; set; } = "~/Themes/Default/Views/Shared/AdminLayouts/FullMainLayout.cshtml";

        public static string PopUpLayout { get; set; } = "~/Themes/Default/Views/Shared/AdminLayouts/PopUpLayout.cshtml";

        public static string ReportLayout { get; set; } = "~/Themes/Default/Views/Shared/AdminLayouts/ReportLayout.cshtml";

        public static string FormLayout { get; set; } = "~/Themes/Default/Views/Shared/AdminLayouts/FormLayout.cshtml";

        public static string ErrorLayout { get; set; } = "~/Themes/Default/Views/Shared/AdminLayouts/ErrorLayout.cshtml";

        public static string LoginLayout { get; set; } = "~/Themes/Default/Views/Shared/AdminLayouts/LoginLayout.cshtml";

        #endregion end Theme ==================================================
      
    }
}
