using Eagle.Common.Utilities;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Documents;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Documentation;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.Messaging;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eagle.Services.Contents
{
    public class DocumentationService : BaseService, IDocumentationService
    {
        public IApplicationService ApplicationService { get; set; }
        public ICommonService CommonService { get; set; }
        public IDocumentService DocumentService { get; set; }
        public IMailService MailService { get; set; }
        public IMessageService MessageService { get; set; }
        public INotificationService NotificationService { get; set; }
        
        public DocumentationService(IUnitOfWork unitOfWork, IApplicationService applicationService, ICommonService commonService, IDocumentService documentService, IMessageService messageService,
            INotificationService notificationService, IMailService mailService) : base(unitOfWork)
        {
            ApplicationService = applicationService;
            CommonService = commonService;
            DocumentService = documentService;
            MessageService = messageService;
            NotificationService = notificationService;
            MailService = mailService;
        }

        public IEnumerable<DocumentationInfoDetail> GetDocumentations(int vendorId, DocumentationStatus? status)
        {
            var documentations = UnitOfWork.DocumentationRepository.GetList(vendorId, status);
            if (documentations == null) return null;

            var lst = (from documentation in documentations
                       let documentInfo = DocumentService.GetFileInfoDetail(documentation.FileId)
                       select new DocumentationInfoDetail
                       {
                           DocumentationId = documentation.DocumentationId,
                           VendorId = documentation.VendorId,
                           FileId = documentation.FileId,
                           Status = documentation.Status,
                           DocumentInfo = documentInfo
                       }).AsEnumerable();

            return lst;
        }

        public DocumentationDetail InsertDocumentation(Guid applicationId, Guid userId, int vendorId, DocumentationEntry entry)
        {
            var entity = new Documentation
            {
                VendorId = vendorId,
                Status = DocumentationStatus.Active,
                CreatedByUserId = userId,
                CreatedDate = DateTime.UtcNow,
                Ip = NetworkUtils.GetIP4Address()
            };

            if (entry.File != null && entry.File.ContentLength > 0)
            {
                var fileInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId, entry.File, (int)FileLocation.Document, StorageType.Local);
                entity.FileId = fileInfo.FileId;
            }

            UnitOfWork.DocumentationRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            return entity.ToDto<Documentation, DocumentationDetail>();
        }

        public void UpdateDocumentation(Guid applicationId, Guid userId, int vendorId, DocumentationEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.DocumentationRepository.FindById(entry.DocumentationId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundDocumentation, "DocumentationId", entry.DocumentationId));
                throw new ValidationError(violations);
            }

            if (entry.File != null && entry.File.ContentLength > 0)
            {
                var existedFileId = entity.FileId;
                var fileInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId, entry.File, (int)FileLocation.Document, StorageType.Local);
                entity.FileId = fileInfo.FileId;

                entity.LastModifiedDate = DateTime.UtcNow;
                entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
                entity.LastModifiedByUserId = userId;

                UnitOfWork.DocumentationRepository.Update(entity);
                UnitOfWork.SaveChanges();

                // Delete old file
                DocumentService.DeleteFile(existedFileId);
            }

        }

        public void UpdateDocumentationStatus(Guid userId, int documentationId, DocumentationStatus status)
        {
            var violations = new List<RuleViolation>();
            var isValid = Enum.IsDefined(typeof(DocumentationStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundDocumentation, "DocumentationId", documentationId));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.DocumentationRepository.FindById(documentationId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundDocumentation, "DocumentationId", documentationId));
                throw new ValidationError(violations);
            }
            if (entity.Status == status) return;

            entity.Status = status;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastModifiedByUserId = userId;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            UnitOfWork.DocumentationRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void DeleteDocumentation(int documentationId)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.DocumentationRepository.FindById(documentationId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundDocumentation, "DocumentationId", documentationId));
                throw new ValidationError(violations);
            }

            UnitOfWork.DocumentationRepository.Delete(entity);
            UnitOfWork.SaveChanges();

            DocumentService.DeleteFile(entity.FileId);
        }

        public IEnumerable<DocumentationInfoDetail> Search(DocumentationSearchEntry searchEntry, out int recordCount, string orderBy = null, int? page = default(int?), int? pageSize = default(int?))
        {
            var documents = UnitOfWork.DocumentationRepository.Search(out recordCount, searchEntry.Keywords,
                searchEntry.Status, orderBy, page, pageSize).ToList();

            if (!documents.Any()) return null;

            var lst = documents.Select(x => new DocumentationInfoDetail
            {
                DocumentationId = x.DocumentationId,
                VendorId = x.VendorId,
                FileId = x.FileId,
                Status = x.Status,
                CreatedDate = x.CreatedDate,
                LastModifiedDate = x.LastModifiedDate,
                CreatedByUserId = x.CreatedByUserId,
                LastModifiedByUserId = x.LastModifiedByUserId,
                Ip = x.Ip,
                LastUpdatedIp = x.LastUpdatedIp,
                DocumentInfo = DocumentService.GetFileInfoDetail(x.FileId)
            }).ToList();

            return lst;
        }

        public DocumentationInfoDetail GetDocumentationDetail(int id)
        {
            var item = UnitOfWork.DocumentationRepository.GetDetails(id);
            var model = new DocumentationInfoDetail
            {
                DocumentationId = item.DocumentationId,
                VendorId = item.VendorId,
                FileId = item.FileId,
                Status = item.Status,
                CreatedDate = item.CreatedDate,
                LastModifiedDate = item.LastModifiedDate,
                CreatedByUserId = item.CreatedByUserId,
                LastModifiedByUserId = item.LastModifiedByUserId,
                Ip = item.Ip,
                LastUpdatedIp = item.LastUpdatedIp,
                DocumentInfo = DocumentService.GetFileInfoDetail(item.FileId)
            };

            return model;
        }

        #region Dispose

        private bool _disposed;
        protected override void Dispose(bool isDisposing)
        {
            if (!_disposed)
            {
                if (isDisposing)
                {
                    ApplicationService = null;
                    CommonService = null;
                    DocumentService = null;
                    MailService = null;
                    MessageService = null;
                    NotificationService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }
        #endregion
    }
}
