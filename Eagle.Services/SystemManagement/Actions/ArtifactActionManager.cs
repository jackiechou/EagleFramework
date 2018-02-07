using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using Sherpa.Core.Common;
using Sherpa.Core.Permission;
using Sherpa.Core.Validation;
using Sherpa.Entities.Library;
using Sherpa.Services.Common;
using Sherpa.Services.Common.Validations;
using Sherpa.Services.Dtos.Common;
using Sherpa.Services.Dtos.ContentManagement;

namespace Sherpa.Services.Library
{
    public class ArtifactActionManager : DisposableObject
    {
        private ClaimsPrincipal Principal { get; set; }

        public ArtifactActionManager(ClaimsPrincipal principal)
        {
            Principal = principal;
        }

        public IEnumerable<EntityAction> GetSystemContainerSummaryListActions(
            IEnumerable<ContainerSummaryItemResult> list)
        {
            return null;
        }

        public IEnumerable<EntityAction> GetUserContainerSummaryListActions(IEnumerable<ContainerSummaryItemResult> list)
        {
            yield return ArtifactAction.CreateUserContainer();
        }

        public IEnumerable<EntityAction> GetSystemContainerSummaryActions(ContainerSummary item)
        {
            yield return ArtifactAction.ViewContainer(item.ContainerId.ToString());
        }

        public IEnumerable<EntityAction> GetUserContainerSummaryActions(ContainerSummary item)
        {
            yield return ArtifactAction.ViewContainer(item.ContainerId.ToString());

            ISpecification<int> editValidator = new PermissionValidator<int>(Principal,
                SherpaPermissionCapabilities.LIBRARY, SherpaPermission.Super);
            if (editValidator.IsSatisfyBy(int.MinValue) || Principal.GetMemberId() == item.MemberId)
            {
                yield return ArtifactAction.DeleteUserContainer(item.ContainerId.ToString());
                yield return ArtifactAction.UpdateUserContainer(item.ContainerId.ToString());
            }
        }

        public IEnumerable<EntityAction> GetContainerDetailActions(ContainerDetail item)
        {
            if (item.ContainerType == ContainerType.UserCreated)
            {
                ISpecification<int> editValidator = new PermissionValidator<int>(Principal,
                    SherpaPermissionCapabilities.LIBRARY, SherpaPermission.Super);
                if (editValidator.IsSatisfyBy(int.MinValue) || Principal.GetMemberId() == item.ContainerId)
                {
                    yield return ArtifactAction.DeleteUserContainer(item.ContainerId.ToString());
                    yield return ArtifactAction.UpdateUserContainer(item.ContainerId.ToString());
                }
                yield return ArtifactAction.CreateArtifact();
                yield return ArtifactAction.CreateUserContainer();
            }
        }

        public IEnumerable<EntityAction> GetArtifactSummaryListActions(IEnumerable<ArtifactSummaryItemResult> list)
        {
            return null;
        }

        public IEnumerable<EntityAction> GetArtifactSummaryActions(ArtifactSummary item)
        {
            yield return ArtifactAction.ViewArtifactInfo(item.ArtifactId.ToString());
            yield return ArtifactAction.PostRelatedArtifacts(item.ArtifactId.ToString());
            yield return ArtifactAction.DownloadArtifactFile(item.ArtifactId.ToString());
            var fileExtension = Path.GetExtension(item.FileName);
            switch (item.ArtifactTypeName)
            {
                case "Document":
                    switch (fileExtension)
                    {
                        case ".doc":
                        case ".docx":
                        case ".pdf":
                        case ".html":
                        yield return ArtifactAction.LaunchArtifact(item.ArtifactId.ToString());
                            break;
                    }
                    break;
                case "PaySlip":
                case "Template":
                case "Image":
                    yield return ArtifactAction.LaunchArtifact(item.ArtifactId.ToString());
                    break;
                case "Audio":
                case "Video":
                    yield return ArtifactAction.LaunchArtifact(item.ArtifactId.ToString());
                    yield return ArtifactAction.StreamingArtifactFile(item.ArtifactId.ToString());
                    break;
            }

            if (item.Container.ContainerType == ContainerType.SystemDefined)
            {
                ISpecification<int> editValidator;
                switch (item.Container.ContainerId)
                {
                    case (int) FeatureContext.MemberProfile:
                    case (int) FeatureContext.Inbox:
                        editValidator = new PermissionValidator<int>(Principal, SherpaPermissionCapabilities.PROFILE,
                            SherpaPermission.Edit);
                        break;
                    case (int) FeatureContext.Onboarding:
                        editValidator = new PermissionValidator<int>(Principal, SherpaPermissionCapabilities.ONBOARDING,
                            SherpaPermission.Edit);
                        break;
                    case (int) FeatureContext.Recruitment:
                        editValidator = new PermissionValidator<int>(Principal, SherpaPermissionCapabilities.RECRUITMENT,
                            SherpaPermission.Edit);
                        break;
                    case (int) FeatureContext.Roster:
                        editValidator = new PermissionValidator<int>(Principal, SherpaPermissionCapabilities.ROSTERS,
                            SherpaPermission.Edit);
                        break;
                    case (int) FeatureContext.Event:
                        editValidator = new PermissionValidator<int>(Principal, SherpaPermissionCapabilities.EVENTS,
                            SherpaPermission.Edit);
                        break;
                    case (int) FeatureContext.Request:
                        editValidator = new PermissionValidator<int>(Principal,
                            SherpaPermissionCapabilities.STORE_REQUESTS, SherpaPermission.Edit);
                        break;
                    case (int) FeatureContext.Dashboard:
                    case (int) FeatureContext.TeamWall:
                    case (int) FeatureContext.Activity:
                        editValidator = new PermissionValidator<int>(Principal, SherpaPermissionCapabilities.NEWS,
                            SherpaPermission.Edit);
                        break;
                    default:
                        editValidator = new PermissionValidator<int>(Principal, SherpaPermissionCapabilities.LIBRARY,
                            SherpaPermission.Super);
                        break;
                }

                if (editValidator.IsSatisfyBy(int.MinValue) || Principal.GetMemberId() == item.SubmitterId)
                {
                    yield return ArtifactAction.UpdateArtifact(item.ArtifactId.ToString());
                    yield return ArtifactAction.DeleteArtifact(item.ArtifactId.ToString());
                }
            }
            else
            {
                ISpecification<int> editValidator = new PermissionValidator<int>(Principal,
                    SherpaPermissionCapabilities.LIBRARY, SherpaPermission.Super);
                if (editValidator.IsSatisfyBy(int.MinValue) || Principal.GetMemberId() == item.SubmitterId)
                {
                    yield return ArtifactAction.UpdateArtifact(item.ArtifactId.ToString());
                    yield return ArtifactAction.DeleteArtifact(item.ArtifactId.ToString());
                }
            }
        }

