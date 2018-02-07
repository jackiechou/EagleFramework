using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Common.Utilities;
using Eagle.Core.Configuration;
using Eagle.Core.Settings;
using Eagle.Entities.Skins;
using Eagle.Repositories;
using Eagle.Repositories.Themes;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.Skins;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.SystemManagement;
using Eagle.Services.Validations;

namespace Eagle.Services.Skins
{
    public class ThemeService : BaseService, IThemeService
    {
        private ICacheService CacheService { get; set; }
        private IDocumentService DocumentService { get; set; }

        public ThemeService(IUnitOfWork unitOfWork, ICacheService cacheService, IDocumentService documentService) : base(unitOfWork)
        {
            CacheService = cacheService;
            DocumentService = documentService;
        }

        #region Skin Package
        public ThemeDetail GetSelectedTheme(Guid applicationId)
        {
            var theme = UnitOfWork.SkinPackageRepository.GetSelectedTheme(applicationId);
            if(theme==null)
            {
               return new ThemeDetail
               {
                   PackageName = ThemeSettings.ThemeName,
                   PackageSrc = ThemeSettings.ThemeSrc
               };
            }

            CacheService.Add(CacheKeySetting.ThemeName, theme.PackageName);
            CacheService.Add(CacheKeySetting.ThemeSrc, theme.PackageSrc);
            return theme.ToDto<Theme, ThemeDetail>();
        }
        public IEnumerable<SkinPackageInfoDetail> GetSkinPackages(Guid applicationId, SkinPackageSearchEntry entry,
            out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = new List<SkinPackageInfoDetail>();
            var packages = UnitOfWork.SkinPackageRepository.GetSkinPackages(applicationId, entry.SearchTypeId, entry.SearchStatus, out recordCount, orderBy, page, pageSize);
            lst.AddRange(packages.Select(item => new SkinPackageInfoDetail
            {
                ApplicationId = item.ApplicationId,
                TypeId = item.TypeId,
                PackageId = item.PackageId,
                PackageName = item.PackageName,
                PackageAlias = item.PackageAlias,
                PackageSrc = item.PackageSrc,
                IsSelected = item.IsSelected,
                IsActive = item.IsActive,
                Type = item.Type.ToDto<SkinPackageType, SkinPackageTypeDetail>()
            }));
            return lst;
        }
        public IEnumerable<SkinPackageDetail> GetSkinPackages(Guid applicationId, SkinPackageSearchEntry entry)
        {
            var lst = UnitOfWork.SkinPackageRepository.GetSkinPackages(applicationId, entry.SearchTypeId, entry.SearchStatus);
            return lst.ToDtos<SkinPackage, SkinPackageDetail>();
        }
        public SkinPackageInfoDetail GetSkinPackageDetail(int packageId)
        {
            var item = UnitOfWork.SkinPackageRepository.GetDetail(packageId);
            return new SkinPackageInfoDetail
            {
                ApplicationId = item.ApplicationId,
                TypeId = item.TypeId,
                PackageId = item.PackageId,
                PackageName = item.PackageName,
                PackageAlias = item.PackageAlias,
                PackageSrc = item.PackageSrc,
                IsSelected = item.IsSelected,
                IsActive = item.IsActive,
                Type = item.Type.ToDto<SkinPackageType, SkinPackageTypeDetail>()
            };
        }
        public SelectList PopulateSkinPackageSelectList(Guid applicationId, int? typeId, bool? status = null, int? selectedValue = null, bool? isShowSelectText = true)
        {
            return UnitOfWork.SkinPackageRepository.PopulateSkinPackageSelectList(applicationId, typeId, status, selectedValue, isShowSelectText);
        }
        public SelectList PopulateSkinPackageStatus(bool? selectedValue = null, bool? isShowSelectText = true)
        {
            return UnitOfWork.SkinPackageRepository.PopulateSkinPackageStatus(selectedValue, isShowSelectText);
        }
        public bool HasDataExisted(Guid applicationId, string skinPackageName)
        {
            return UnitOfWork.SkinPackageRepository.HasDataExisted(applicationId, skinPackageName);
        }
        public void InsertSkinPackage(Guid applicationId, SkinPackageEntry entry)
        {
            var violations = new List<RuleViolation>();
            var skinPackageType = UnitOfWork.SkinPackageTypeRepository.FindById(entry.TypeId);
            if (skinPackageType == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidTypeId, "TypeId", entry.TypeId,
                    ErrorMessage.Messages[ErrorCode.InvalidTypeId]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.PackageName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullPackageName, "PackageName", entry.PackageName,
                    ErrorMessage.Messages[ErrorCode.NullPackageName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.PackageName.Length > 150)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidPackageName, "PackageName", entry.PackageName,
                   ErrorMessage.Messages[ErrorCode.InvalidPackageName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    var isExisted = UnitOfWork.SkinPackageRepository.HasDataExisted(applicationId, entry.PackageName);
                    if (isExisted)
                    {
                        violations.Add(new RuleViolation(ErrorCode.DuplicatePackageName, "PackageName", entry.PackageName,
                   ErrorMessage.Messages[ErrorCode.DuplicatePackageName]));
                        throw new ValidationError(violations);
                    }
                }
            }
            
            var entity = entry.ToEntity<SkinPackageEntry, SkinPackage>();
            entity.ApplicationId = applicationId;
            entity.PackageAlias = StringUtils.ConvertTitle2Alias(entry.PackageName);
            entity.IsSelected = false;

            UnitOfWork.SkinPackageRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateSkinPackage(Guid applicationId, SkinPackageEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.SkinPackageRepository.FindById(entry.PackageId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidPackageId, "PackageId", entry.PackageId,
                  ErrorMessage.Messages[ErrorCode.InvalidPackageId]));
                throw new ValidationError(violations);
            }

