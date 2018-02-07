using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Eagle.Common.Utilities;
using Eagle.Core.Configuration;
using Eagle.Core.Extension;
using Eagle.Core.Permission;
using Eagle.Core.Settings;
using Eagle.Entities.Common;
using Eagle.Entities.Contents.Media;
using Eagle.Repositories;
using Eagle.Resources;
using Eagle.Services.Contents.Validation;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Contents.Media;
using Eagle.Services.Dtos.SystemManagement.FileStorage;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;
using NReco.VideoConverter;

namespace Eagle.Services.Contents
{
    public class MediaService : BaseService, IMediaService
    {
        private IDocumentService DocumentService { get; set; }

        public MediaService(IUnitOfWork unitOfWork, IDocumentService documentService) : base(unitOfWork)
        {
            DocumentService = documentService;
        }

        #region Album
        public IEnumerable<MediaAlbumInfoDetail> GetAlbums(MediaAlbumSearchEntry filter, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = new List<MediaAlbumInfoDetail>();
            var albums = UnitOfWork.MediaAlbumRepository.GetAlbums(filter.SearchText, filter.SearchTypeId, filter.SearchTopicId, filter.SearchStatus, ref recordCount, orderBy, page, pageSize);
            if (albums != null)
            {
                foreach (var item in albums)
                {
                    var album = new MediaAlbumInfoDetail
                    {
                        TypeId = item.AlbumId,
                        TopicId = item.TopicId,
                        AlbumId = item.AlbumId,
                        AlbumName = item.AlbumName,
                        AlbumAlias = item.AlbumAlias,
                        FrontImage = item.FrontImage,
                        MainImage = item.MainImage,
                        Description = item.Description,
                        TotalViews = item.TotalViews,
                        ListOrder = item.ListOrder,
                        Status = item.Status,
                        Type = item.Type.ToDto<MediaType, MediaTypeDetail>(),
                        Topic = item.Topic.ToDto<MediaTopic, MediaTopicDetail>(),
                    };

                    if (item.FrontImage != null && item.FrontImage > 0)
                    {
                        var frontImageInfo = DocumentService.GetFileInfoDetail(Convert.ToInt32(item.FrontImage));
                        if (frontImageInfo != null)
                        {
                            album.FrontImageUrl = frontImageInfo.FileUrl;
                            album.FrontImageInfo = frontImageInfo;
                        }
                        else
                        {
                            album.FrontImageUrl = GlobalSettings.NotFoundImageUrl;
                        }
                    }
                    else
                    {
                        album.FrontImageUrl = GlobalSettings.NotFoundImageUrl;
                    }

                    if (item.MainImage != null && item.MainImage > 0)
                    {
                        var mainImageInfo = DocumentService.GetFileInfoDetail(Convert.ToInt32(item.MainImage));
                        if (mainImageInfo != null)
                        {
                            album.MainImageUrl = mainImageInfo.FileUrl;
                            album.MainImageInfo = mainImageInfo;
                        }
                        else
                        {
                            album.MainImageUrl = GlobalSettings.NotFoundImageUrl;
                        }
                    }
                    else
                    {
                        album.MainImageUrl = GlobalSettings.NotFoundImageUrl;
                    }
                    lst.Add(album);
                }
            }
            return lst;
        }

        public IEnumerable<MediaAlbumInfoDetail> GetAlbums(int? typeId, int? topicId, MediaAlbumStatus? status)
        {
            var lst = new List<MediaAlbumInfoDetail>();
            var albums = UnitOfWork.MediaAlbumRepository.GetAlbums(typeId, topicId, status).ToList();
            if (albums.Any())
            {
                foreach (var item in albums)
                {
                    var album = new MediaAlbumInfoDetail
                    {
                        TypeId = item.TypeId,
                        TopicId = item.TopicId,
                        AlbumId = item.AlbumId,
                        AlbumName = item.AlbumName,
                        AlbumAlias = item.AlbumAlias,
                        FrontImage = item.FrontImage,
                        MainImage = item.MainImage,
                        Description = item.Description,
                        TotalViews = item.TotalViews,
                        ListOrder = item.ListOrder,
                        Status = item.Status,
                        Type = item.Type.ToDto<MediaType, MediaTypeDetail>(),
                        Topic = item.Topic.ToDto<MediaTopic, MediaTopicDetail>()
                    };

                    if (item.FrontImage != null && item.FrontImage > 0)
                    {
                        var frontImageInfo = DocumentService.GetFileInfoDetail(Convert.ToInt32(item.FrontImage));
                        if (frontImageInfo != null)
                        {
                            album.FrontImageUrl = frontImageInfo.FileUrl;
                            album.FrontImageInfo = frontImageInfo;
                        }
                        else
                        {
                            album.FrontImageUrl = GlobalSettings.NotFoundImageUrl;
                        }
                    }
                    else
                    {
                        album.FrontImageUrl = GlobalSettings.NotFoundImageUrl;
                    }

                    if (item.MainImage != null && item.MainImage > 0)
                    {
                        var mainImageInfo = DocumentService.GetFileInfoDetail(Convert.ToInt32(item.MainImage));
                        if (mainImageInfo != null)
                        {
                            album.MainImageUrl = mainImageInfo.FileUrl;
                            album.MainImageInfo = mainImageInfo;
                        }
                        else
                        {
                            album.MainImageUrl = GlobalSettings.NotFoundImageUrl;
                        }
                    }
                    else
                    {
                        album.MainImageUrl = GlobalSettings.NotFoundImageUrl;
                    }
                    
                    lst.Add(album);
                }
            }
            return lst;
        }

        public MultiSelectList PoplulateMediaAlbumMultiSelectList(int typeId, int topicId, MediaAlbumStatus? status = null, bool? isShowSelectText = null,
            int[] selectedValues = null)
        {
            return UnitOfWork.MediaAlbumRepository.PoplulateMediaAlbumMultiSelectList(typeId, topicId, status, isShowSelectText,
           selectedValues);
        }

