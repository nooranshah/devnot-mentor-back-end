﻿using DevnotMentor.Api.Entities;
using DevnotMentor.Api.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevnotMentor.Api.Repositories
{
    public class MentorMenteePairsRepository : Repository<MentorMenteePairs>
    {
        private MentorDBContext _context;

        public MentorMenteePairsRepository(MentorDBContext context) : base(context)
        {
            _context = context;
        }

        public int GetCountForContinuesStatusByMenteeUserId(int menteeUserId)
        {
            return _context.MentorMenteePairs.Where(i => i.Mentee.UserId == menteeUserId && i.State == MentorMenteePairStatus.Continues.ToInt()).Count();
        }

        public int GetCountForContinuesStatusByMentorUserId(int mentorUserId)
        {
            return _context.MentorMenteePairs.Where(i => i.Mentor.UserId == mentorUserId && i.State == MentorMenteePairStatus.Continues.ToInt()).Count();
        }
    }
}