            var skinPackageType = UnitOfWork.SkinPackageTypeRepository.FindById(entry.TypeId);
            if (skinPackageType == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidTypeId, "TypeId", entry.TypeId,
                    ErrorMessage.Messages[ErrorCode.InvalidTypeId]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.PackageName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullPackageName, "PackageName", entry.PackageName,
                    ErrorMessage.Messages[ErrorCode.NullPackageName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.PackageName.Length > 150)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidPackageName, "PackageName", entry.PackageName,
                   ErrorMessage.Messages[ErrorCode.InvalidPackageName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entity.PackageName != entry.PackageName)
                    {
                        var isExisted = UnitOfWork.SkinPackageRepository.HasDataExisted(applicationId, entry.PackageName);
                        if (isExisted)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicatePackageName, "PackageName", entry.PackageName,
                       ErrorMessage.Messages[ErrorCode.DuplicatePackageName]));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }

            entity.TypeId = entry.TypeId;
            entity.PackageName = entry.PackageName;
            entity.PackageAlias = StringUtils.ConvertTitle2Alias(entry.PackageName);
            entity.PackageSrc = entry.PackageSrc;
            entity.IsActive = entry.IsActive;

            UnitOfWork.SkinPackageRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateSkinPackageStatus(int packageId, bool status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.SkinPackageRepository.FindById(packageId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundPackageId, "PackageId", packageId,
                 ErrorMessage.Messages[ErrorCode.NotFoundPackageId]));
                throw new ValidationError(violations);
            }
            if (entity.IsActive == status) return;

            entity.IsActive = status;

            UnitOfWork.SkinPackageRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateSelectedSkin(Guid applicationId, int id)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.SkinPackageRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundSkinPackage, "PackageId", id,
                    ErrorMessage.Messages[ErrorCode.NotFoundSkinPackage]));
                throw new ValidationError(violations);
            }

            if (entity.IsSelected) return;

            var lst = UnitOfWork.SkinPackageRepository.GetSkinPackages(applicationId, entity.TypeId).ToList();
            if (!lst.Any()) return;

            foreach (var item in lst)
            {
                item.IsSelected = (item.PackageId == id);
                UnitOfWork.SkinPackageRepository.Update(item);
                UnitOfWork.SaveChanges();
            }
        }
        #endregion

        #region Skin Package Background
        public IEnumerable<SkinPackageBackgroundDetail> GetSkinPackageBackgrounds(SkinPackageBackgroundSearchEntry entry, out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.SkinPackageBackgroundRepository.GetSkinPackageBackgrounds(entry.SearchPackageId, entry.SearchStatus, out recordCount, orderBy, page, pageSize);
            return lst.ToDtos<SkinPackageBackground, SkinPackageBackgroundDetail>();
        }
        public SkinPackageBackgroundInfoDetail GetSkinPackageBackgroundDetail(int backgroundId)
        {
            var item = UnitOfWork.SkinPackageBackgroundRepository.GetDetail(backgroundId);
            return new SkinPackageBackgroundInfoDetail
            {
                TypeId = item.TypeId,
                PackageId = item.PackageId,
                BackgroundId = item.BackgroundId,
                BackgroundFile = item.BackgroundFile,
                BackgroundLink = item.BackgroundLink,
                IsExternalLink = item.IsExternalLink,
                IsActive = item.IsActive,
                Package = item.Package.ToDto<SkinPackage, SkinPackageDetail>()
            };
        }
        public SelectList PopulateSkinPackageBackgroundStatus(bool? selectedValue = null, bool? isShowSelectText = true)
        {
            return UnitOfWork.SkinPackageBackgroundRepository.PopulateSkinPackageBackgroundStatus(selectedValue, isShowSelectText);
        }
        public void InsertSkinPackageBackground(Guid applicationId, Guid userId, SkinPackageBackgroundEntry entry)
        {
            var violations = new List<RuleViolation>();
            var entity = entry.ToEntity<SkinPackageBackgroundEntry, SkinPackageBackground>();

            var packageType = UnitOfWork.SkinPackageTypeRepository.FindById(entry.TypeId);
            if (packageType == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidTypeId, "TypeId", entry.TypeId,
                    ErrorMessage.Messages[ErrorCode.InvalidTypeId]));
                throw new ValidationError(violations);
            }

            var package = UnitOfWork.SkinPackageRepository.FindById(entry.PackageId);
            if (package == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidPackageId, "PackageId", entry.PackageId,
                  ErrorMessage.Messages[ErrorCode.InvalidPackageId]));
                throw new ValidationError(violations);
            }

            if (entry.File == null && string.IsNullOrEmpty(entry.BackgroundLink))
            {
                violations.Add(new RuleViolation(ErrorCode.PleaseBrowseFileOrInputLink, "File", entry.File,
                 ErrorMessage.Messages[ErrorCode.PleaseBrowseFileOrInputLink]));
                throw new ValidationError(violations);
            }

            if (entry.File != null && entry.File.ContentLength > 0)
            {
                var fileInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId, entry.File, (int) FileLocation.BackgroundPhoto,
                    StorageType.Local);
                entity.BackgroundFile = fileInfo.FileId;
                entity.BackgroundName = fileInfo.FileName;
            }

            entity.ListOrder = UnitOfWork.SkinPackageBackgroundRepository.GetNewListOrder();

            UnitOfWork.SkinPackageBackgroundRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateSkinPackageBackground(Guid applicationId, Guid userId, SkinPackageBackgroundEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.SkinPackageBackgroundRepository.FindById(entry.BackgroundId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundBackgroundId, "BackgroundId", entry.BackgroundId,
                    ErrorMessage.Messages[ErrorCode.NotFoundBackgroundId]));
                throw new ValidationError(violations);
            }

            var packageType = UnitOfWork.SkinPackageTypeRepository.FindById(entry.TypeId);
            if (packageType == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidTypeId, "TypeId", entry.TypeId,
                    ErrorMessage.Messages[ErrorCode.InvalidTypeId]));
                throw new ValidationError(violations);
            }

            var package = UnitOfWork.SkinPackageRepository.FindById(entry.PackageId);
            if (package == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidPackageId, "PackageId", entry.PackageId,
                  ErrorMessage.Messages[ErrorCode.InvalidPackageId]));
                throw new ValidationError(violations);
            }

            if (entry.File == null && string.IsNullOrEmpty(entry.BackgroundLink) && entry.BackgroundFile == null)
            {
                violations.Add(new RuleViolation(ErrorCode.PleaseBrowseFileOrInputLink, "File", entry.File,
                 ErrorMessage.Messages[ErrorCode.PleaseBrowseFileOrInputLink]));
                throw new ValidationError(violations);
            }
       
            if (entry.File != null && entry.File.ContentLength > 0)
            {
                if (entity.BackgroundFile != null)
                {
                    DocumentService.DeleteFile(Convert.ToInt32(entity.BackgroundFile));
                }

                var fileInfo = DocumentService.UploadAndSaveDbByFolderId(applicationId, userId, entry.File, (int)FileLocation.BackgroundPhoto, StorageType.Local);
                entity.BackgroundFile = fileInfo.FileId;
                entity.BackgroundName = fileInfo.FileName;
            }

            entity.TypeId = entry.TypeId;
            entity.PackageId = entry.PackageId;
            entity.BackgroundLink = entry.BackgroundLink;
            entity.IsExternalLink = entry.IsExternalLink;
            entity.IsActive = entry.IsActive;

            UnitOfWork.SkinPackageBackgroundRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateSkinPackageBackgroundStatus(int skinPackageBackgroundId, bool status)
        {
            var entity = UnitOfWork.SkinPackageBackgroundRepository.FindById(skinPackageBackgroundId);
            if (entity == null) return;

            entity.IsActive = status;
            UnitOfWork.SkinPackageBackgroundRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateSkinPackageBackgroundSortKey(int backgroundId, int listOrder)
        {
            var entity = UnitOfWork.SkinPackageBackgroundRepository.FindById(backgroundId);
            if (entity == null) return;

            entity.ListOrder = listOrder;
            UnitOfWork.SkinPackageBackgroundRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateSortKeyUpDown(int signal, int currentIdx)
        {
            var currentItem = UnitOfWork.SkinPackageBackgroundRepository.GetCurrentBackground(currentIdx);
            if (currentItem == null) return;

            if (signal == 1)
            {
                //Previous
                var prevItem = UnitOfWork.SkinPackageBackgroundRepository.GetPreviousBackground(currentIdx);
                if (prevItem == null) return;
                UpdateSkinPackageBackgroundSortKey(currentIdx, Convert.ToInt32(prevItem.ListOrder));
                UpdateSkinPackageBackgroundSortKey(prevItem.BackgroundId, Convert.ToInt32(currentItem.ListOrder));
            }
            else
            {
                //Next
                var nextItem = UnitOfWork.SkinPackageBackgroundRepository.GetNextBackground(currentIdx);
                if (nextItem == null) return;
                UpdateSkinPackageBackgroundSortKey(currentIdx, Convert.ToInt32(nextItem.ListOrder));
                UpdateSkinPackageBackgroundSortKey(nextItem.BackgroundId, Convert.ToInt32(currentItem.ListOrder));
            }
            UnitOfWork.SaveChanges();
        }

        public void DeleteSkinPackageBackground(int skinPackageBackgroundId, string dirPath)
        {
            var entity = UnitOfWork.SkinPackageBackgroundRepository.FindById(skinPackageBackgroundId);
            if (entity == null) return;

            FileUtils.DeleteFileWithPredefinedDatePath(entity.BackgroundLink, dirPath);
            UnitOfWork.SkinPackageBackgroundRepository.Delete(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Skin Package Template
        public IEnumerable<SkinPackageTemplateInfoDetail> GetSkinPackageTemplates(SkinPackageTemplateSearchEntry entry,
          out int recordCount, string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = new List<SkinPackageTemplateInfoDetail>();
            var templates = UnitOfWork.SkinPackageTemplateRepository.GetSkinPackageTemplates(entry.SearchPackageId, entry.SearchStatus, out recordCount, orderBy, page,pageSize);
            lst.AddRange(templates.Select(item => new SkinPackageTemplateInfoDetail
            {
                TypeId = item.TypeId,
                PackageId = item.PackageId,
                TemplateId = item.TemplateId,
                TemplateName = item.TemplateName,
                TemplateKey = item.TemplateKey,
                TemplateSrc = item.TemplateSrc,
                IsActive = item.IsActive,
                Package = item.Package.ToDto<SkinPackage, SkinPackageDetail>(),
                Type = item.Type.ToDto<SkinPackageType, SkinPackageTypeDetail>()
            }));
            return lst;
        }

        public IEnumerable<SkinPackageTemplateDetail> GetSkinPackageTemplatesBySelectedSkin()
        {
            var lst = UnitOfWork.SkinPackageTemplateRepository.GetListBySelectedSkin();
            return lst.ToDtos<SkinPackageTemplate, SkinPackageTemplateDetail>();
        }

        public SelectList PopulateTemplateSelectList(int packageId, bool? status = null, string selectedValue = null, bool? isShowSelectText = true)
        {
            return UnitOfWork.SkinPackageTemplateRepository.PopulateTemplateSelectList(packageId, status, selectedValue, isShowSelectText);
        }

        public string GetTemplateSrcByPageId(int pageId)
        {
            return UnitOfWork.SkinPackageTemplateRepository.GetTemplateSrcByPageId(pageId);
        }
        public SelectList PopulateTemplateSelectListBySelectedSkin(string selectedValue = null, bool isShowSelectText = true)
        {
            return UnitOfWork.SkinPackageTemplateRepository.PopulateTemplateSelectListBySelectedSkin(selectedValue, isShowSelectText);
        }
        public SkinPackageTemplateInfoDetail GetSkinPackageTemplateDetail(int templateId)
        {
            var item = UnitOfWork.SkinPackageTemplateRepository.GetDetail(templateId);
            return new SkinPackageTemplateInfoDetail
            {
                TypeId = item.TypeId,
                PackageId = item.PackageId,
                TemplateId = item.TemplateId,
                TemplateName = item.TemplateName,
                TemplateKey = item.TemplateKey,
                TemplateSrc = item.TemplateSrc,
                IsActive = item.IsActive,
                Package = item.Package.ToDto<SkinPackage, SkinPackageDetail>(),
                Type = item.Type.ToDto<SkinPackageType, SkinPackageTypeDetail>()
            };
        }
        public void InsertSkinPackageTemplate(SkinPackageTemplateEntry entry)
        {
            var entity = entry.ToEntity<SkinPackageTemplateEntry, SkinPackageTemplate>();
            var violations = new List<RuleViolation>();
            var packageType = UnitOfWork.SkinPackageTypeRepository.FindById(entry.TypeId);
            if(packageType == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidTypeId, "TypeId", entry.TypeId,
                    ErrorMessage.Messages[ErrorCode.InvalidTypeId]));
                throw new ValidationError(violations);
            }

            var package = UnitOfWork.SkinPackageRepository.FindById(entry.PackageId);
            if (package == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidPackageId, "PackageId", entry.PackageId,
                    ErrorMessage.Messages[ErrorCode.InvalidPackageId]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.TemplateName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullTemplateName, "TemplateName", entry.TemplateName,
                    ErrorMessage.Messages[ErrorCode.NullTemplateName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.TemplateName.Length > 250)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidTemplateName, "TemplateName", entry.TemplateName,
                   ErrorMessage.Messages[ErrorCode.InvalidTemplateName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    var isExisted = UnitOfWork.SkinPackageTemplateRepository.HasDataExists(entry.PackageId, entry.TemplateName);
                    if (isExisted)
                    {
                        violations.Add(new RuleViolation(ErrorCode.DuplicateTemplateName, "TemplateName",
                            entry.TemplateName,
                            ErrorMessage.Messages[ErrorCode.DuplicateTemplateName]));
                        throw new ValidationError(violations);
                    }
                }
            }

            if (string.IsNullOrEmpty(entry.TemplateKey))
            {
                violations.Add(new RuleViolation(ErrorCode.NullTemplateKey, "TemplateKey", entry.TemplateKey,
                    ErrorMessage.Messages[ErrorCode.NullTemplateKey]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.TemplateKey))
            {
                violations.Add(new RuleViolation(ErrorCode.NullTemplateSrc, "TemplateSrc", entry.TemplateSrc,
                    ErrorMessage.Messages[ErrorCode.NullTemplateSrc]));
                throw new ValidationError(violations);
            }

            UnitOfWork.SkinPackageTemplateRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        
        public void UpdateSkinPackageTemplate(SkinPackageTemplateEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.SkinPackageTemplateRepository.FindById(entry.TemplateId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullTemplateId, "TemplateId", entry.TemplateId,
                    ErrorMessage.Messages[ErrorCode.NullTemplateId]));
                throw new ValidationError(violations);
            }

            var packageType = UnitOfWork.SkinPackageTypeRepository.FindById(entry.TypeId);
            if (packageType == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidTypeId, "TypeId", entry.TypeId,
                    ErrorMessage.Messages[ErrorCode.InvalidTypeId]));
                throw new ValidationError(violations);
            }

            var package = UnitOfWork.SkinPackageRepository.FindById(entry.PackageId);
            if (package == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidPackageId, "PackageId", entry.PackageId,
                    ErrorMessage.Messages[ErrorCode.InvalidPackageId]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.TemplateName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullTemplateName, "TemplateName", entry.TemplateName,
                    ErrorMessage.Messages[ErrorCode.NullTemplateName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.TemplateName.Length > 250)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidTemplateName, "TemplateName", entry.TemplateName,
                   ErrorMessage.Messages[ErrorCode.InvalidTemplateName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entity.TemplateName != entry.TemplateName)
                    {
                        var isExisted = UnitOfWork.SkinPackageTemplateRepository.HasDataExists(entry.PackageId, entry.TemplateName);
                        if (isExisted)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateTemplateName, "TemplateName",
                                entry.TemplateName,
                                ErrorMessage.Messages[ErrorCode.DuplicateTemplateName]));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }


            if (string.IsNullOrEmpty(entry.TemplateKey))
            {
                violations.Add(new RuleViolation(ErrorCode.NullTemplateKey, "TemplateKey", entry.TemplateKey,
                    ErrorMessage.Messages[ErrorCode.NullTemplateKey]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.TemplateKey))
            {
                violations.Add(new RuleViolation(ErrorCode.NullTemplateSrc, "TemplateSrc", entry.TemplateSrc,
                    ErrorMessage.Messages[ErrorCode.NullTemplateSrc]));
                throw new ValidationError(violations);
            }

            entity.TypeId = entry.TypeId;
            entity.PackageId = entry.PackageId;
            entity.TemplateName = entry.TemplateName;
            entity.TemplateSrc = entry.TemplateSrc;
            entity.TemplateKey = entry.TemplateKey;
            entity.IsActive = entry.IsActive;

            UnitOfWork.SkinPackageTemplateRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        public void UpdateSkinPackageTemplateStatus(int templateId, bool status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.SkinPackageTemplateRepository.FindById(templateId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundTemplateId, "TemplateId", templateId,
                 ErrorMessage.Messages[ErrorCode.NotFoundTemplateId]));
                throw new ValidationError(violations);
            }
            if (entity.IsActive == status) return;

            entity.IsActive = status;
            UnitOfWork.SkinPackageTemplateRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
      
        #endregion

        #region Skin Package Type
        public IEnumerable<SkinPackageTypeDetail> GetSkinPackageTypes(bool? status)
        {
            var entityList = UnitOfWork.SkinPackageTypeRepository.GetSkinPackageTypes(status);
            return entityList.ToDtos<SkinPackageType, SkinPackageTypeDetail>();
        }
        public SkinPackageTypeDetail GetSkinPackageTypeDetail(int id)
        {
            var item = UnitOfWork.SkinPackageTypeRepository.FindById(id);
            return item.ToDto<SkinPackageType, SkinPackageTypeDetail>();
        }

        public SelectList PopulateSkinPackageTypeSelectList(int? selectedValue = null, bool? isShowSelectText = true)
        {
            return UnitOfWork.SkinPackageTypeRepository.PopulateSkinPackageTypeSelectList(selectedValue, isShowSelectText);
        }
        public SelectList PopulateSkinPackageTypeStatus(bool? selectedValue = null, bool? isShowSelectText = true)
        {
            return UnitOfWork.SkinPackageTypeRepository.PopulateSkinPackageTypeStatus(selectedValue, isShowSelectText);
        }

        public SkinPackageTypeDetail InsertSkinPackageType(SkinPackageTypeEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (string.IsNullOrEmpty(entry.TypeName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullTypeName, "TypeName", entry.TypeName,
                    ErrorMessage.Messages[ErrorCode.NullTypeName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.TypeName.Length > 250)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidTypeName, "TypeName", entry.TypeName,
                   ErrorMessage.Messages[ErrorCode.InvalidTypeName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    var isExisted = UnitOfWork.SkinPackageTypeRepository.HasDataExists(entry.TypeName);
                    if (isExisted)
                    {
                        violations.Add(new RuleViolation(ErrorCode.DuplicateTypeName, "TypeName", entry.TypeName,
                   ErrorMessage.Messages[ErrorCode.DuplicateTypeName]));
                        throw new ValidationError(violations);
                    }
                }
            }

            var entity = entry.ToEntity<SkinPackageTypeEntry, SkinPackageType>();
            UnitOfWork.SkinPackageTypeRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<SkinPackageType, SkinPackageTypeDetail>();
        }

        public void UpdateSkinPackageType(SkinPackageTypeEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.SkinPackageTypeRepository.Find(entry.TypeId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundTypeId, "TypeId", entry.TypeId,
                    ErrorMessage.Messages[ErrorCode.NotFoundTypeId]));
                throw new ValidationError(violations);
            }


            if (string.IsNullOrEmpty(entry.TypeName))
            {
                violations.Add(new RuleViolation(ErrorCode.NullTypeName, "TypeName", entry.TypeName,
                    ErrorMessage.Messages[ErrorCode.NullTypeName]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.TypeName.Length > 250)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidTypeName, "TypeName", entry.TypeName,
                   ErrorMessage.Messages[ErrorCode.InvalidTypeName]));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entity.TypeName != entry.TypeName)
                    {
                        var isExisted = UnitOfWork.SkinPackageTypeRepository.HasDataExists(entry.TypeName);
                        if (isExisted)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicateTypeName, "TypeName",
                                entry.TypeName,
                                ErrorMessage.Messages[ErrorCode.DuplicateTypeName]));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }

            //Assign data
            entity.TypeName = entry.TypeName;
            entity.IsActive = entry.IsActive;

            UnitOfWork.SkinPackageTypeRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public void UpdateSkinPackageTypeStatus(int id, bool status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.SkinPackageTypeRepository.FindById(id);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundTypeId, "TypeId", id, ErrorMessage.Messages[ErrorCode.NotFoundTypeId]));
                throw new ValidationError(violations);
            }
            if (entity.IsActive == status) return;

            entity.IsActive = status;

            UnitOfWork.SkinPackageTypeRepository.Update(entity);
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
                    CacheService = null;
                    DocumentService = null;
                }
                _disposed = true;
            }
            base.Dispose(isDisposing);
        }

        #endregion
    }
}
