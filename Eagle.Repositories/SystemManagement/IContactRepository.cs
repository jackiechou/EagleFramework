using System;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.SystemManagement
{
    public interface IContactRepository: IRepositoryBase<Contact>
    {
        bool HasDataExisted(string email, string mobile, string firstName, string lastName, SexType? sex,
             DateTime? dob, string linePhone1, string linePhone2, string taxNo);

    }
}
