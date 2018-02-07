using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Sherpa.Core.Common;
using Sherpa.Core.Permission;
using Sherpa.Services.Common;
using Sherpa.Services.Dtos.Common;
using Sherpa.Services.Dtos.Community;

namespace Sherpa.Services.Library
{
    public class DocumentActionManager : DisposableObject
    {
        #region Entity Action Onboard document

        public IEnumerable<EntityAction> GetOnboardDocumentItemResultActions(OnboardDocumentDetail onboardDocDetail)
        {
            yield return
                DocumentAction.ViewDocument(EntityActionType.ViewDocument, onboardDocDetail.OnboardDocumentId.ToString(),
                    null, null, onboardDocDetail.Url);
        }

        public IEnumerable<EntityAction> GetOnboardDocumentItemActions(OnboardDocumentDetail onboardDocDetail)
        {
            yield return
                DocumentAction.ViewDocument(EntityActionType.ViewDocument, onboardDocDetail.OnboardDocumentId.ToString(),
                    "CanView", null, onboardDocDetail.Url);
        }

        public IEnumerable<EntityAction> GetOnboardDocumentListActions(ClaimsPrincipal principal, IEnumerable<OnboardDocumentItemResult> onboardDocList)
        {
            if (principal.IsAdmin() ||
                principal.GetMemberId() == onboardDocList.FirstOrDefault().OnboardDocument.MemberId ||
                (principal.FindFirst(SherpaPermissionCapabilities.PROFILE) != null &&
                 principal.FindFirst(SherpaPermissionCapabilities.PROFILE).Value.ToPermission() > SherpaPermission.View)) 
                yield return DocumentAction.UploadDocument();
        }

        #endregion


        #region Disposable

        private bool _disposed;

        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}