using System;
using System.Collections.Generic;
using Eagle.Entities.Business.Roster;
using Eagle.Repositories;
using Eagle.Services.Dtos.Business.Roster;
using Eagle.Services.Dtos.Common;
using Eagle.Services.EntityMapping.Common;
using Eagle.Services.Validations;

namespace Eagle.Services.Business
{
    public class RosterService : BaseService, IRosterService
    {
        public RosterService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        #region Public Holiday Set
        public PublicHolidaySetDetail InsertPublicHolidaySet(PublicHolidaySetEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullPublicHolidaySetEntry, "PublicHolidaySetEntry", null, ErrorMessage.Messages[ErrorCode.NullPublicHolidaySetEntry]));
                throw new ValidationError(violations);
            }

            var country = UnitOfWork.CountryRepository.FindById(entry.CountryId);
            if (country == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidCountryId, "CountryId", entry.CountryId, ErrorMessage.Messages[ErrorCode.InvalidCountryId]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.Description) || entry.Description.Length > 255)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidPublicHolidaySetName, "PublicHolidaySetName", null,
                    ErrorMessage.Messages[ErrorCode.InvalidPublicHolidaySetName]));
                throw new ValidationError(violations);
            }
            else
            {
                bool isDuplicate = UnitOfWork.PublicHolidaySetRepository.HasPublicHolidaySetExisted(entry.Description, entry.CountryId);
                if (isDuplicate)
                {
                    violations.Add(new RuleViolation(ErrorCode.DuplicatePublicHolidaySet, "PublicHolidaySet",
                            entry.Description, ErrorMessage.Messages[ErrorCode.DuplicatePublicHolidaySet]));
                    throw new ValidationError(violations);
                }
            }

            var entity = entry.ToEntity<PublicHolidaySetEntry, PublicHolidaySet>();
            entity.CreatedDate = DateTime.UtcNow;

            UnitOfWork.PublicHolidaySetRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<PublicHolidaySet, PublicHolidaySetDetail>();
        }
        public void UpdatePublicHolidaySet(PublicHolidaySetEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullPublicHolidaySetEditEntry, "PublicHolidaySetEditEntry", null, ErrorMessage.Messages[ErrorCode.NullPublicHolidaySetEditEntry]));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.PublicHolidaySetRepository.FindById(entry.PublicHolidaySetId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidPublicHolidaySetId, "PublicHolidaySetId", entry.PublicHolidaySetId, ErrorMessage.Messages[ErrorCode.InvalidPublicHolidaySetId]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.Description))
            {
                violations.Add(new RuleViolation(ErrorCode.NullDescription, "Description", null, ErrorMessage.Messages[ErrorCode.NullDescription]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.Description.Length > 250)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidDescription, "Description", null, ErrorMessage.Messages[ErrorCode.InvalidDescription]));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entity.Description != entry.Description)
                    {
                        bool isDuplicate = UnitOfWork.PublicHolidaySetRepository.HasPublicHolidaySetExisted(entry.Description, entry.CountryId);
                        if (isDuplicate)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicatePublicHolidaySet, "PublicHolidaySet",
                                    entry.Description, ErrorMessage.Messages[ErrorCode.DuplicatePublicHolidaySet]));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }

            entity.CountryId = entry.CountryId;
            entity.Description = entry.Description;
            entity.LastModifiedDate =DateTime.UtcNow;

            UnitOfWork.PublicHolidaySetRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion

        #region Public Holiday
        public PublicHolidayDetail InsertPublicHoliday(PublicHolidayEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullPublicHolidayEntry, "PublicHolidayEntry", null, ErrorMessage.Messages[ErrorCode.NullPublicHolidayEntry]));
                throw new ValidationError(violations);
            }

            var publicHolidaySet = UnitOfWork.PublicHolidaySetRepository.FindById(entry.PublicHolidaySetId);
            if (publicHolidaySet == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidPublicHolidaySetId, "PublicHolidaySetId", entry.PublicHolidaySetId, ErrorMessage.Messages[ErrorCode.InvalidPublicHolidaySetId]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.Description) || entry.Description.Length > 255)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidDescription, "Description", null,
                    ErrorMessage.Messages[ErrorCode.InvalidDescription]));
                throw new ValidationError(violations);
            }
            else
            {
                bool isDuplicate = UnitOfWork.PublicHolidayRepository.HasPublicHolidayExisted(entry.Holiday, entry.PublicHolidaySetId);
                if (isDuplicate)
                {
                    violations.Add(new RuleViolation(ErrorCode.DuplicatePublicHoliday, "PublicHoliday",
                            entry.Holiday, ErrorMessage.Messages[ErrorCode.DuplicatePublicHoliday]));
                    throw new ValidationError(violations);
                }
            }

            var entity = entry.ToEntity<PublicHolidayEntry, PublicHoliday>();
            entity.CreatedDate = DateTime.UtcNow;

            UnitOfWork.PublicHolidayRepository.Insert(entity);
            UnitOfWork.SaveChanges();
            return entity.ToDto<PublicHoliday, PublicHolidayDetail>();
        }
        public void UpdatePublicHoliday(PublicHolidayEditEntry entry)
        {
            var violations = new List<RuleViolation>();
            if (entry == null)
            {
                violations.Add(new RuleViolation(ErrorCode.NullPublicHolidayEditEntry, "PublicHolidayEditEntry", null, ErrorMessage.Messages[ErrorCode.NullPublicHolidayEditEntry]));
                throw new ValidationError(violations);
            }

            var entity = UnitOfWork.PublicHolidayRepository.FindById(entry.PublicHolidayId);
            if (entity == null)
            {
                violations.Add(new RuleViolation(ErrorCode.InvalidPublicHolidaySetId, "PublicHolidaySetId", entry.PublicHolidaySetId, ErrorMessage.Messages[ErrorCode.InvalidPublicHolidaySetId]));
                throw new ValidationError(violations);
            }

            if (string.IsNullOrEmpty(entry.Description))
            {
                violations.Add(new RuleViolation(ErrorCode.NullDescription, "Description", null, ErrorMessage.Messages[ErrorCode.NullDescription]));
                throw new ValidationError(violations);
            }
            else
            {
                if (entry.Description.Length > 250)
                {
                    violations.Add(new RuleViolation(ErrorCode.InvalidDescription, "Description", null, ErrorMessage.Messages[ErrorCode.InvalidDescription]));
                    throw new ValidationError(violations);
                }
                else
                {
                    if (entity.Description != entry.Description)
                    {
                        bool isDuplicate = UnitOfWork.PublicHolidayRepository.HasPublicHolidayExisted(entry.Holiday, entry.PublicHolidaySetId);
                        if (isDuplicate)
                        {
                            violations.Add(new RuleViolation(ErrorCode.DuplicatePublicHolidaySet, "PublicHolidaySet",
                                    entry.Description, ErrorMessage.Messages[ErrorCode.DuplicatePublicHolidaySet]));
                            throw new ValidationError(violations);
                        }
                    }
                }
            }

            entity.PublicHolidaySetId = entry.PublicHolidaySetId;
            entity.Holiday = entry.Holiday;
            entity.Description = entry.Description;
            entity.LastModifiedDate = DateTime.UtcNow;

            UnitOfWork.PublicHolidayRepository.Update(entity);
            UnitOfWork.SaveChanges();
        }
        #endregion
    }
}
