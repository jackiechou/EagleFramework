namespace Eagle.EntityFramework
{
    public interface IMsmqContext : IDataContext
    {
        void CreateMessageQueue(string connectionString);
        void Insert<T>(T item, string label, bool isRetry);
        new T Get<T>();
        int GetMessageCount<T>();
        void EnQueueItem<T>(T item, string lable, string path) where T : class;
        T DeQueueItem<T>(string path);
    }
}
