using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Eagle.Entities.SystemManagement;
using Eagle.EntityFramework;

namespace Eagle.Repositories.SystemManagement
{
    public class FunctionCommandRepository : RepositoryExtend<FunctionCommand, Guid>, IFunctionCommandRepository
    {
        public FunctionCommandRepository(IDataContext context) : base(context)
        {
        }

        public IEnumerable<AppClaim> GetAppClaimsByFunctionName(string functionName)
        {
            if (string.IsNullOrEmpty(functionName))
            {
                return null;
            }

            Expression<Func<FunctionCommand, bool>> predicate = f => f.Name == functionName;
            var functionCommand = this.FindBy(predicate, x => x.AppClaims).FirstOrDefault();
            if (functionCommand == null)
            {
                return null;
            }

            return functionCommand.AppClaims;
        }
    }
}
