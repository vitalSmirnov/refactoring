using CloneIntime.Entities;
using CloneIntime.Models.DTO;

namespace CloneIntime.Utils.helpers
{
    public class DisciplineHelper
    {

        private List<DisciplineDTO> DisciplineMapper(List<DisciplineEntity> disciplines)
        {
            var result = new List<DisciplineDTO>();
            result.AddRange(disciplines.Select(x => new DisciplineDTO
            {
                Name = x.Name,
                Id = x.Id,
                IsActive = x.IsActive
            }));
            return result;
        }

        public List<DisciplineDTO> FillDisciplines(FacultyEntity disciplines)
        {
            return DisciplineMapper(disciplines.Disciplines);
        }

        public List<DisciplineDTO> FillDisciplines(List<DisciplineEntity> disciplines)
        {
            return DisciplineMapper(disciplines);
        }

    }
}
