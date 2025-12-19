using CloneIntime.Entities;
using CloneIntime.Migrations;
using CloneIntime.Models;
using CloneIntime.Models.DTO;
using CloneIntime.Services.Interfaces;
using CloneIntime.Utils.helpers;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
namespace CloneIntime.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly Context _context;
        private readonly AdminHelper _adminHelper;
        public ScheduleService(Context context, AdminHelper adminHelper)
        {
            _context = context;
            _adminHelper = adminHelper;
        }

        private bool IsDateInInterval(DateTime date, WeekDateDTO interval)
        {
            return date.Date >= interval.StartDate && date.Date <= interval.EndDate;
        }

        private static Expression<Func<PairEntity, PairDTO>> MapPair()
        {
            return p => new PairDTO
            {
                Proffessor = p.Teacher.Name,
                Discipline = p.Discipline.Name,
                Audiroty = p.Auditory.Number,
                Groups = p.Group
                    .Select(g => new GroupDTO
                    {
                        Name = g.Name,
                        Number = g.Number
                    })
                    .ToList()
            };
        }

        private Expression<Func<DayEntity, DayDTO>> MapDay(
            Expression<Func<PairEntity, bool>> pairFilter)
        {
            return day => new DayDTO
            {
                WeekDay = _adminHelper.GetWeekDayNameFromDate(day.Date),
                Timeslot = day.Lessons.Select(lesson => new TimeSlotDTO
                {
                    Pairs = lesson.Pair
                        .AsQueryable()
                        .Where(pairFilter)
                        .Select(MapPair())
                        .ToList()
                }).ToList()
            };
        }
        private Expression<Func<DayEntity, bool>> IsDayInInterval(WeekDateDTO interval, Expression<Func<PairEntity, bool>> filter)
        {
            return day => IsDateInInterval(day.Date, interval) &&
                          day.Lessons.Any(l => l.Pair.AsQueryable().Any(filter));
        }

        private async Task<List<DayDTO>> GetSchedule(WeekDateDTO interval, Expression<Func<PairEntity, bool>> filter)
        {
            return await _context.DayEntities
                 .Where(IsDayInInterval(interval, filter))
                .Select(MapDay(filter))
                .ToListAsync();
        }

        public async Task<WeekDTO> GetGroupsSchedule(List<string> groupNumbers, WeekDateDTO interval)
        {
            Expression<Func<PairEntity, bool>> filter =
                p => p.Group.Any(g => groupNumbers.Contains(g.Number));

            return new WeekDTO { Days = await GetSchedule(interval, filter) };
        }
        public async Task<WeekDTO> GetAuditorySchedule(string audId, WeekDateDTO interval)
        {
            Expression<Func<PairEntity, bool>> filter =
                p => p.Auditory.Number.ToString() == audId;

            return new WeekDTO { Days = await GetSchedule(interval, filter) };
        }

        public async Task<WeekDTO> GetTecherSchedule(string teacherId, WeekDateDTO interval)
        {
            Expression<Func<PairEntity, bool>> filter =
                p => p.Teacher.Id.ToString() == teacherId;

            return new WeekDTO { Days = await GetSchedule(interval, filter) };
        }
    }
    }
