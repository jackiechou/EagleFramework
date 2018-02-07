using System;
using System.Collections.Generic;
using Eagle.Entities.SystemManagement;
using Eagle.Repositories.SystemManagement;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.EntityMapping.Common;

namespace Eagle.Services.SystemManagement
{
    public class FunctionCommandService : BaseService, IFunctionCommandService
    {
        private IFunctionCommandRepository _repository;

        public FunctionCommandService(IFunctionCommandRepository repository)
        {
            if (repository == null)
                throw new ArgumentNullException("functionCommandrepository");
            _repository = repository;
        }

        public IEnumerable<AppClaimDetail> GetClaimsAuthorization(string functionName)
        {
            if (string.IsNullOrEmpty(functionName))
                return null;

            var result = _repository.GetAppClaimsByFunctionName(functionName);
            return result.ToDtos<AppClaim, AppClaimDetail>();
        }


        //public FunctionCommandService(IUnitOfWork unitOfWork) : base(unitOfWork) { }

        //public IEnumerable<AppClaimDetail> GetClaimsAuthorization(string functionName)
        //{
        //    if (string.IsNullOrEmpty(functionName))
        //        return null;

        //    var result = UnitOfWork.FunctionCommandRepository.GetAppClaimsByFunctionName(functionName);
        //    return result.ToDtos<AppClaim, AppClaimDetail>();
        //}
    }
}
