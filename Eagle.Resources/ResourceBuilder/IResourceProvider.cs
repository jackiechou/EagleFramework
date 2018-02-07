namespace Eagle.Resources.ResourceBuilder
{
    public interface IResourceProvider
    {
        object GetResource(string name, string culture);
    }

    //private IResourceProvider resourceProvider = new XmlResourceProvider(
    //    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"bin\Eagle.Resources.xml")
    //); // assume Resources.xml is in the bin folder
}
