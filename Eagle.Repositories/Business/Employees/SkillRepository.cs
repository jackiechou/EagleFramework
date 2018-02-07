using System.Collections.Generic;
using System.Linq;
using Eagle.Core.Common;
using Eagle.Entities.Business.Employees;
using Eagle.EntityFramework;

namespace Eagle.Repositories.Business.Employees
{
    public class SkillRepository: RepositoryBase<Skill>, ISkillRepository
    {
        public SkillRepository(IDataContext dataContext) : base(dataContext) { }
        public IEnumerable<Skill> GetSkills(string filter, int? page, int? pageSize, ref int? recordCount)
        {
            var skillQueryable = DataContext.Get<Skill>().AsQueryable().Where(s => s.IsActive == true);

            if (!string.IsNullOrEmpty(filter))
            {
                skillQueryable = skillQueryable.Where(s => s.SkillName.ToLower().Contains(filter.ToLower()));
            }

            if (recordCount != null)
            {
                recordCount = skillQueryable.Count();
            }

            skillQueryable = skillQueryable.OrderByDescending(u => u.SkillId);

            if (page != null && pageSize != null)
            {
                skillQueryable = skillQueryable.ApplyPaging(page.Value, pageSize.Value);
            }

            return skillQueryable.AsEnumerable();
        }
        public IEnumerable<Skill> GetSkillsByEmployee(int employeeId)
        {
            var query = (DataContext.Get<Skill>()
                            .Join(DataContext.Get<EmployeeSkill>(),
                                skill => skill.SkillId,
                                memberskill => memberskill.SkillId,
                                (skill, memberskill) => new { skill, memberskill })
                            .Where(@t => @t.memberskill.EmployeeId == employeeId && @t.skill.IsActive == true)
                            .Select(@t => new
                            {
                                Id = @t.skill.SkillId,
                                Name = @t.skill.SkillName,
                                CreatedDate = @t.skill.CreatedDate

                            })).ToList()
                            .Select(x => new Skill()
                            {
                                SkillId = x.Id,
                                SkillName = x.Name,
                                CreatedDate = x.CreatedDate
                            });

            return query.AsEnumerable();
        }
        public List<int> GetSkillsByEmployeeId(int employeeId)
        {
            return (DataContext.Get<Skill>()
                .Join(DataContext.Get<EmployeeSkill>(),
                    skill => skill.SkillId,
                    memberskill => memberskill.SkillId,
                    (skill, memberskill) => new { skill, memberskill })
                .Where(@t => @t.memberskill.EmployeeId == employeeId && @t.skill.IsActive == true)
                .Select(@t => @t.skill.SkillId)).ToList();
        }
        public bool HasNameExisted(string name)
        {
            return DataContext.Get<Skill>().Any(s => Equals(s.SkillName.ToLower(), name.ToLower()) );
        }
       
    }
}
