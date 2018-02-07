namespace Eagle.Core.HtmlHelper
{
    public class PartsDescriptionAttribute : System.ComponentModel.DescriptionAttribute
    {
        /// <summary>
        /// この項目が部品に表示されるかを表します。
        /// </summary>
        public bool EnableParts
        {
            get
            {
                return _enableparts;
            }
            private set
            {
                _enableparts = value;
            }
        }
        private bool _enableparts = true;
        public PartsDescriptionAttribute() { }
        public PartsDescriptionAttribute(string Descript) : base(Descript) { }
        public PartsDescriptionAttribute(string Descript, bool enableparts)
            : base(Descript)
        {
            this.EnableParts = enableparts;
        }
        public PartsDescriptionAttribute(bool enableparts)
        {
            this.EnableParts = enableparts;
        }

    }
}
