using System.Collections.Generic;
using Eagle.Core.Settings;
using Eagle.Entities.Contents.Documents;
using Eagle.EntityFramework.Repositories;

namespace Eagle.Repositories.Contents
{
    public interface IDocumentationRepository : IRepositoryBase<Documentation>
    {
        IEnumerable<Documentation> GetList(int vendorId, DocumentationStatus? status);
        Documentation GetDocumentationsByFileId(int fileId);
        IEnumerable<DocumentationInfo> Search(out int recordCount, string searchText = null, DocumentationStatus? status = null, string orderBy = null, int? page = null, int? pageSize = null);
        DocumentationInfo GetDetails(int documentationId);
    }
}