        public MediaAlbumInfoDetail GetAlbumDetail(int id)
        {
            var item = UnitOfWork.MediaAlbumRepository.GeDetails(id);
            if (item == null) return null;

            var album = new MediaAlbumInfoDetail
            {
                TypeId = item.TypeId,
                TopicId = item.TopicId,
                AlbumId = item.AlbumId,
                AlbumName = item.AlbumName,
                AlbumAlias = item.AlbumAlias,
                FrontImage = item.FrontImage,
                MainImage = item.MainImage,
                Description = item.Description,
                TotalViews = item.TotalViews,
                ListOrder = item.ListOrder,
                Status = item.Status,
                Type = item.Type.ToDto<MediaType, MediaTypeDetail>(),
                Topic = item.Topic.ToDto<MediaTopic, MediaTopicDetail>(),
            };

            if (item.FrontImage != null && item.FrontImage > 0)
            {
                var frontImageInfo = DocumentService.GetFileInfoDetail(Convert.ToInt32(item.FrontImage));
                if (frontImageInfo != null)
                {
                    album.FrontImageUrl = frontImageInfo.FileUrl;
                    album.FrontImageInfo = frontImageInfo;
                }
                else
                {
                    album.FrontImageUrl = GlobalSettings.NotFoundImageUrl;
                }
            }
            else
            {
                album.FrontImageUrl = GlobalSettings.NotFoundImageUrl;
            }

            if (item.MainImage != null && item.MainImage > 0)
            {
                var mainImageInfo = DocumentService.GetFileInfoDetail(Convert.ToInt32(item.MainImage));
                if (mainImageInfo != null)
                {
                    album.MainImageUrl = mainImageInfo.FileUrl;
                    album.MainImageInfo = mainImageInfo;
                }
                else
                {
                    album.MainImageUrl = GlobalSettings.NotFoundImageUrl;
                }
            }
            else
            {
                album.MainImageUrl = GlobalSettings.NotFoundImageUrl;
            }
            return album;
        }
        public MediaAlbumDetail InsertAlbum(Guid applicationId, Guid userId, MediaAlbumEntry entry)
        {
            ISpecification<MediaAlbumEntry> validator = new MediaAlbumEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var violations = new List<RuleViolation>();
            if (!validator.IsSatisfyBy(entry, violations)) throw new ValidationError(violations);

            var entity = entry.ToEntity<MediaAlbumEntry, MediaAlbum>();
            entity.ListOrder = UnitOfWork.MediaAlbumRepository.GetNewListOrder();
            entity.Ip = NetworkUtils.GetIP4Address();
            entity.CreatedDate = DateTime.UtcNow;
            entity.CreatedByUserId = userId;

            if (entry.FileUpload != null && entry.FileUpload.ContentLength > 0)
            {
                int maxContentLength = UnitOfWork.ApplicationSettingRepository.GetAllowedMaxImageContentLength(applicationId);
                string[] allowedFileExtensions = UnitOfWork.ApplicationSettingRepository.GetAllowedImageExtensions(applicationId);

                if (!allowedFileExtensions.Contains(entry.FileUpload.FileName.Substring(entry.FileUpload.FileName.LastIndexOf('.'))))
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidFileType, "FileUpload", LanguageResource.InvalidFileExtension + " : " + string.Join(", ", allowedFileExtensions)));
                    throw new ValidationError(violations);
                }
                else if (entry.FileUpload.ContentLength > maxContentLength)
                {
                    violations.Add(new RuleViolation(ErrorCode.MaximumAllowedSize, "FileUpload", LanguageResource.InvalidFileSize + " , " + LanguageResource.MaximumAllowedSize + maxContentLength + " MB"));
                    throw new ValidationError(violations);
                }
                else
                {
                    var fileIds = DocumentService.UploadAndSaveWithThumbnail(applicationId, userId, entry.FileUpload, (int)FileLocation.Album, StorageType.Local);
                    entity.MainImage = fileIds[0].FileId;
                    entity.FrontImage = fileIds[1].FileId;
                }
            }

            UnitOfWork.MediaAlbumRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            return entity.ToDto<MediaAlbum, MediaAlbumDetail>();
        }
        public void UpdateAlbum(Guid applicationId, Guid userId, MediaAlbumEditEntry entry)
        {
            ISpecification<MediaAlbumEditEntry> validator = new MediaAlbumEditEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var violations = new List<RuleViolation>();
            if (!validator.IsSatisfyBy(entry, violations)) throw new ValidationError(violations);

            var entity = UnitOfWork.MediaAlbumRepository.FindById(entry.AlbumId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidAlbumId, "AlbumId", entry.AlbumId, ErrorMessage.Messages[ErrorCode.InvalidAlbumId]));
                throw new ValidationError(violations);
            }

            entity.TypeId = entry.TypeId;
            entity.TopicId = entry.TopicId;
            entity.AlbumName = entry.AlbumName;
            entity.AlbumAlias = StringUtils.ConvertTitle2Alias(entry.AlbumName);
            entity.Description = entry.Description;
            entity.Status = entry.Status;
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();

            if (entry.FileUpload != null && entry.FileUpload.ContentLength > 0)
            {
                int maxContentLength = UnitOfWork.ApplicationSettingRepository.GetAllowedMaxImageContentLength(applicationId);
                string[] allowedFileExtensions = UnitOfWork.ApplicationSettingRepository.GetAllowedImageExtensions(applicationId);

                if (!allowedFileExtensions.Contains(entry.FileUpload.FileName.Substring(entry.FileUpload.FileName.LastIndexOf('.'))))
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidFileType, "FileUpload", LanguageResource.InvalidFileExtension + " : " + string.Join(", ", allowedFileExtensions)));
                    throw new ValidationError(violations);
                }
                else if (entry.FileUpload.ContentLength > maxContentLength)
                {
                    violations.Add(new RuleViolation(ErrorCode.MaximumAllowedSize, "FileUpload", LanguageResource.InvalidFileSize + " , " + LanguageResource.MaximumAllowedSize + maxContentLength + " MB"));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entity.FrontImage != null)
                    {
                        DocumentService.DeleteFile(Convert.ToInt32(entity.FrontImage));
                    }

                    if (entity.MainImage != null)
                    {
                        DocumentService.DeleteFile(Convert.ToInt32(entity.MainImage));
                    }

                    var fileIds = DocumentService.UploadAndSaveWithThumbnail(applicationId, userId, entry.FileUpload, (int)FileLocation.Album, StorageType.Local);
                    entity.MainImage = fileIds[0].FileId;
                    entity.FrontImage = fileIds[1].FileId;
                }
            }

            UnitOfWork.MediaAlbumRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateAlbumStatus(Guid userId, int id, MediaAlbumStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.MediaAlbumRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidAlbumId, "AlbumId", id, ErrorMessage.Messages[ErrorCode.InvalidAlbumId]));
                throw new ValidationError(violations);
            }
            if (entity.Status == status) return;

            entity.Status = status;
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();

            UnitOfWork.MediaAlbumRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateAlbumTotalViews(Guid userId, int id)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.MediaAlbumRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidAlbumId, "AlbumId", id, ErrorMessage.Messages[ErrorCode.NotFoundMediaAlbum]));
                throw new ValidationError(violations);
            }

            entity.TotalViews = UnitOfWork.MediaAlbumRepository.GetNewListOrder();
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastModifiedByUserId = userId;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();

            UnitOfWork.MediaAlbumRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateAlbumListOrder(Guid userId, int id, int listOrder)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.MediaAlbumRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidAlbumId, "AlbumId", id, ErrorMessage.Messages[ErrorCode.NotFoundMediaAlbum]));
                throw new ValidationError(violations);
            }

            entity.ListOrder = listOrder;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastModifiedByUserId = userId;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();

            UnitOfWork.MediaAlbumRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Composer - Author
        public IEnumerable<MediaComposerDetail> GetComposers(MediaComposerSearchEntry filter, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.MediaComposerRepository.GetMediaComposers(filter.SearchText, filter.Status, ref recordCount, orderBy, page, pageSize);
            return lst.ToDtos<MediaComposer, MediaComposerDetail>();
        }
        public SelectList PoplulateMediaComposerSelectList(int? selectedValue, bool? isShowSelectText = null, MediaComposerStatus? status = null)
        {
            var listItems = new List<SelectListItem>();
            var lst = UnitOfWork.MediaComposerRepository.GetMediaComposers(MediaComposerStatus.Active).ToList();
            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem { Text = p.ComposerName, Value = p.ComposerId.ToString() }).ToList();
                if (isShowSelectText != null && isShowSelectText == true)
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.Select} ---", Value = "" });
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }
        public MediaComposerDetail GetComposerDetail(int id)
        {
            var entity = UnitOfWork.MediaComposerRepository.FindById(id);
            return entity.ToDto<MediaComposer, MediaComposerDetail>();
        }
        public MediaComposerDetail InsertComposer(Guid userId, MediaComposerEntry entry)
        {
            ISpecification<MediaComposerEntry> validator = new MediaComposerEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var violations = new List<RuleViolation>();
            if (!validator.IsSatisfyBy(entry, violations)) throw new ValidationError(violations);

            var entity = entry.ToEntity<MediaComposerEntry, MediaComposer>();
            entity.ComposerAlias = StringUtils.ConvertTitle2Alias(entry.ComposerName);
            entity.ListOrder = UnitOfWork.MediaAlbumRepository.GetNewListOrder();
            entity.Ip = NetworkUtils.GetIP4Address();
            entity.CreatedByUserId = userId;
            entity.CreatedDate = DateTime.UtcNow;

            UnitOfWork.MediaComposerRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<MediaComposer, MediaComposerDetail>();
        }
        public void UpdateComposer(Guid userId, MediaComposerEditEntry entry)
        {
            //Check validation
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMediaComposerEditEntry, "MediaComposerEditEntry", null, ErrorMessage.Messages[ErrorCode.NotFoundMediaComposerEditEntry]));
                throw new ValidationError(violations);
            }
            var entity = UnitOfWork.MediaComposerRepository.FindById(entry.ComposerId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMediaComposer, "MediaComposer", entry.ComposerId, ErrorMessage.Messages[ErrorCode.NotFoundMediaComposer]));
                throw new ValidationError(violations);
            }

            entity.ComposerName = entry.ComposerName;
            entity.ComposerAlias = StringUtils.ConvertTitle2Alias(entry.ComposerName);
            entity.Photo = entry.Photo;
            entity.Description = entry.Description;
            entity.Status = entry.Status;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.MediaComposerRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateComposerStatus(Guid userId, int id, MediaComposerStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.MediaComposerRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMediaComposer, "MediaComposer", id, ErrorMessage.Messages[ErrorCode.NotFoundMediaComposer]));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(MediaComposerStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", null,
                    ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.Status == status) return;

            entity.Status = status;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.MediaComposerRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateComposerListOrder(Guid userId, int id, int listOrder)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.MediaComposerRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMediaComposer, "MediaComposer", id, ErrorMessage.Messages[ErrorCode.NotFoundMediaComposer]));
                throw new ValidationError(violations);
            }

            entity.ListOrder = listOrder;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.MediaComposerRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Artist
        public IEnumerable<MediaArtistDetail> GetArtists(MediaArtistSearchEntry filter, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.MediaArtistRepository.GetMediaArtists(filter.SearchText, filter.Status, ref recordCount, orderBy, page, pageSize);
            return lst.ToDtos<MediaArtist, MediaArtistDetail>();
        }
        public MediaArtistDetail GetArtistDetail(int id)
        {
            var entity = UnitOfWork.MediaArtistRepository.FindById(id);
            return entity.ToDto<MediaArtist, MediaArtistDetail>();
        }
        public MediaArtistDetail InsertArtist(Guid userId, MediaArtistEntry entry)
        {
            ISpecification<MediaArtistEntry> validator = new MediaArtistEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var violations = new List<RuleViolation>();
            if (!validator.IsSatisfyBy(entry, violations)) throw new ValidationError(violations);

            var entity = entry.ToEntity<MediaArtistEntry, MediaArtist>();
            entity.ArtistAlias = StringUtils.ConvertTitle2Alias(entry.ArtistName);
            entity.ListOrder = UnitOfWork.MediaAlbumRepository.GetNewListOrder();
            entity.CreatedByUserId = userId;
            entity.Ip = NetworkUtils.GetIP4Address();

            UnitOfWork.MediaArtistRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<MediaArtist, MediaArtistDetail>();
        }
        public void UpdateArtist(Guid userId, MediaArtistEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMediaArtistEditEntry, "MediaArtistEditEntry", null, ErrorMessage.Messages[ErrorCode.NotFoundMediaArtistEditEntry]));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.MediaArtistRepository.FindById(entry.ArtistId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMediaArtist, "MediaArtist", entry.ArtistId, ErrorMessage.Messages[ErrorCode.NotFoundMediaArtist]));
                throw new ValidationError(violations);
            }

            entity.ArtistName = entry.ArtistName;
            entity.ArtistAlias = StringUtils.ConvertTitle2Alias(entry.ArtistName);
            entity.Photo = entry.Photo;
            entity.Description = entry.Description;
            entity.Status = entry.Status;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.MediaArtistRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateArtistStatus(Guid userId, int id, MediaArtistStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.MediaArtistRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMediaArtist, "MediaArtist", id, ErrorMessage.Messages[ErrorCode.NotFoundMediaArtist]));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(MediaArtistStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", null,
                    ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.Status == status) return;

            entity.Status = status;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.MediaArtistRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateArtistListOrder(Guid userId, int id, int listOrder)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.MediaArtistRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMediaArtist, "MediaArtist", id, ErrorMessage.Messages[ErrorCode.NotFoundMediaArtist]));
                throw new ValidationError(violations);
            }

            entity.ListOrder = listOrder;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.MediaArtistRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Media File
        public SelectList PopulateMediaFileTypes(string selectedValue = null, bool? isShowSelectText = false)
        {
            return UnitOfWork.MediaFileRepository.PopulateMediaFileTypes(selectedValue, isShowSelectText);
        }
        public List<MediaFileInfoDetail> GetMediaFiles(MediaFileSearchEntry filter, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = new List<MediaFileInfoDetail>();
            var mediaFiles = UnitOfWork.MediaFileRepository.GetMediaFiles(filter.SearchText, filter.SearchTypeId, filter.SearchTopicId, filter.SearchStatus, ref recordCount, orderBy, page, pageSize).ToList();
            if (mediaFiles.Any())
            {
                foreach (var item in mediaFiles)
                {
                    var mediaFile = new MediaFileInfoDetail
                    {
                        MediaId = item.MediaId,
                        FileId = item.FileId,
                        TypeId = item.TypeId,
                        TopicId = item.TopicId,
                        ComposerId = item.ComposerId,
                        Artist = item.Artist,
                        AutoStart = item.AutoStart,
                        MediaLoop = item.MediaLoop,
                        Lyric = item.Lyric,
                        SmallPhoto = item.SmallPhoto,
                        LargePhoto = item.LargePhoto,
                        ListOrder = item.ListOrder,
                        Type = item.Type.ToDto<MediaType, MediaTypeDetail>(),
                        Topic = item.Topic.ToDto<MediaTopic, MediaTopicDetail>(),
                        Composer = item.Composer.ToDto<MediaComposer, MediaComposerDetail>(),
                        DocumentFileInfo = DocumentService.GetFileInfoDetail(item.FileId)
                    };

                    if (item.SmallPhoto != null && item.SmallPhoto > 0)
                    {
                        var smallPhotoInfo = DocumentService.GetFileInfoDetail(Convert.ToInt32(item.SmallPhoto));
                        if (smallPhotoInfo != null)
                        {
                            mediaFile.SmallPhotoUrl = smallPhotoInfo.FileUrl;
                            mediaFile.SmallPhotoInfo = smallPhotoInfo;
                        }
                        else
                        {
                            mediaFile.SmallPhotoUrl = GlobalSettings.NotFoundImageUrl;
                        }
                    }
                    else
                    {
                        mediaFile.SmallPhotoUrl = GlobalSettings.NotFoundImageUrl;
                    }

                    if (item.LargePhoto != null && item.LargePhoto > 0)
                    {
                        var largePhotoInfo = DocumentService.GetFileInfoDetail(Convert.ToInt32(item.LargePhoto));
                        if (largePhotoInfo != null)
                        {
                            mediaFile.LargePhotoUrl = largePhotoInfo.FileUrl;
                            mediaFile.LargePhotoInfo = largePhotoInfo;
                        }
                        else
                        {
                            mediaFile.LargePhotoUrl = GlobalSettings.NotFoundImageUrl;
                        }
                    }
                    else
                    {
                        mediaFile.LargePhotoUrl = GlobalSettings.NotFoundImageUrl;
                    }

                    lst.Add(mediaFile);
                }
            }
            return lst;
        }

        public List<MediaFileInfoDetail> GetMediaFiles(int? typeId, int? topicId, DocumentFileStatus? status)
        {
            var lst = new List<MediaFileInfoDetail>();
            var mediaFiles = UnitOfWork.MediaFileRepository.GetMediaFiles(typeId, topicId, status).ToList();
            if (mediaFiles.Any())
            {
                foreach (var item in mediaFiles)
                {
                    var mediaFile = new MediaFileInfoDetail
                    {
                        MediaId = item.MediaId,
                        FileId = item.FileId,
                        TypeId = item.TypeId,
                        TopicId = item.TopicId,
                        ComposerId = item.ComposerId,
                        Artist = item.Artist,
                        AutoStart = item.AutoStart,
                        MediaLoop = item.MediaLoop,
                        Lyric = item.Lyric,
                        SmallPhoto = item.SmallPhoto,
                        LargePhoto = item.LargePhoto,
                        ListOrder = item.ListOrder,
                        Type = item.Type.ToDto<MediaType, MediaTypeDetail>(),
                        Topic = item.Topic.ToDto<MediaTopic, MediaTopicDetail>(),
                        Composer = item.Composer.ToDto<MediaComposer, MediaComposerDetail>(),
                        DocumentFileInfo = DocumentService.GetFileInfoDetail(item.FileId)
                    };

                    if (item.SmallPhoto != null && item.SmallPhoto > 0)
                    {
                        var smallPhotoInfo = DocumentService.GetFileInfoDetail(Convert.ToInt32(item.SmallPhoto));
                        if (smallPhotoInfo != null)
                        {
                            mediaFile.SmallPhotoUrl = smallPhotoInfo.FileUrl;
                            mediaFile.SmallPhotoInfo = smallPhotoInfo;
                        }
                        else
                        {
                            mediaFile.SmallPhotoUrl = GlobalSettings.NotFoundImageUrl;
                        }
                    }
                    else
                    {
                        mediaFile.SmallPhotoUrl = GlobalSettings.NotFoundImageUrl;
                    }

                    if (item.LargePhoto != null && item.LargePhoto > 0)
                    {
                        var largePhotoInfo = DocumentService.GetFileInfoDetail(Convert.ToInt32(item.LargePhoto));
                        if (largePhotoInfo != null)
                        {
                            mediaFile.LargePhotoUrl = largePhotoInfo.FileUrl;
                            mediaFile.LargePhotoInfo = largePhotoInfo;
                        }
                        else
                        {
                            mediaFile.LargePhotoUrl = GlobalSettings.NotFoundImageUrl;
                        }
                    }
                    else
                    {
                        mediaFile.LargePhotoUrl = GlobalSettings.NotFoundImageUrl;
                    }

                    lst.Add(mediaFile);
                }
            }
            return lst;
        }

        public List<MediaAlbumFileInfoDetail> GetMediaFilesByAlbumId(int albumId, MediaAlbumFileStatus? status = null)
        {
            var albumFiles = UnitOfWork.MediaAlbumFileRepository.GetListByAlbumId(albumId, status).ToList();
            if (!albumFiles.Any()) return null;

            return albumFiles.Select(albumFile => new MediaAlbumFileInfoDetail
            {
                AlbumId = albumFile.AlbumId,
                FileId = albumFile.FileId,
                ListOrder = albumFile.ListOrder,
                Status = albumFile.Status,
                Album = GetAlbumDetail(albumFile.AlbumId),
                File = GetMediaFileInfoDetailByFileId(albumFile.FileId)
            }).ToList();
        }

        public List<MediaPlayListFileInfoDetail> GetMediaFilesByPlayListId(int playListId, MediaPlayListFileStatus? status = null)
        {
            var playListFiles = UnitOfWork.MediaPlayListFileRepository.GetListByPlayListId(playListId, status).ToList();
            if (!playListFiles.Any()) return null;

            return playListFiles.Select(playListFile => new MediaPlayListFileInfoDetail
            {
                PlayListId = playListFile.PlayListId,
                FileId = playListFile.FileId,
                ListOrder = playListFile.ListOrder,
                Status = playListFile.Status,
                PlayList = GetPlayListDetail(playListFile.PlayListId),
                File = GetMediaFileInfoDetailByFileId(playListFile.FileId)
            }).ToList();
        }

        public MediaFileDetail GetMediaFileDetail(int mediaId)
        {
            var entity = UnitOfWork.MediaFileRepository.FindById(mediaId);
            return entity.ToDto<MediaFile, MediaFileDetail>();
        }

        public MediaFileInfoDetail GetMediaFileInfoDetail(int mediaId)
        {
            var entity = UnitOfWork.MediaFileRepository.GetDetails(mediaId);
            return new MediaFileInfoDetail
            {
                MediaId = entity.MediaId,
                FileId = entity.FileId,
                TypeId = entity.TypeId,
                TopicId = entity.TopicId,
                Artist = entity.Artist,
                ComposerId = entity.ComposerId,
                AutoStart = entity.AutoStart,
                MediaLoop = entity.MediaLoop,
                Lyric = entity.Lyric,
                SmallPhoto = entity.SmallPhoto,
                LargePhoto = entity.LargePhoto,
                ListOrder = entity.ListOrder,
                Type = entity.Type.ToDto<MediaType, MediaTypeDetail>(),
                Topic = entity.Topic.ToDto<MediaTopic, MediaTopicDetail>(),
                Composer = entity.Composer.ToDto<MediaComposer, MediaComposerDetail>(),
                DocumentFileInfo = DocumentService.GetFileInfoDetail(entity.FileId)
            };
        }

        public MediaFileInfoDetail GetMediaFileInfoDetailByFileId(int fileId)
        {
            var entity = UnitOfWork.MediaFileRepository.GetDetailsByFileId(fileId);
            if (entity == null) return null;

            var mediaInfo = new MediaFileInfoDetail
            {
                MediaId = entity.MediaId,
                FileId = entity.FileId,
                TypeId = entity.TypeId,
                TopicId = entity.TopicId,
                Artist = entity.Artist,
                ComposerId = entity.ComposerId,
                AutoStart = entity.AutoStart,
                MediaLoop = entity.MediaLoop,
                Lyric = entity.Lyric,
                SmallPhoto = entity.SmallPhoto,
                LargePhoto = entity.LargePhoto,
                ListOrder = entity.ListOrder,
                Type = entity.Type.ToDto<MediaType, MediaTypeDetail>(),
                Topic = entity.Topic.ToDto<MediaTopic, MediaTopicDetail>(),
                Composer = entity.Composer.ToDto<MediaComposer, MediaComposerDetail>(),
                DocumentFileInfo = DocumentService.GetFileInfoDetail(entity.FileId)
            };

            if (entity.SmallPhoto != null && entity.SmallPhoto > 0)
            {
                mediaInfo.SmallPhotoInfo = DocumentService.GetFileInfoDetail(Convert.ToInt32(entity.SmallPhoto));
            }

            if (entity.LargePhoto != null && entity.LargePhoto > 0)
            {
                mediaInfo.LargePhotoInfo = DocumentService.GetFileInfoDetail(Convert.ToInt32(entity.LargePhoto));
            }

            return mediaInfo;
        }

        private void CreateAlbumFiles(int fileId, List<int> albumIds)
        {
            if (albumIds == null || !albumIds.Any()) return;
            foreach (var albumId in albumIds)
            {
                var albumFile = new MediaAlbumFile
                {
                    AlbumId = albumId,
                    FileId = fileId,
                    ListOrder = UnitOfWork.MediaAlbumFileRepository.GetNewListOrder(),
                    Status = MediaAlbumFileStatus.Active
                };
                UnitOfWork.MediaAlbumFileRepository.Insert(albumFile);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteMediaPlayListFiles(int fileId, List<int> playListIds)
        {
            if (playListIds != null && fileId > 0)
            {
                foreach (var playListId in playListIds)
                {
                    var entity = UnitOfWork.MediaPlayListFileRepository.GetDetails(playListId, fileId);
                    if (entity != null)
                    {
                        UnitOfWork.MediaPlayListFileRepository.Delete(entity);
                    }
                }
                UnitOfWork.SaveChanges();
            }
        }
        private void CreatePlayListFiles(int fileId, List<int> playListIds)
        {
            if (playListIds == null || !playListIds.Any()) return;
            foreach (var playListId in playListIds)
            {
                var albumFile = new MediaPlayListFile
                {
                    PlayListId = playListId,
                    FileId = fileId,
                    ListOrder = UnitOfWork.MediaPlayListFileRepository.GetNewListOrder(),
                    Status = MediaPlayListFileStatus.Active
                };
                UnitOfWork.MediaPlayListFileRepository.Insert(albumFile);
                UnitOfWork.SaveChanges();
            }
        }
        public void DeleteMediaAlbumFiles(int fileId, List<int> albumIds)
        {
            if (albumIds != null && fileId > 0)
            {
                foreach (var albumId in albumIds)
                {
                    var entity = UnitOfWork.MediaAlbumFileRepository.GetDetails(albumId, fileId);
                    if (entity != null)
                    {
                        UnitOfWork.MediaAlbumFileRepository.Delete(entity);
                    }
                }
                UnitOfWork.SaveChanges();
            }
        }

        public int[] UploadVideoThumbnail(Guid applicationId, Guid? userId, int videoFileId)
        {
            var violations = new List<RuleViolation>();
            try
            {
                int[] result = new int[2];
                if (videoFileId <= 0)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidFileId, "FileId", videoFileId));
                    throw new ValidationError(violations);
                }

                var videoInfo = DocumentService.GetFileInfoDetail(videoFileId);
                if (videoInfo == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.NotFoundForDocumentFile, "DocumentFile", videoFileId));
                    throw new ValidationError(violations);
                }

                string videoPath = HttpContext.Current.Server.MapPath("~" + videoInfo.FileUrl);
                var videoFileInfo = new FileInfo(videoPath);
                if (!videoFileInfo.Exists)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidVideoPath, "FileUrl", videoPath));
                    throw new ValidationError(violations);
                }

                int videoThumbnailFolderId = Convert.ToInt32(FileLocation.VideoThumbnail);
                var videoThumbnailFolderEntity = UnitOfWork.DocumentFolderRepository.FindById(videoThumbnailFolderId);
                if (videoThumbnailFolderEntity == null)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidFolderId, "FolderId", videoThumbnailFolderId));
                    throw new ValidationError(violations);
                }

                string virtualVideoThumbPath = $"~{videoThumbnailFolderEntity.FolderPath}";
                string physicalVideoThumbPath = HttpContext.Current.Server.MapPath(virtualVideoThumbPath);
                if (!Directory.Exists(physicalVideoThumbPath))
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidFolder, "VideoThumbPath", virtualVideoThumbPath));
                    throw new ValidationError(violations);
                }

                string videoNameWithoutExtension = Path.GetFileNameWithoutExtension(videoInfo.FileName);
                string videoAlias = StringUtils.ConvertTitle2Alias(videoNameWithoutExtension);
                string videoThumbnailName = $"{videoAlias}-thumb.jpg";
                string videoThumbnailPath = Path.Combine(physicalVideoThumbPath, videoThumbnailName);

                // Generate large thumbnail image
                var ffMpeg = new FFMpegConverter();
                ffMpeg.GetVideoThumbnail(videoPath, videoThumbnailPath, 10); //extract the frame 10 second

                // Save small image====================================================================
                var videoThumbnailFileInfo = new FileInfo(videoThumbnailPath);
                if (videoThumbnailFileInfo.Exists)
                {
                    string videoSmallPhotoName = $"{videoAlias}.jpg";
                    string videoSmallPhotoPath = Path.Combine(physicalVideoThumbPath, videoSmallPhotoName);

                    //Create small image from large thumbnail path
                    //System.Drawing.Image image = System.Drawing.Image.FromFile(videoThumbnailPath);
                    //image.Save(videoSmallPhotoName);

                    FileUtils.ResizeImage(videoThumbnailPath, videoSmallPhotoPath, (int)GlobalSettings.DefaultThumbnailWidth, (int)GlobalSettings.DefaultThumbnailHeight);
                    //image.Dispose();

                    //save small photo
                    var videoSmallPhoto = DocumentService.InsertFile(applicationId, userId, new DocumentFileEntry
                    {
                        FileName = videoSmallPhotoName,
                        FileTitle = videoNameWithoutExtension,
                        FolderId = videoThumbnailFolderId,
                        StorageType = StorageType.Local,
                        FileDescription = videoInfo.FileDescription,
                        FileSource = videoInfo.FileSource
                    });

                    //save large thumbnail
                    var videoThumbnail = DocumentService.InsertFile(applicationId, userId, new DocumentFileEntry
                    {
                        FileName = videoThumbnailName,
                        FileTitle = Path.GetFileNameWithoutExtension(videoThumbnailName),
                        FolderId = videoThumbnailFolderId,
                        StorageType = StorageType.Local,
                        FileDescription = videoInfo.FileDescription,
                        FileSource = videoInfo.FileSource
                    });

                    result[0] = videoSmallPhoto.FileId;
                    result[1] = videoThumbnail.FileId;
                }
                return result;
            }
            catch (FFMpegException ex)
            {
                violations.Add(new RuleViolation(ErrorCode.UnableToGetVideoThumbnail, "FFMpeg", ex.ToString()));
                throw new ValidationError(violations);
            }
        }

        public void InsertMediaFile(Guid applicationId, Guid userId, MediaFileEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullMediaFileEntry, "MediaFileEntry", null, ErrorMessage.Messages[ErrorCode.NullMediaFileEntry]));
                throw new ValidationError(violations);
            }

            if (entry.TypeId <= 0)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidTypeId, "TypeId", entry.TypeId, ErrorMessage.Messages[ErrorCode.InvalidTypeId]));
                throw new ValidationError(violations);
            }

            if (entry.TopicId <= 0)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidTopicId, "TopicId", entry.TopicId, ErrorMessage.Messages[ErrorCode.InvalidTopicId]));
                throw new ValidationError(violations);
            }

            int listOrder = UnitOfWork.MediaFileRepository.GetNewListOrder();

            var entity = new MediaFile
            {
                TypeId = entry.TypeId,
                TopicId = entry.TopicId,
                ComposerId = entry.ComposerId,
                Artist = entry.Artist,
                AutoStart = entry.AutoStart ?? false,
                MediaLoop = entry.MediaLoop ?? false,
                Lyric = entry.Lyric,
                ListOrder = listOrder
            };

            //Proceed to save
            int mediaThumbnailStorageId;
            int mediaFileStorageId;
            if (entry.TypeId == Convert.ToInt32(MediaTypeSetting.Video))
            {
                mediaThumbnailStorageId = Convert.ToInt32(FileLocation.VideoThumbnail);
                mediaFileStorageId = Convert.ToInt32(FileLocation.Video);
            }
            else if (entry.TypeId == Convert.ToInt32(MediaTypeSetting.Audio))
            {
                mediaThumbnailStorageId = Convert.ToInt32(FileLocation.VideoThumbnail);
                mediaFileStorageId = Convert.ToInt32(FileLocation.Video);
            }
            else
            {
                mediaThumbnailStorageId = Convert.ToInt32(FileLocation.VideoThumbnail);
                mediaFileStorageId = Convert.ToInt32(FileLocation.Video);
            }

            if (entry.Media == null || entry.Media.ContentLength == 0)
            {
                var documenFileEntry = new DocumentFileEntry
                {
                    FileCode = Guid.NewGuid().ToString(),
                    FileTitle = entry.FileTitle,
                    FileName = entry.FileTitle,

                    FileDescription = entry.FileDescription,
                    FileSource = entry.FileUrl,
                    StorageType = entry.StorageType
                };

                var documenFile = DocumentService.InsertFile(applicationId, userId, documenFileEntry);
                entity.FileId = documenFile.FileId;
            }
            else
            {
                var fileInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId, entry.Media, mediaFileStorageId, StorageType.Local, entry.Width, entry.Height);
                if (fileInfo != null)
                {
                    entity.FileId = fileInfo.FileId;

                    if (entry.Photo == null)
                    {
                        var videoPhotos = UploadVideoThumbnail(applicationId, userId, fileInfo.FileId);
                        entity.SmallPhoto = videoPhotos[0];
                        entity.LargePhoto = videoPhotos[1];
                    }
                }
            }

            if (entry.Photo != null && entry.Photo.ContentLength > 0)
            {
                var photos = DocumentService.UploadAndSaveWithThumbnail(applicationId, userId, entry.Photo, 
                    mediaThumbnailStorageId, StorageType.Local);
                if (photos != null)
                {
                    entity.SmallPhoto = photos[0].FileId;
                    entity.LargePhoto = photos[1].FileId;
                }
            }

            UnitOfWork.MediaFileRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            if (entry.Albums != null && entry.Albums.Any())
            {
                CreateAlbumFiles(entity.FileId, entry.Albums);
            }

            if (entry.PlayLists != null && entry.PlayLists.Any())
            {
                CreatePlayListFiles(entity.FileId, entry.PlayLists);
            }
        }

        public void UpdateMediaFile(Guid applicationId, Guid userId, MediaFileEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullMediaFileEntry, "MediaFileEditEntry", null, ErrorMessage.Messages[ErrorCode.NullMediaFileEntry]));
                throw new ValidationError(violations);
            }

            if (entry.TypeId <= 0)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidTypeId, "TypeId", entry.TypeId, ErrorMessage.Messages[ErrorCode.InvalidTypeId]));
                throw new ValidationError(violations);
            }

            if (entry.TopicId <= 0)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidTopicId, "TopicId", entry.TopicId, ErrorMessage.Messages[ErrorCode.InvalidTopicId]));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.MediaFileRepository.FindById(entry.MediaId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidMediaId, "MediaId", entry.MediaId, ErrorMessage.Messages[ErrorCode.InvalidMediaId]));
                throw new ValidationError(violations);
            }

            int mediaThumbnailStorageId;
            int mediaFileStorageId;
            if (entry.TypeId == Convert.ToInt32(MediaTypeSetting.Video))
            {
                mediaThumbnailStorageId = Convert.ToInt32(FileLocation.VideoThumbnail);
                mediaFileStorageId = Convert.ToInt32(FileLocation.Video);
            }
            else if (entry.TypeId == Convert.ToInt32(MediaTypeSetting.Audio))
            {
                mediaThumbnailStorageId = Convert.ToInt32(FileLocation.VideoThumbnail);
                mediaFileStorageId = Convert.ToInt32(FileLocation.Video);
            }
            else
            {
                mediaThumbnailStorageId = Convert.ToInt32(FileLocation.VideoThumbnail);
                mediaFileStorageId = Convert.ToInt32(FileLocation.Video);
            }

            int currentSmallPhoto = Convert.ToInt32(entity.SmallPhoto);
            int currentLargePhoto = Convert.ToInt32(entity.LargePhoto);

            //Save video file
            int videoFileId = Convert.ToInt32(entity.FileId);
            if (entry.Media != null)
            {
                var videoFileInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId, entry.Media, mediaFileStorageId, StorageType.Local);
                if (videoFileInfo != null)
                {
                    DocumentService.DeleteFile(videoFileId);

                    videoFileId = videoFileInfo.FileId;
                    entity.FileId = videoFileId;

                    UnitOfWork.MediaFileRepository.Update(entity);
                    UnitOfWork.SaveChanges();

                    if (entry.SmallPhoto != null && entry.SmallPhoto > 0)
                    {
                        DocumentService.DeleteFile(currentSmallPhoto);
                    }

                    if (entry.LargePhoto != null && entry.LargePhoto > 0)
                    {
                        DocumentService.DeleteFile(currentLargePhoto);
                    }

                    var videoPhotos = UploadVideoThumbnail(applicationId, userId, videoFileId);
                    entity.SmallPhoto = videoPhotos[0];
                    entity.LargePhoto = videoPhotos[1];
                }
            }
            else
            {
                var mediaFileEntity = UnitOfWork.DocumentFileRepository.FindById(entity.FileId);
                if (mediaFileEntity != null)
                {
                    mediaFileEntity.FileId = mediaFileEntity.FileId;
                    mediaFileEntity.FileName = mediaFileEntity.FileName;
                    mediaFileEntity.FileTitle = entry.FileTitle;
                    mediaFileEntity.FileDescription = entry.FileDescription;
                    mediaFileEntity.FileSource = entry.FileUrl;
                    mediaFileEntity.Width = entry.Width;
                    mediaFileEntity.Height = entry.Height;
                    mediaFileEntity.StorageType = entry.StorageType;
                    mediaFileEntity.FolderId = mediaFileStorageId;

                    UnitOfWork.DocumentFileRepository.Update(mediaFileEntity);
                    UnitOfWork.SaveChanges();
                }

                if (entry.Photo != null)
                {
                    var photos = DocumentService.UploadAndSaveWithThumbnail(applicationId, userId, entry.Photo, 
                        mediaThumbnailStorageId, StorageType.Local);
                    if (photos != null)
                    {
                        entity.SmallPhoto = photos[0].FileId;
                        entity.LargePhoto = photos[1].FileId;

                        if (currentSmallPhoto > 0)
                        {
                            DocumentService.DeleteFile(currentSmallPhoto);
                        }

                        if (currentLargePhoto > 0)
                        {
                            DocumentService.DeleteFile(currentLargePhoto);
                        }
                    }
                }
            }


            entity.TypeId = entry.TypeId;
            entity.TopicId = entry.TopicId;
            entity.ComposerId = entry.ComposerId;
            entity.Artist = entry.Artist;
            entity.AutoStart = entry.AutoStart ?? false;
            entity.MediaLoop = entry.MediaLoop ?? false;
            entity.Lyric = entry.Lyric;

            UnitOfWork.MediaFileRepository.Update(entity);
            UnitOfWork.SaveChanges();

            //Save Selected Albums
            if (entry.Albums != null && entry.Albums.Any())
            {
                var selectedAlbumIds = entry.Albums;
                var existedAlbumIds = UnitOfWork.MediaAlbumFileRepository.GetListByFileId(entity.FileId).Select(x => x.AlbumId).ToList();
                var differentAlbumIds = existedAlbumIds.Except(selectedAlbumIds).ToList();
                if (differentAlbumIds.Any())
                {
                    DeleteMediaAlbumFiles(entry.FileId, differentAlbumIds);
                }

                var newAlbumIds = selectedAlbumIds.Except(existedAlbumIds).ToList();
                if (newAlbumIds.Any())
                {
                    CreateAlbumFiles(entry.FileId, newAlbumIds);
                }
            }

            //Save Selected PlayLists
            if (entry.PlayLists != null && entry.PlayLists.Any())
            {
                var selectedPlayListIds = entry.PlayLists;
                var existedPlayListIds = UnitOfWork.MediaPlayListFileRepository.GetListByFileId(entity.FileId).Select(x => x.PlayListId).ToList();
                var differentPlayListIds = existedPlayListIds.Except(selectedPlayListIds).ToList();
                if (differentPlayListIds.Any())
                {
                    DeleteMediaPlayListFiles(entry.FileId, differentPlayListIds);
                }

                var newAlbumIds = selectedPlayListIds.Except(existedPlayListIds).ToList();
                if (newAlbumIds.Any())
                {
                    CreatePlayListFiles(entry.FileId, newAlbumIds);
                }
            }
        }

        public void UpdateMediaFileStatus(int fileId, DocumentFileStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.DocumentFileRepository.FindById(fileId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidFileId, "FileId", fileId, ErrorMessage.Messages[ErrorCode.InvalidFileId]));
                throw new ValidationError(violations);
            }

            if (entity.IsActive == status) return;
            entity.IsActive = status;

            UnitOfWork.DocumentFileRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateMediaFileListOrder(int mediaId, int listOrder)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.MediaFileRepository.GetDetails(mediaId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidMediaId, "FileId", mediaId, ErrorMessage.Messages[ErrorCode.InvalidMediaId]));
                throw new ValidationError(violations);
            }

            entity.ListOrder = listOrder;

            UnitOfWork.MediaFileRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void DeleteMediaFile(int mediaId)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.MediaFileRepository.FindById(mediaId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMediaFile, "MediaId", mediaId, ErrorMessage.Messages[ErrorCode.NotFoundMediaFile]));
                throw new ValidationError(violations);
            }

            DocumentService.DeleteFile(entity.FileId);
            UnitOfWork.MediaFileRepository.Delete(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Media Topic

        public IEnumerable<TreeNode> GetMediaTopicTreeNode(MediaTopicStatus? status, int? selectedId,
            bool? isRootShowed = false)
        {
            return UnitOfWork.MediaTopicRepository.GetMediaTopicTreeNode(status, selectedId, isRootShowed);
        }

        public IEnumerable<TreeGrid> GetMediaTopicTreeGrid(MediaTopicStatus? status, int? selectedId, bool? isRootShowed)
        {
            return UnitOfWork.MediaTopicRepository.GetMediaTopicTreeGrid(status, selectedId, isRootShowed);
        }

        public IEnumerable<TreeDetail> GetMediaTopicSelectTree(int typeId, MediaTopicStatus? status, int? selectedId, bool? isRootShowed = false)
        {
            var lst = UnitOfWork.MediaTopicRepository.GetMediaTopicSelectTree(typeId, status, selectedId, isRootShowed);
            return lst.ToDtos<TreeEntity, TreeDetail>();
        }

        public MediaTopicDetail GetMediaTopicDetail(int id)
        {
            var entity = UnitOfWork.MediaTopicRepository.FindById(id);
            return entity.ToDto<MediaTopic, MediaTopicDetail>();
        }

        public void InsertMediaTopic(Guid userId, MediaTopicEntry entry)
        {
            ISpecification<MediaTopicEntry> validator = new MediaTopicEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = entry.ToEntity<MediaTopicEntry, MediaTopic>();
            entity.TopicAlias = StringUtils.ConvertTitle2Alias(entry.TopicName);
            entity.Icon = entry.Icon;
            entity.HasChild = false;
            entity.TypeId = entry.TypeId;
            entity.TopicAlias = StringUtils.ConvertTitle2Alias(entry.TopicName);
            entity.ListOrder = UnitOfWork.NewsRepository.GetNewListOrder();
            entity.CreatedDate = DateTime.UtcNow;
            entity.Ip = NetworkUtils.GetIP4Address();
            entity.CreatedByUserId = userId;

            UnitOfWork.MediaTopicRepository.Insert(entity);
            UnitOfWork.SaveChanges();

            if (entry.ParentId != null && entry.ParentId > 0)
            {
                var parentEntity = UnitOfWork.MediaTopicRepository.FindById(Convert.ToInt32(entry.ParentId));
                if (parentEntity == null) return;

                parentEntity.HasChild = true;
                UnitOfWork.MediaTopicRepository.Update(parentEntity);

                var lineage = $"{parentEntity.Lineage},{entity.TopicId}";
                entity.Lineage = lineage;
                entity.Depth = lineage.Split(',').Count();
                entity.ParentId = entry.ParentId;
            }
            else
            {
                entity.ParentId = 0;
                entity.Lineage = $"{entity.TopicId}";
                entity.Depth = 1;
            }

            UnitOfWork.MediaTopicRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateMediaTopic(Guid userId, MediaTopicEditEntry entry)
        {
            ISpecification<MediaTopicEditEntry> validator = new MediaTopicEditEntryValidator(UnitOfWork, PermissionLevel.Edit, CurrentClaimsIdentity);
            var dataViolations = new List<RuleViolation>();
            var isDataValid = validator.IsSatisfyBy(entry, dataViolations);
            if (!isDataValid) throw new ValidationError(dataViolations);

            var entity = UnitOfWork.MediaTopicRepository.FindById(entry.TopicId);
            if (entity == null) return;

            if (entity.TopicName != entry.TopicName)
            {
                bool isDuplicate = UnitOfWork.MediaTopicRepository.HasDataExisted(entry.TopicName, entry.ParentId);
                if (isDuplicate)
                {
                    dataViolations.Add(new RuleViolation(ErrorCode.DuplicateTopicName, "TopicName", entry.TopicName, ErrorMessage.Messages[ErrorCode.DuplicateTopicName]));
                    throw new ValidationError(dataViolations);
                }
            }

            if (entry.ParentId != entity.ParentId && entry.ParentId != entity.TopicId)
            {
                if (entry.ParentId != null && entry.ParentId > 0)
                {
                    var children = UnitOfWork.MediaTopicRepository.GetAllChildrenNodesOfSelectedNode(Convert.ToInt32(entry.TopicId)).ToList();
                    if (children.Any())
                    {
                        var parentIds = children.Select(x => x.TopicId).ToList();
                        if (parentIds.Contains(Convert.ToInt32(entry.ParentId)))
                        {
                            dataViolations.Add(new RuleViolation(ErrorCode.InvalidParentId, "ParentId", entry.ParentId, ErrorMessage.Messages[ErrorCode.InvalidParentId]));
                            throw new ValidationError(dataViolations);
                        }
                    }

                    //Update parent entry
                    var parentEntryEntity = UnitOfWork.MediaTopicRepository.FindById(Convert.ToInt32(entry.ParentId));
                    if (parentEntryEntity == null)
                    {
                        dataViolations.Add(new RuleViolation(ErrorCode.NotFoundParentId, "ParentId", entry.ParentId, ErrorMessage.Messages[ErrorCode.NotFoundParentId]));
                        throw new ValidationError(dataViolations);
                    }
                    else
                    {
                        if (parentEntryEntity.HasChild == null || parentEntryEntity.HasChild == false)
                        {
                            parentEntryEntity.HasChild = true;
                            UnitOfWork.MediaTopicRepository.Update(parentEntryEntity);
                        }
                    }

                    //Update parent entity
                    var parentEntity = UnitOfWork.MediaTopicRepository.FindById(entity.ParentId);
                    if (parentEntity != null)
                    {
                        var childList = UnitOfWork.MediaTopicRepository.GetAllChildrenNodesOfSelectedNode(Convert.ToInt32(entity.ParentId)).ToList();
                        if (childList.Any())
                        {
                            childList = childList.Where(x => (x.TopicId != entity.ParentId) && (x.TopicId != entity.TopicId)).ToList();
                            parentEntity.HasChild = childList.Any();
                            UnitOfWork.MediaTopicRepository.Update(parentEntity);
                        }
                    }

                    var lineage = $"{parentEntryEntity.Lineage},{entry.TopicId}";
                    entity.Lineage = lineage;
                    entity.Depth = lineage.Split(',').Count();
                    entity.ParentId = entry.ParentId;
                }
                else
                {
                    entity.ParentId = 0;
                    entity.Lineage = $"{entry.TopicId}";
                    entity.Depth = 1;
                }
            }

            //Update entity

            var hasChild = UnitOfWork.MediaTopicRepository.HasChild(entity.TopicId);
            entity.HasChild = hasChild;
            entity.TypeId = entry.TypeId;
            entity.TopicName = entry.TopicName;
            entity.TopicAlias = StringUtils.GenerateFriendlyString(entry.TopicName);
            entity.Icon = entry.Icon;
            entity.Description = entry.Description;
            entity.Status = entry.Status;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.MediaTopicRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateMediaTopicStatus(Guid userId, int id, MediaTopicStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.MediaTopicRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMediaTopic, "TopicId", id, ErrorMessage.Messages[ErrorCode.NotFoundMediaTopic]));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(MediaTopicStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", status, ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }

            if (entity.Status == status) return;

            entity.Status = status;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.MediaTopicRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateMediaTopicListOrder(Guid userId, int id, int listOrder)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.MediaTopicRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundProductCategory, "MediaTopicId", id, ErrorMessage.Messages[ErrorCode.NotFoundProductCategory]));
                throw new ValidationError(violations);
            }

            if (entity.ListOrder == listOrder) return;

            entity.ListOrder = listOrder;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.MediaTopicRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Media PlayList
        public IEnumerable<MediaPlayListInfoDetail> GetPlayLists(MediaPlayListSearchEntry filter, ref int? recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = new List<MediaPlayListInfoDetail>();
            var playLists = UnitOfWork.MediaPlayListRepository.GetMediaPlayLists(filter.SearchText, filter.SearchTypeId, filter.SearchTopicId, filter.SearchStatus, ref recordCount, orderBy, page, pageSize);
            if (playLists != null && playLists.Any())
            {
                foreach (var item in playLists)
                {
                    var playList = new MediaPlayListInfoDetail
                    {
                        TypeId = item.TypeId,
                        TopicId = item.TopicId,
                        PlayListId = item.PlayListId,
                        PlayListName = item.PlayListName,
                        PlayListAlias = item.PlayListAlias,
                        FrontImage = item.FrontImage,
                        MainImage = item.MainImage,
                        Description = item.Description,
                        TotalViews = item.TotalViews,
                        ListOrder = item.ListOrder,
                        Status = item.Status,
                        FrontImageUrl = (item.FrontImage != null && item.FrontImage > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(item.FrontImage)).FileUrl : GlobalSettings.NotFoundFileUrl,
                        MainImageUrl = (item.MainImage != null && item.FrontImage > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(item.MainImage)).FileUrl : GlobalSettings.NotFoundFileUrl,
                        Type = item.Type.ToDto<MediaType, MediaTypeDetail>(),
                        Topic = item.Topic.ToDto<MediaTopic, MediaTopicDetail>()
                    };

                    if (item.FrontImage != null)
                    {
                        playList.FrontImageInfo = DocumentService.GetFileInfoDetail(Convert.ToInt32(item.FrontImage));
                    }

                    if (item.MainImage != null)
                    {
                        playList.MainImageInfo = DocumentService.GetFileInfoDetail(Convert.ToInt32(item.MainImage));
                    }
                    lst.Add(playList);
                }
            }
            return lst;
        }
        public IEnumerable<MediaPlayListInfoDetail> GetPlayLists(int? typeId, int? topicId, MediaPlayListStatus? status)
        {
            var lst = new List<MediaPlayListInfoDetail>();
            var playLists = UnitOfWork.MediaPlayListRepository.GetMediaPlayLists(typeId, topicId, status).ToList();
            if (playLists.Any())
            {
                foreach (var item in playLists)
                {
                    var playList = new MediaPlayListInfoDetail
                    {
                        TypeId = item.TypeId,
                        TopicId = item.TopicId,
                        PlayListId = item.PlayListId,
                        PlayListName = item.PlayListName,
                        PlayListAlias = item.PlayListAlias,
                        FrontImage = item.FrontImage,
                        MainImage = item.MainImage,
                        Description = item.Description,
                        TotalViews = item.TotalViews,
                        ListOrder = item.ListOrder,
                        Status = item.Status,
                        FrontImageUrl = (item.FrontImage != null && item.FrontImage > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(item.FrontImage)).FileUrl : GlobalSettings.NotFoundFileUrl,
                        MainImageUrl = (item.MainImage != null && item.FrontImage > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(item.MainImage)).FileUrl : GlobalSettings.NotFoundFileUrl,
                        Type = item.Type.ToDto<MediaType, MediaTypeDetail>(),
                        Topic = item.Topic.ToDto<MediaTopic, MediaTopicDetail>()
                    };

                    if (item.FrontImage != null)
                    {
                        playList.FrontImageInfo = DocumentService.GetFileInfoDetail(Convert.ToInt32(item.FrontImage));
                    }

                    if (item.MainImage != null)
                    {
                        playList.MainImageInfo = DocumentService.GetFileInfoDetail(Convert.ToInt32(item.MainImage));
                    }
                    lst.Add(playList);
                }
            }
            return lst;
        }

        public MultiSelectList PoplulateMediaPlayListMultiSelectList(int typeId, int topicId, MediaPlayListStatus? status = null, bool? isShowSelectText = null, int[] selectedValues = null)
        {
            return UnitOfWork.MediaPlayListRepository.PoplulateMediaPlayListMultiSelectList(typeId, topicId, status, isShowSelectText,
           selectedValues);
        }

        public MediaPlayListInfoDetail GetPlayListDetail(int id)
        {
            var item = UnitOfWork.MediaPlayListRepository.GeDetails(id);
            if (item == null) return null;

            var playList = new MediaPlayListInfoDetail
            {
                TypeId = item.TypeId,
                TopicId = item.TopicId,
                PlayListId = item.PlayListId,
                PlayListName = item.PlayListName,
                PlayListAlias = item.PlayListAlias,
                FrontImage = item.FrontImage,
                MainImage = item.MainImage,
                Description = item.Description,
                TotalViews = item.TotalViews,
                ListOrder = item.ListOrder,
                Status = item.Status,
                FrontImageUrl = (item.FrontImage != null && item.FrontImage > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(item.FrontImage)).FileUrl : GlobalSettings.NotFoundFileUrl,
                MainImageUrl = (item.MainImage != null && item.FrontImage > 0) ? DocumentService.GetFileInfoDetail(Convert.ToInt32(item.MainImage)).FileUrl : GlobalSettings.NotFoundFileUrl,
                Type = item.Type.ToDto<MediaType, MediaTypeDetail>(),
                Topic = item.Topic.ToDto<MediaTopic, MediaTopicDetail>()
            };

            if (item.FrontImage != null)
            {
                playList.FrontImageInfo = DocumentService.GetFileInfoDetail(Convert.ToInt32(item.FrontImage));
            }

            if (item.MainImage != null)
            {
                playList.MainImageInfo = DocumentService.GetFileInfoDetail(Convert.ToInt32(item.MainImage));
            }
            return playList;
        }
        public SelectList PoplulatePlayListSelectList(int typeId, int topicId, int? selectedValue, bool? isShowSelectText = null, MediaPlayListStatus? status = null)
        {
            var listItems = new List<SelectListItem>();
            var lst = UnitOfWork.MediaPlayListRepository.GetMediaPlayLists(typeId, topicId, MediaPlayListStatus.Active).ToList();
            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem { Text = p.PlayListName, Value = p.PlayListId.ToString() }).ToList();
                if (isShowSelectText != null && isShowSelectText == true)
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.Select} ---", Value = "" });
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }
        public MediaPlayListDetail InsertPlayList(Guid applicationId, Guid userId, MediaPlayListEntry entry)
        {
            ISpecification<MediaPlayListEntry> validator = new MediaPlayListEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var violations = new List<RuleViolation>();
            if (!validator.IsSatisfyBy(entry, violations)) throw new ValidationError(violations);

            var entity = entry.ToEntity<MediaPlayListEntry, MediaPlayList>();
            entity.PlayListAlias = StringUtils.ConvertTitle2Alias(entry.PlayListName);
            entity.ListOrder = UnitOfWork.MediaPlayListRepository.GetNewListOrder();
            entity.CreatedDate = DateTime.UtcNow;
            entity.Ip = NetworkUtils.GetIP4Address();
            entity.CreatedByUserId = userId;

            if (entry.FileUpload != null && entry.FileUpload.ContentLength > 0)
            {
                int maxContentLength = UnitOfWork.ApplicationSettingRepository.GetAllowedMaxImageContentLength(applicationId);
                string[] allowedFileExtensions = UnitOfWork.ApplicationSettingRepository.GetAllowedImageExtensions(applicationId);

                if (!allowedFileExtensions.Contains(entry.FileUpload.FileName.Substring(entry.FileUpload.FileName.LastIndexOf('.'))))
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidFileType, "FileUpload", LanguageResource.InvalidFileExtension + " : " + string.Join(", ", allowedFileExtensions)));
                    throw new ValidationError(violations);
                }
                else if (entry.FileUpload.ContentLength > maxContentLength)
                {
                    violations.Add(new RuleViolation(ErrorCode.MaximumAllowedSize, "FileUpload", LanguageResource.InvalidFileSize + " , " + LanguageResource.MaximumAllowedSize + maxContentLength + " MB"));
                    throw new ValidationError(violations);
                }
                else
                {
                    var fileIds = DocumentService.UploadAndSaveWithThumbnail(applicationId, userId, entry.FileUpload, (int)FileLocation.Album, StorageType.Local);
                    entity.MainImage = fileIds[0].FileId;
                    entity.FrontImage = fileIds[1].FileId;
                }
            }

            UnitOfWork.MediaPlayListRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<MediaPlayList, MediaPlayListDetail>();
        }
        public void UpdatePlayList(Guid applicationId, Guid userId, MediaPlayListEditEntry entry)
        {
            //Check validation
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMediaPlayListEditEntry, "MediaPlayListEditEntry", null, ErrorMessage.Messages[ErrorCode.NotFoundMediaPlayListEditEntry]));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.MediaPlayListRepository.Find(entry.PlayListId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMediaPlayListEditEntry, "MediaPlayListEditEntry", entry.PlayListId, ErrorMessage.Messages[ErrorCode.NotFoundMediaPlayListEditEntry]));
                throw new ValidationError(violations);
            }

            if (!string.IsNullOrEmpty(entry.PlayListName) && entry.PlayListName.Length > 250)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidName, "PlayListName", entry.PlayListName.Length, ErrorMessage.Messages[ErrorCode.InvalidName]));
                throw new ValidationError(violations);
            }

            if (!string.IsNullOrEmpty(entry.Description) && entry.Description.Length > 500)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidDescription, "Description", entry.Description.Length, ErrorMessage.Messages[ErrorCode.InvalidDescription]));
                throw new ValidationError(violations);
            }

            if (!Enum.IsDefined(typeof(MediaPlayListStatus), entry.Status))
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", entry.Status, ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }

            if (entry.FileUpload != null && entry.FileUpload.ContentLength > 0)
            {
                int maxContentLength = UnitOfWork.ApplicationSettingRepository.GetAllowedMaxImageContentLength(applicationId);
                string[] allowedFileExtensions = UnitOfWork.ApplicationSettingRepository.GetAllowedImageExtensions(applicationId);

                if (!allowedFileExtensions.Contains(entry.FileUpload.FileName.Substring(entry.FileUpload.FileName.LastIndexOf('.'))))
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidFileType, "FileUpload", LanguageResource.InvalidFileExtension + " : " + string.Join(", ", allowedFileExtensions)));
                    throw new ValidationError(violations);
                }
                else if (entry.FileUpload.ContentLength > maxContentLength)
                {
                    violations.Add(new RuleViolation(ErrorCode.MaximumAllowedSize, "FileUpload", LanguageResource.InvalidFileSize + " , " + LanguageResource.MaximumAllowedSize + maxContentLength + " MB"));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entity.FrontImage != null)
                    {
                        DocumentService.DeleteFile(Convert.ToInt32(entity.FrontImage));
                    }

                    if (entity.MainImage != null)
                    {
                        DocumentService.DeleteFile(Convert.ToInt32(entity.MainImage));
                    }

                    var fileIds = DocumentService.UploadAndSaveWithThumbnail(applicationId, userId, entry.FileUpload, (int)FileLocation.Album, StorageType.Local);
                    entity.MainImage = fileIds[0].FileId;
                    entity.FrontImage = fileIds[1].FileId;
                }
            }

            //Assign data
            entity.TypeId = entry.TypeId;
            entity.TopicId = entry.TopicId;
            entity.PlayListName = entry.PlayListName;
            entity.PlayListAlias = StringUtils.ConvertTitle2Alias(entry.PlayListName);
            entity.Description = entry.Description;
            entity.Status = entry.Status;
            entity.LastModifiedByUserId = userId;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();

            UnitOfWork.MediaPlayListRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdatePlayListStatus(Guid userId, int id, MediaPlayListStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.MediaPlayListRepository.Find(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMediaPlayList, "MediaPlayListId", id, ErrorMessage.Messages[ErrorCode.NotFoundMediaPlayList]));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(MediaPlayListStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", status, ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.Status == status) return;

            entity.Status = status;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;

            UnitOfWork.MediaPlayListRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdatePlayListListOrder(Guid userId, int id, int listOrder)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.MediaPlayListRepository.Find(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMediaPlayList, "MediaPlayList", id, ErrorMessage.Messages[ErrorCode.NotFoundMediaPlayList]));
                throw new ValidationError(violations);
            }

            entity.ListOrder = listOrder;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;

            UnitOfWork.MediaPlayListRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void DeletePlayList(int id)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.MediaPlayListRepository.Find(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMediaPlayList, "MediaPlayListId", id, ErrorMessage.Messages[ErrorCode.NotFoundMediaPlayList]));
                throw new ValidationError(violations);
            }

            UnitOfWork.MediaPlayListRepository.Delete(entity);
            UnitOfWork.SaveChanges();
        }

        #endregion

        #region Media Type
        public IEnumerable<MediaTypeDetail> GetMediaTypes(MediaTypeStatus? status, ref int? recordCount, int? page, int? pageSize)
        {
            var entityList = UnitOfWork.MediaTypeRepository.GetMediaTypes(status, ref recordCount, page, pageSize);
            return entityList.ToDtos<MediaType, MediaTypeDetail>();
        }
        public IEnumerable<MediaTypeDetail> GetMediaTypes(MediaTypeStatus? status)
        {
            var entityList = UnitOfWork.MediaTypeRepository.GetList(status);
            return entityList.ToDtos<MediaType, MediaTypeDetail>();
        }
        public MediaTypeDetail GetMediaTypeDetail(int id)
        {
            var entity = UnitOfWork.MediaTypeRepository.Find(id);
            return entity.ToDto<MediaType, MediaTypeDetail>();
        }
        public SelectList PoplulateMediaTypeSelectList(int? selectedValue, bool? isShowSelectText = false, MediaTypeStatus? status = null)
        {
            var listItems = new List<SelectListItem>();
            var lst = UnitOfWork.MediaTypeRepository.GetList(MediaTypeStatus.Active).ToList();
            if (lst.Any())
            {
                listItems = lst.Select(p => new SelectListItem { Text = p.TypeName, Value = p.TypeId.ToString() }).ToList();
                if (isShowSelectText != null && isShowSelectText == true)
                    listItems.Insert(0, new SelectListItem { Text = $"--- {LanguageResource.Select} ---", Value = "" });
            }
            else
            {
                listItems.Insert(0, new SelectListItem { Text = $"-- {LanguageResource.None} --", Value = "-1" });
            }
            return new SelectList(listItems, "Value", "Text", selectedValue);
        }
        public MediaTypeDetail InsertMediaType(Guid userId, MediaTypeEntry entry)
        {
            ISpecification<MediaTypeEntry> validator = new MediaTypeEntryValidator(UnitOfWork, PermissionLevel.Create, CurrentClaimsIdentity);
            var violations = new List<RuleViolation>();
            if (!validator.IsSatisfyBy(entry, violations)) throw new ValidationError(violations);

            var entity = entry.ToEntity<MediaTypeEntry, MediaType>();
            entity.ListOrder = UnitOfWork.MediaTypeRepository.GetNewListOrder();
            entity.CreatedDate = DateTime.UtcNow;
            entity.Ip = NetworkUtils.GetIP4Address();
            entity.CreatedByUserId = userId;

            UnitOfWork.MediaTypeRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<MediaType, MediaTypeDetail>();
        }
        public void UpdateMediaType(Guid userId, MediaTypeEditEntry entry)
        {
            //Check validation
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMediaTypeEditEntry, "MediaTypeEditEntry", null, ErrorMessage.Messages[ErrorCode.NotFoundMediaTypeEditEntry]));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.MediaTypeRepository.Find(entry.TypeId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMediaTypeEditEntry, "MediaTypeEditEntry", entry.TypeId, ErrorMessage.Messages[ErrorCode.NotFoundMediaTypeEditEntry]));
                throw new ValidationError(violations);
            }

            if (!string.IsNullOrEmpty(entry.TypeName) && entry.TypeName.Length > 250)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidName, "TypeName", entry.TypeName.Length, ErrorMessage.Messages[ErrorCode.InvalidName]));
                throw new ValidationError(violations);
            }

            if (!string.IsNullOrEmpty(entry.Description) && entry.Description.Length > 500)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidDescription, "Description", entry.Description.Length, ErrorMessage.Messages[ErrorCode.InvalidDescription]));
                throw new ValidationError(violations);
            }

            if (!Enum.IsDefined(typeof(MediaTypeStatus), entry.Status))
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", entry.Status, ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }

            //Assign data
            entity.TypeName = entry.TypeName;
            entity.Description = entry.Description;
            entity.Status = entry.Status;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;

            UnitOfWork.MediaTypeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateMediaTypeStatus(Guid userId, int id, MediaTypeStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.MediaTypeRepository.Find(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMediaType, "MediaTypeId", id, ErrorMessage.Messages[ErrorCode.NotFoundMediaType]));
                throw new ValidationError(violations);
            }

            var isValid = Enum.IsDefined(typeof(MediaTypeStatus), status);
            if (!isValid)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidStatus, "Status", status, ErrorMessage.Messages[ErrorCode.InvalidStatus]));
                throw new ValidationError(violations);
            }
            if (entity.Status == status) return;

            entity.Status = status;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;

            UnitOfWork.MediaTypeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateMediaTypeListOrder(Guid userId, int id, int listOrder)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.MediaTypeRepository.Find(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMediaType, "MediaType", id, ErrorMessage.Messages[ErrorCode.NotFoundMediaType]));
                throw new ValidationError(violations);
            }

            entity.ListOrder = listOrder;
            entity.LastModifiedDate = DateTime.UtcNow;
            entity.LastUpdatedIp = NetworkUtils.GetIP4Address();
            entity.LastModifiedByUserId = userId;

            UnitOfWork.MediaTypeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void DeleteMediaType(int id)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.MediaTypeRepository.Find(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundMediaType, "MediaType", id, ErrorMessage.Messages[ErrorCode.NotFoundMediaType]));
                throw new ValidationError(violations);
            }

            UnitOfWork.MediaTypeRepository.Delete(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Dipose

        private bool _disposed = false;
        protected override void Dispose(bool isDisposing)
        {
            if (!this._disposed)
            {
                if (isDisposing)
                {
                    DocumentService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}