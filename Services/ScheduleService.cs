using CloneIntime.Entities;
using CloneIntime.Models;
using CloneIntime.Models.DTO;
using CloneIntime.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace CloneIntime.Services
{
    public class ScheduleService : IScheduleService
    {
        private readonly Context _context;
        public ScheduleService(Context context)
        {
            _context = context;
        }

        private static List<GroupDTO> fillGroups(List<GroupEntity> pairs)
        {
            var groups = new List<GroupDTO>();
            groups.AddRange(pairs.Select(x => new GroupDTO
            {
                Name = x.Name,
                Number = x.Number,
            }));
            return groups;
        }

        private static List<PairDTO> fillPairs(List<PairEntity> pair)
        {
            var result = new List<PairDTO>();
            result.AddRange(pair.Select(j => new PairDTO
            {
                PairId = j.Id,
                LessonType = j.LessonType,
                Audiroty = j.Auditory.Number,
                Discipline = j.Discipline.Name,
                Groups = fillGroups(j.Group),
                Proffessor = j.Teacher.Name
            }));
            return result;
        }

        private static List<TimeSlotDTO> TimeSlotFilling(List<TimeSlotEntity> timeslot)
        {
            var TimeSlots = new List<TimeSlotDTO>();
            TimeSlots.AddRange(timeslot.Select(j => new TimeSlotDTO
            {
                Pairs = fillPairs(j.Pair),
                SlotNumber = j.PairNumber

            }));
            return TimeSlots;
        }
        private WeekDTO FillSchedule(List<DayEntity> days)
        { 
            var result = new List<DayDTO>();
            result.AddRange(days.Select(x => new DayDTO
            {
                Day = x.Date.ToString(),
                WeekDay = x.DayName,
                Timeslot = TimeSlotFilling(x.Lessons)
            }));

            return new WeekDTO
            {
                Days = result
            };
        }

        public async Task<WeekDTO> GetGroupsSchedule(List<string> groupNumber, DateTime startDate, DateTime endDate)
        {
            var groupScheduleEntity = await _context.DayEntities
                .Where(day => day.Lessons.Any(lesson => lesson.Pair.Any(pair => pair.Group.Any(group => groupNumber.Contains(group.Number)))) 
                && day.Date.Date >= startDate && day.Date.Date <= endDate)
                .Include(day => day.Lessons)
                    .ThenInclude(timeslot => timeslot.Pair)
                        .ThenInclude(pair => pair.Teacher)
                .Include(day => day.Lessons)
                    .ThenInclude(timeslot => timeslot.Pair)
                        .ThenInclude(pair => pair.Auditory)
                .Include(day => day.Lessons)
                    .ThenInclude(timeslot => timeslot.Pair)
                        .ThenInclude(pair => pair.Discipline)
                .Include(day => day.Lessons)
                    .ThenInclude(timeslot => timeslot.Pair)
                        .ThenInclude(pair => pair.Group)
                .OrderBy(x=> x.Date)
                    .ToListAsync();


            var results = FillSchedule(groupScheduleEntity);
            return results;
        }
        public async Task<WeekDTO> GetAuditorySchedule(string audId, DateTime startDate, DateTime endDate)
        {
            var getAuditoryEntity = await _context.DayEntities
                .Where(day => day.Lessons.Any(timeslot => timeslot.Pair.Any(pair => pair.Auditory.Number == audId)) 
                && day.Date.Date >= startDate && day.Date.Date <= endDate)
                .Include(day => day.Lessons)
                    .ThenInclude(timeslot => timeslot.Pair)
                        .ThenInclude(pair => pair.Teacher)
                .Include(day => day.Lessons)
                    .ThenInclude(timeslot => timeslot.Pair)
                        .ThenInclude(pair => pair.Auditory)
                .Include(day => day.Lessons)
                    .ThenInclude(timeslot => timeslot.Pair)
                        .ThenInclude(pair => pair.Discipline)
                .Include(day => day.Lessons)
                    .ThenInclude(timeslot => timeslot.Pair)
                        .ThenInclude(pair => pair.Group)
                    .ToListAsync();


            var results = FillSchedule(getAuditoryEntity);
            return results;
        }

        public async Task<WeekDTO> GetTecherSchedule(string teacherId, DateTime startDate, DateTime endDate)
        {
            var getTeacherEntity = await _context.DayEntities
                .Where(day => day.Lessons.Any(timeslot => timeslot.Pair.Any(pair => pair.Teacher.Id.ToString() == teacherId))
                    && day.Date.Date >= startDate && day.Date.Date <= endDate)
                .Include(day => day.Lessons)
                    .ThenInclude(timeslot => timeslot.Pair)
                        .ThenInclude(pair => pair.Teacher)
                .Include(day => day.Lessons)
                    .ThenInclude(timeslot => timeslot.Pair)
                        .ThenInclude(pair => pair.Auditory)
                .Include(day => day.Lessons)
                    .ThenInclude(timeslot => timeslot.Pair)
                        .ThenInclude(pair => pair.Discipline)
                .Include(day => day.Lessons)
                    .ThenInclude(timeslot => timeslot.Pair)
                        .ThenInclude(pair => pair.Group)
                    .ToListAsync();

            var results = FillSchedule(getTeacherEntity);
            return results;
        }
    }
}