        public IEnumerable<EntityAction> GetArtifactDetailActions(ArtifactDetail item)
        {
            yield return ArtifactAction.ViewArtifactInfo(item.ArtifactId.ToString());
            yield return ArtifactAction.PostRelatedArtifacts(item.ArtifactId.ToString());
            yield return ArtifactAction.DownloadArtifactFile(item.ArtifactId.ToString());

            var fileExtension = Path.GetExtension(item.FileName);
            switch (item.ArtifactTypeName)
            {
                case "Document":
                    switch (fileExtension)
                    {
                        case ".doc":
                        case ".docx":
                        case ".pdf":
                        case ".html":
                        yield return ArtifactAction.LaunchArtifact(item.ArtifactId.ToString());
                        break;
                    }
                    break;
                case "PaySlip":
                case "Template":
                case "Image":
                    yield return ArtifactAction.LaunchArtifact(item.ArtifactId.ToString());
                    break;
                case "Audio":
                case "Video":
                    yield return ArtifactAction.LaunchArtifact(item.ArtifactId.ToString());
                    yield return ArtifactAction.StreamingArtifactFile(item.ArtifactId.ToString());
                    break;
            }

            if (item.Container.ContainerType == ContainerType.SystemDefined)
            {
                ISpecification<int> editValidator;
                switch (item.Container.ContainerId)
                {
                    case (int) FeatureContext.MemberProfile:
                    case (int) FeatureContext.Inbox:
                        editValidator = new PermissionValidator<int>(Principal, SherpaPermissionCapabilities.PROFILE,
                            SherpaPermission.Edit);
                        break;
                    case (int) FeatureContext.Onboarding:
                        editValidator = new PermissionValidator<int>(Principal, SherpaPermissionCapabilities.ONBOARDING,
                            SherpaPermission.Edit);
                        break;
                    case (int) FeatureContext.Recruitment:
                        editValidator = new PermissionValidator<int>(Principal, SherpaPermissionCapabilities.RECRUITMENT,
                            SherpaPermission.Edit);
                        break;
                    case (int) FeatureContext.Roster:
                        editValidator = new PermissionValidator<int>(Principal, SherpaPermissionCapabilities.ROSTERS,
                            SherpaPermission.Edit);
                        break;
                    case (int) FeatureContext.Event:
                        editValidator = new PermissionValidator<int>(Principal, SherpaPermissionCapabilities.EVENTS,
                            SherpaPermission.Edit);
                        break;
                    case (int) FeatureContext.Request:
                        editValidator = new PermissionValidator<int>(Principal,
                            SherpaPermissionCapabilities.STORE_REQUESTS, SherpaPermission.Edit);
                        break;
                    case (int) FeatureContext.Dashboard:
                    case (int) FeatureContext.TeamWall:
                    case (int) FeatureContext.Activity:
                        editValidator = new PermissionValidator<int>(Principal, SherpaPermissionCapabilities.NEWS,
                            SherpaPermission.Edit);
                        break;
                    default:
                        editValidator = new PermissionValidator<int>(Principal, SherpaPermissionCapabilities.LIBRARY,
                            SherpaPermission.Super);
                        break;
                }

                if (editValidator.IsSatisfyBy(int.MinValue) || Principal.GetMemberId() == item.SubmitterId)
                {
                    yield return ArtifactAction.UpdateArtifact(item.ArtifactId.ToString());

                    yield return ArtifactAction.DeleteArtifact(item.ArtifactId.ToString());
                }
            }
            else
            {
                ISpecification<int> editValidator = new PermissionValidator<int>(Principal,
                    SherpaPermissionCapabilities.LIBRARY, SherpaPermission.Super);
                if (editValidator.IsSatisfyBy(int.MinValue) || Principal.GetMemberId() == item.SubmitterId)
                {
                    yield return ArtifactAction.UpdateArtifact(item.ArtifactId.ToString());
                    yield return ArtifactAction.DeleteArtifact(item.ArtifactId.ToString());
                }
            }
        }

