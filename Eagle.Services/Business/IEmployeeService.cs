using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Eagle.Core.Settings;
using Eagle.Services.Dtos.Business.Personnel;

namespace Eagle.Services.Business
{
    public interface IEmployeeService : IBaseService
    {
        #region Employee

        IEnumerable<EmployeeInfoDetail> GetEmployees(int vendorId, EmployeeSearchEntry entry, ref int? recordCount, string orderBy = null,
            int? page = null, int? pageSize = null);

        IEnumerable<EmployeeInfoDetail> GetEmployees(int vendorId, EmployeeStatus? status);

        SelectList PopulateEmployeeSelectList(int vendorId, EmployeeStatus? status = null, int? selectedValue = null,
            bool? isShowSelectText = false);

        MultiSelectList PopulateEmployeeMultiSelectList(int vendorId, EmployeeStatus? status = null, int[] selectedValues = null);
        EmployeeInfoDetail GetEmployeeDetail(int id);
        EmployeeInfoDetail GetEmployeeDetails(int id);

        string GenerateCode(int maxLetters);
        void InsertEmployee(Guid applicationId, Guid userId, int vendorId, EmployeeEntry entry);
        void UpdateEmployee(Guid applicationId, Guid userId, int vendorId, EmployeeEditEntry entry);
        void UpdateEmployeeStatus(Guid userId, int id, EmployeeStatus status);

        #endregion

        #region Job Position

        IEnumerable<JobPositionDetail> GetJobPositions(JobPositionSearchEntry filter, ref int? recordCount,
            string orderBy = null, int? page = null, int? pageSize = null);
        JobPositionDetail GetJobPositionDetail(int id);
        SelectList PoplulateJobPositionSelectList(bool? status = null, int? selectedValue = null, bool? isShowSelectText = true);

        SelectList PopulateJobPositionStatus(bool? selectedValue = true, bool isShowSelectText = false);
        JobPositionDetail InsertJobPosition(JobPositionEntry entry);

        void UpdateJobPosition(JobPositionEditEntry entry);
        void UpdateJobPositionStatus(int id, bool status);

        #endregion

        #region Job Position Skill

        void InsertJobPositionSkill(int positionId, int skillId);

        void DeleteJobPositionSkill(int positionId, int skillId);

        #endregion

        #region Skill

        IEnumerable<SkillDetail> GetSkillsByMember(int employeeId);
        SkillDetail GetSkillDetail(int skillId);
        SkillDetail InsertSkill(SkillEntry entry);
        void UpdateSkill(SkillEditEntry entry);
        void DeleteSkill(int id);

        #endregion

        #region Employee Position

        void InsertEmployeePosition(int employeeId, JobPositionEntry entry);

        void DeleteEmployeePosition(int employeeId, int positionId);

        #endregion

        #region Employee Skill

        void AssignSkill(int employeeId, int skillId);
        void UnAssignSkill(int employeeId, int skillId);
        #endregion

        #region Employee Availability

        void InsertEmployeeAvailability(EmployeeAvailabilityEntry entry);

        void DeleteEmployeeAvailability(int employeeAvailabilityId);

        #endregion

        #region Employee Time-Off

        void InsertEmployeeTimeOff(EmployeeTimeOffEntry entry);

        void DeleteEmployeeTimeOff(int employeeTimeOffId);

        #endregion

        #region Qualification

        string GenerateQualificationNo(int maxLetters);

        QualificationDetail GetQualificationDetail(int id);

        QualificationDetail InsertQualification(Guid applicationId, Guid userId, QualificationEntry entry);
        void UpdateQualification(Guid applicationId, Guid userId, QualificationEditEntry entry);

        void DeleteQualification(int qualificationId);

        #endregion

        #region Reward Discipline

        RewardDisciplineDetail InsertRewardDiscipline(RewardDisciplineEntry entry);

        void UpdateRewardDiscipline(RewardDisciplineEditEntry entry);

        #endregion

        #region Salary

        SalaryDetail InsertSalary(SalaryEntry entry);

        void UpdateSalary(SalaryEditEntry entry);

        #endregion

        #region Termination

        TerminationDetail GetTerminationDetailByEmployeeId(int employeeId);
        TerminationDetail GetTerminationDetail(int terminationId);
        TerminationDetail InsertTermination(TerminationEntry entry);

        void UpdateTermination(TerminationEditEntry entry);

        #endregion

        #region Working History

        IEnumerable<WorkingHistoryDetail> GetWorkingHistories(int employeeId);
        WorkingHistoryDetail GetWorkingHistoryDetail(int workingHistoryId);
        WorkingHistoryDetail InsertWorkingHistory(WorkingHistoryEntry entry);
        void UpdateWorkingHistory(WorkingHistoryEditEntry entry);

        #endregion

    }
}
