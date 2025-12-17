using CloneIntime.Entities;
using CloneIntime.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloneIntime.Models
{
    public class Context:DbContext
    {
        public DbSet<EditorEntity> EditorEntity { get; set; }
        public DbSet<AuditoryEntity> AuditoryEntities { get; set; }
        public DbSet<DayEntity> DayEntities { get; set; }
        public DbSet<DirectionEntity> DirectionEntities { get; set; }
        public DbSet<DisciplineEntity> DisciplineEntities { get; set; }
        public DbSet<FacultyEntity> FacultyEntities { get; set; }
        public DbSet<GroupEntity> GroupEntities { get; set; }
        public DbSet<PairEntity> PairEntities { get; set; }
        public DbSet<TeacherEntity> TeachersEntities { get; set; }
        public DbSet<TimeSlotEntity> TimeSlotEntities { get; set; }
        public DbSet<TokenEntity> TokenEntity { get; set; }
        public Context(DbContextOptions<Context> options) : base(options)
        {
            Database.EnsureCreated();
        }

    }
}
