using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Entities.SystemManagement;
using Eagle.Repositories;
using Eagle.Services.Dtos.Common;
using Eagle.Services.Dtos.SystemManagement;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.SystemManagement
{
    public class LanguageService : BaseService, ILanguageService
    {
        public LanguageService(IUnitOfWork unitOfWork) : base(unitOfWork){ }

        public ApplicationLanguageDetail GetSelectedLanguage(Guid applicationId)
        {
            var entity = UnitOfWork.ApplicationLanguageRepository.GetSelectedLanguage(applicationId);
            return entity.ToDto<ApplicationLanguage, ApplicationLanguageDetail>();
        }

        public IEnumerable<ApplicationLanguageDetail> GetApplicationLanguages(Guid applicationId, LanguageSearchEntry filter, ref int? recordCount,
           string orderBy = null, int? page = null, int? pageSize = null)
        {
            var lst = UnitOfWork.ApplicationLanguageRepository.GetApplicationLanguages(applicationId, filter.Status, ref recordCount, orderBy, page, pageSize);
            return lst.Select(sourceEntry => new ApplicationLanguageDetail
            {
                ApplicationLanguageId = sourceEntry.ApplicationLanguageId,
                ApplicationId = sourceEntry.ApplicationId,
                LanguageCode = sourceEntry.LanguageCode,
                IsSelected = sourceEntry.IsSelected,
                Status = sourceEntry.Status,
                Language = new LanguageDetail
                {
                    LanguageId = sourceEntry.Language.LanguageId,
                    LanguageCode = sourceEntry.Language.LanguageCode,
                    LanguageName = sourceEntry.Language.LanguageName,
                    Description = sourceEntry.Language.Description,
                    Status = sourceEntry.Language.Status,
                    ModifiedDate = sourceEntry.Language.ModifiedDate
                }
            });
        }

        public ApplicationLanguageDetail GetApplicationLanguageDetail(Guid applicationId, string languageCode)
        {
            var entity = UnitOfWork.ApplicationLanguageRepository.GetDetails(applicationId, languageCode);
            return entity.ToDto<ApplicationLanguage, ApplicationLanguageDetail>();
        }
        
        public SelectList PopulateApplicationLanguages(Guid applicationId, ApplicationLanguageStatus? status = null, string selectedValue = null, bool isShowSelectText = false)
        {
            return UnitOfWork.ApplicationLanguageRepository.PopulateApplicationLanguages(applicationId, status, selectedValue, isShowSelectText);
        }

        public void EditApplicationLanguages(Guid applicationId, ApplicationLanguageListEntry entry)
        {
            if (entry.SelectedLanguages == null || !entry.SelectedLanguages.Any()) return;

            var applicationLanguages = UnitOfWork.ApplicationLanguageRepository.GetApplicationLanguageList(applicationId).ToList();
            if (applicationLanguages.Any())
            {
                var existingItems = applicationLanguages.Select(x => x.LanguageCode).ToList();
                var latestItems = entry.SelectedLanguages.Select(x => x).ToList();

                //Get the elements in latest list a but not in previous list b - Except
                var exceptLatestList = latestItems.Except(existingItems).ToList();
                if (exceptLatestList.Count > 0)
                {
                    foreach (var languageCode in exceptLatestList)
                    {
                        InsertApplicationLanguage(applicationId, new ApplicationLanguageEntry
                        {
                            LanguageCode = languageCode
                        });
                    }
                }

                //Get the elements in previous list b but not in latest list a - Except
                var exceptPreviousList = existingItems.Except(latestItems).ToList();
                if (exceptPreviousList.Count > 0)
                {
                    foreach (var languageCode in exceptPreviousList)
                    {
                        var applicationLanguage = UnitOfWork.ApplicationLanguageRepository.GetDetails(applicationId, languageCode);
                        if (applicationLanguage == null) return;

                        UnitOfWork.ApplicationLanguageRepository.Delete(applicationLanguage);
                    }
                    UnitOfWork.SaveChanges();
                }

            }
            else
            {
                foreach (var item in entry.SelectedLanguages)
                {
                    InsertApplicationLanguage(applicationId, new ApplicationLanguageEntry
                    {
                        LanguageCode = item,
                    });
                }
            }
        }

        public void InsertApplicationLanguage(Guid applicationId, ApplicationLanguageEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (string.IsNullOrEmpty(entry.LanguageCode))
            {
                violations.Add(new RuleViolation(ErrorCode.NullLanguageCode, "LanguageCode"));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.LanguageCode.Length > 250)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidLanguageCode, "LanguageCode",
                            entry.LanguageCode));
                    throw new ValidationError(violations);
                }
                else
                {
                    var item = UnitOfWork.ApplicationLanguageRepository.GetDetails(applicationId, entry.LanguageCode);
                    if (item != null)
                    {
                        violations.Add(new RuleViolation(ErrorCode.DuplicateLanguageCode, "LanguageCode",
                               entry.LanguageCode));
                        throw new ValidationError(violations);
                    }
                }
            }

            var entity = new ApplicationLanguage
            {
                ApplicationId = applicationId,
                LanguageCode = entry.LanguageCode,
                IsSelected = false,
                Status = ApplicationLanguageStatus.Active
            };

            UnitOfWork.ApplicationLanguageRepository.Insert(entity);
            UnitOfWork.SaveChanges();
        }
        
        public void UpdateSelectedApplicationLanguage(Guid applicationId, string languageCode)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ApplicationLanguageRepository.GetDetails(applicationId, languageCode);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundLanguage, "Language"));
                throw new ValidationError(violations);
            }

            if (entity.IsSelected) return;

            var lst = UnitOfWork.ApplicationLanguageRepository.GetApplicationLanguageList(applicationId).ToList();
            if (!lst.Any()) return;

            foreach (var item in lst)
            {
                item.IsSelected = (item.LanguageCode == languageCode);
                UnitOfWork.ApplicationLanguageRepository.Update(item);
            }
            UnitOfWork.SaveChanges();
        }

        public void UpdateApplicationLanguageStatus(Guid applicationId, string languageCode, ApplicationLanguageStatus status)
        {
            var violations = new List<RuleViolation>();
            var entity = UnitOfWork.ApplicationLanguageRepository.GetDetails(applicationId, languageCode);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NotFoundLanguage, "Language"));
                throw new ValidationError(violations);
            }
            if (entity.Status == status) return;

            entity.Status = status;
            UnitOfWork.ApplicationLanguageRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }

        public MultiSelectList PopulateLanguageMultiSelectList(LanguageStatus? status = null, string[] selectedValues = null)
        {
            return UnitOfWork.LanguageRepository.PopulateLanguageMultiSelectList(status, selectedValues);
        }
        public MultiSelectList PopulateAvailableLanguageMultiSelectList(LanguageStatus? status = null, string[] selectedValues = null)
        {
            return UnitOfWork.LanguageRepository.PopulateAvailableLanguageMultiSelectList(status, selectedValues);
        }
        public MultiSelectList PopulateSelectedLanguageMultiSelectList(Guid applicationId, ApplicationLanguageStatus? status = null, string[] selectedValues = null)
        {
            return UnitOfWork.ApplicationLanguageRepository.PopulateeApplicationLanguageMultiSelectList(applicationId, status, selectedValues);
        }
    }
}
