using System;
using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Services.Dtos.Contents.Documentation;

namespace Eagle.Services.Contents
{
    public interface IDocumentationService : IBaseService
    {
        IEnumerable<DocumentationInfoDetail> GetDocumentations(int vendorId, DocumentationStatus? status);
        DocumentationDetail InsertDocumentation(Guid applicationId, Guid userId, int vendorId, DocumentationEntry entry);
        void UpdateDocumentation(Guid applicationId, Guid userId, int vendorId, DocumentationEditEntry entry);
        void UpdateDocumentationStatus(Guid userId, int documentationId, DocumentationStatus status);
        void DeleteDocumentation(int documentationId);
        IEnumerable<DocumentationInfoDetail> Search(DocumentationSearchEntry searchEntry, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null);
        DocumentationInfoDetail GetDocumentationDetail(int id);
    }
}
