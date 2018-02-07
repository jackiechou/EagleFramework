using Autofac;

namespace Eagle.EntityFramework
{
    public class DataEntityFrameworkAutoFacModule: Module
    {
        private readonly string _connectionString;
        public DataEntityFrameworkAutoFacModule(string connectionString)
        {
            _connectionString = connectionString;
        }
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new DataContext(_connectionString)).As<IDataContext>().InstancePerLifetimeScope();

            //var defaultDatabaseContextParam = new NamedParameter("connectionString", _connectionString);
            //builder.RegisterType<DataContext>().WithParameter(defaultDatabaseContextParam).As<IDataContext>().InstancePerLifetimeScope();

            builder.RegisterType<DataContext>().WithParameter(new TypedParameter(typeof(string), _connectionString)).As<DataContext>().InstancePerLifetimeScope();
        }
    }
}
