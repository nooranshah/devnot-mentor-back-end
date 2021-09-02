﻿using DevnotMentor.Data.Entities;
using DevnotMentor.Common.Enums;
using DevnotMentor.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Data
{
    public class IApplicationRepository : BaseRepository<Application>, IApplicationsRepository
    {
        public IApplicationRepository(MentorDBContext context) : base(context)
        {

        }

        public async Task<bool> AnyWaitingApplicationBetweenMentorAndMenteeAsync(int mentorId, int menteeId)
        {
            return await DbContext.MentorApplications.AnyAsync(x => 
                x.Status == (int)ApplicationStatus.Waiting
                && x.MentorId == mentorId
                && x.MenteeId == menteeId);
        }

        public async Task<IEnumerable<Application>> GetApplicationsByUserIdAsync(int userId)
        {
            return await DbContext.MentorApplications
                .Include(x => x.Mentee).ThenInclude(x => x.User)
                .Include(x => x.Mentor).ThenInclude(x => x.User)
                .Where(x => x.Mentor.UserId == userId || x.Mentee.UserId == userId)
                .ToListAsync();
        }

        public async Task<Application> GetWhichIsWaitingByIdAsync(int applicationId)
        {
            return await DbContext.MentorApplications
                .Include(x => x.Mentee).ThenInclude(x => x.User)
                .Include(x => x.Mentor).ThenInclude(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == applicationId && x.Status == (int)ApplicationStatus.Waiting);
        }
    }
}
