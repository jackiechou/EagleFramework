using System.Security.Principal;

namespace Eagle.Repositories.SystemManagement.Security
{
    public interface IUserIdentity : IIdentity
    {
        int Id { get; }
        string FullName { get; set; }
    }
}