        public IEnumerable<EntityAction> GetRelatedArtifactSummaryListActions(ArtifactLinkDetail item)
        {
            yield return ArtifactAction.PostRelatedArtifacts(item.Artifact.ArtifactId.ToString());
        }

        public IEnumerable<EntityAction> GetRelatedArtifactSummaryListActions(ArtifactSummary source, ArtifactSummary destination)
        {
            yield return ArtifactAction.DeleteRelatedArtifact(source.ArtifactId.ToString(), destination.ArtifactId.ToString());
            yield return ArtifactAction.ViewArtifactInfo(destination.ArtifactId.ToString());
            yield return ArtifactAction.PostRelatedArtifacts(destination.ArtifactId.ToString());
            yield return ArtifactAction.DownloadArtifactFile(destination.ArtifactId.ToString());

            var fileExtension = Path.GetExtension(source.FileName);
            switch (source.ArtifactTypeName)
            {
                case "Document":
                    switch (fileExtension)
                    {
                        case ".doc":
                        case ".docx":
                        case ".pdf":
                        case ".html":
                            yield return ArtifactAction.LaunchArtifact(source.ArtifactId.ToString());
                            break;
                    }
                    break;
                case "PaySlip":
                case "Template":
                case "Image":
                    yield return ArtifactAction.LaunchArtifact(source.ArtifactId.ToString());
                    break;
                case "Audio":
                case "Video":
                    yield return ArtifactAction.LaunchArtifact(source.ArtifactId.ToString());
                    yield return ArtifactAction.StreamingArtifactFile(source.ArtifactId.ToString());
                    break;
            }

            if (destination.Container.ContainerType == ContainerType.SystemDefined)
            {
                ISpecification<int> editValidator;
                switch (destination.Container.ContainerId)
                {
                    case (int)FeatureContext.MemberProfile:
                    case (int)FeatureContext.Inbox:
                        editValidator = new PermissionValidator<int>(Principal, SherpaPermissionCapabilities.PROFILE,
                            SherpaPermission.Edit);
                        break;
                    case (int)FeatureContext.Onboarding:
                        editValidator = new PermissionValidator<int>(Principal, SherpaPermissionCapabilities.ONBOARDING,
                            SherpaPermission.Edit);
                        break;
                    case (int)FeatureContext.Recruitment:
                        editValidator = new PermissionValidator<int>(Principal, SherpaPermissionCapabilities.RECRUITMENT,
                            SherpaPermission.Edit);
                        break;
                    case (int)FeatureContext.Roster:
                        editValidator = new PermissionValidator<int>(Principal, SherpaPermissionCapabilities.ROSTERS,
                            SherpaPermission.Edit);
                        break;
                    case (int)FeatureContext.Event:
                        editValidator = new PermissionValidator<int>(Principal, SherpaPermissionCapabilities.EVENTS,
                            SherpaPermission.Edit);
                        break;
                    case (int)FeatureContext.Request:
                        editValidator = new PermissionValidator<int>(Principal,
                            SherpaPermissionCapabilities.STORE_REQUESTS, SherpaPermission.Edit);
                        break;
                    case (int)FeatureContext.Dashboard:
                    case (int)FeatureContext.TeamWall:
                    case (int)FeatureContext.Activity:
                        editValidator = new PermissionValidator<int>(Principal, SherpaPermissionCapabilities.NEWS,
                            SherpaPermission.Edit);
                        break;
                    default:
                        editValidator = new PermissionValidator<int>(Principal, SherpaPermissionCapabilities.LIBRARY,
                            SherpaPermission.Super);
                        break;
                }

                if (editValidator.IsSatisfyBy(int.MinValue) || Principal.GetMemberId() == destination.SubmitterId)
                {
                    yield return ArtifactAction.UpdateArtifact(destination.ArtifactId.ToString());

                    yield return ArtifactAction.DeleteArtifact(destination.ArtifactId.ToString());
                }
            }
            else
            {
                ISpecification<int> editValidator = new PermissionValidator<int>(Principal,
                    SherpaPermissionCapabilities.LIBRARY, SherpaPermission.Super);
                if (editValidator.IsSatisfyBy(int.MinValue) || Principal.GetMemberId() == destination.SubmitterId)
                {
                    yield return ArtifactAction.UpdateArtifact(destination.ArtifactId.ToString());
                    yield return ArtifactAction.DeleteArtifact(destination.ArtifactId.ToString());
                }
            }
        }

    }
}
