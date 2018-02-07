using System;
using System.Collections.Generic;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement.FileStorage;
using Eagle.Services.Validations;

namespace Eagle.Services.SystemManagement.Validation
{
    public class DocumentFileEntryValidator : SpecificationBase<DocumentFileEntry>
    {
        private static IUnitOfWork UnitOfWork { get; set; }
        private PermissionLevel PermissionLevel { get; set; }
        public DocumentFileEntryValidator(IUnitOfWork unitOfWork, PermissionLevel permissionLevel)
        {
            UnitOfWork = unitOfWork;
            PermissionLevel = permissionLevel;
        }

        protected override bool IsSatisfyBy(DocumentFileEntry data, IList<RuleViolation> violations = null)
        {
            if (data == null && violations != null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundDocumentFileEntry, "DocumentFileEntry"));
                return false;
            }
            ISpecification<DocumentFileEntry> hasValidIsValidFileName = new HasValidFileName();
            ISpecification<DocumentFileEntry> hasValidFolder = new HasValidFolder();

            return hasValidIsValidFileName.And(hasValidFolder).IsSatisfyBy(data, violations);
        }

        internal class HasValidFileName : SpecificationBase<DocumentFileEntry>
        {
            protected override bool IsSatisfyBy(DocumentFileEntry data, IList<RuleViolation> violations = null)
            {
                if (string.IsNullOrEmpty(data.FileName))
                {
                    if (violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NullFileName, "FileName", null, ErrorMessage.Messages[ErrorCode.NullFileName]));
                        return false;
                    }
                }
                else
                {
                    if (data.FileName.Length > 250)
                    {
                        if (violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidFileName, "FileName", null, ErrorMessage.Messages[ErrorCode.InvalidFileName]));
                            return false;
                        }
                    }   
                }
                return true;
            }
        }

        internal class HasValidFolder : SpecificationBase<DocumentFileEntry>
        {
            protected override bool IsSatisfyBy(DocumentFileEntry data, IList<RuleViolation> violations = null)
            {
                if (data.StorageType == StorageType.Local)
                {
                    if ((data.FolderId == null || data.FolderId == 0) && violations != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.NullFolder, "FolderId", data.FolderId,
                            ErrorMessage.Messages[ErrorCode.NullFolder]));
                        return false;
                    }
                    else
                    {
                        var item = UnitOfWork.DocumentFolderRepository.FindById(data.FolderId);
                        if (item == null && violations != null)
                        {
                            violations.Add(new RuleViolation(ErrorCode.InvalidFolder, "FolderId", data.FolderId,
                                ErrorMessage.Messages[ErrorCode.InvalidFolder]));
                            return false;
                        }
                        else
                        {
                            var isDuplicate = UnitOfWork.DocumentFileRepository.HasDataExisted(Convert.ToInt32(data.FolderId), data.FileName);
                            if (isDuplicate && violations != null)
                            {
                                violations.Add(new RuleViolation(ErrorCode.DuplicateDocumentFile, "FileName", data.FileName));
                                return false;
                            }
                        }
                    }
                }
                return true;
            }
        }
    }
}
