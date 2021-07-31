﻿using DevnotMentor.Api.ActionFilters;
using DevnotMentor.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DevnotMentor.Api.CustomEntities.Request.MentorRequest;
using DevnotMentor.Api.Helpers.Extensions;

namespace DevnotMentor.Api.Controllers
{
    [ValidateModelState]
    [ApiController]
    [Route("/mentors/")]
    public class MentorController : BaseController
    {
        private readonly IMentorService mentorService;
        private readonly IApplicationService applicationService;
        private readonly IPairService pairService;

        public MentorController(IMentorService mentorService, IPairService pairService, IApplicationService applicationService)
        {
            this.mentorService = mentorService;
            this.pairService = pairService;
            this.applicationService = applicationService;
        }

        [HttpGet("{userName}")]
        public async Task<IActionResult> GetAsync([FromRoute] string userName)
        {
            var result = await mentorService.GetMentorProfileAsync(userName);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpGet("me/paireds/mentees")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> GetPairedMentees()
        {
            var authenticatedUserId = User.Claims.GetUserId();
            var result = await mentorService.GetPairedMenteesByUserIdAsync(authenticatedUserId);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpGet("me/paireds")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> GetMentorships()
        {
            var authenticatedUserId = User.Claims.GetUserId();
            var result = await pairService.GetMentorshipsOfMentorByUserIdAsync(authenticatedUserId);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpPost]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> Post([FromBody] CreateMentorProfileRequest request)
        {
            request.UserId = User.Claims.GetUserId();

            var result = await mentorService.CreateMentorProfileAsync(request);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpPost("me/applications/{id}/accept")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> AcceptMentee([FromRoute] int id)
        {
            var mentorUserId = User.Claims.GetUserId();

            var result = await applicationService.AcceptApplicationByIdAsync(mentorUserId, id);

            return result.Success ? Success(result) : BadRequest(result);
        }

        [HttpPost("me/applications/{id}/reject")]
        [ServiceFilter(typeof(TokenAuthentication))]
        public async Task<IActionResult> RejectMentee([FromRoute] int id)
        {
            var mentorUserId = User.Claims.GetUserId();

            var result = await applicationService.RejectApplicationByIdAsync(mentorUserId, id);

            return result.Success ? Success(result) : BadRequest(result);
        }
    }
}
