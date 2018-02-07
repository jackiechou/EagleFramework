using Eagle.Core.Settings;
using Eagle.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using Eagle.Entities.Contents.Documents;
using Eagle.Core.Common;
using Eagle.Entities.Business.Vendors;
using Eagle.Entities.SystemManagement.FileStorage;

namespace Eagle.Repositories.Contents
{
    public class DocumentationRepository : RepositoryBase<Documentation>, IDocumentationRepository
    {
        public DocumentationRepository(IDataContext databaseContext) : base(databaseContext) { }

        public IEnumerable<Documentation> GetList(int vendorId, DocumentationStatus? status)
        {
            return (from document in DataContext.Get<Documentation>()
                    where document.VendorId == vendorId && (status == null || document.Status == status)
                    select document).AsEnumerable();
        }

        public Documentation GetDocumentationsByFileId(int fileId)
        {
            return (from document in DataContext.Get<Documentation>()
                    where document.FileId == fileId
                    select document).FirstOrDefault();
        }

        public IEnumerable<DocumentationInfo> Search(out int recordCount, string searchText = null, DocumentationStatus? status = default(DocumentationStatus?), string orderBy = null, int? page = default(int?), int? pageSize = default(int?))
        {
            var queryable = from x in DataContext.Get<Documentation>()
                            join vendor in DataContext.Get<Vendor>() on x.VendorId equals vendor.VendorId
                            join file in DataContext.Get<DocumentFile>() on x.FileId equals file.FileId
                            where (status == null || x.Status == status)
                            select new DocumentationInfo
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
                                VendorName = vendor.VendorName,
                                FileName = file.FileName,
                            };

            if (!string.IsNullOrEmpty(searchText))
            {
                queryable = queryable.Where(x => x.FileName.ToLower().Contains(searchText.ToLower())
                //|| x.VendorName.ToLower().Contains(searchText.ToLower())
                );
            }

            return queryable.WithRecordCount(out recordCount)
                            .WithSortingAndPaging(orderBy, page, pageSize);
        }

        public DocumentationInfo GetDetails(int documentationId)
        {
            return (from x in DataContext.Get<Documentation>()
                    join vendor in DataContext.Get<Vendor>() on x.VendorId equals vendor.VendorId
                    join file in DataContext.Get<DocumentFile>() on x.FileId equals file.FileId
                    where x.DocumentationId == documentationId
                    select new DocumentationInfo
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
                        VendorName = vendor.VendorName,
                        FileName = file.FileName,
                    }).FirstOrDefault();
        }
    }
}
