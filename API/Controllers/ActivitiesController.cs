using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistence;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediatR;
using Application.Activities;
using Microsoft.AspNetCore.Http.HttpResults;
using Application.Core;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {

        [HttpGet] //api/activities
        public async Task<IActionResult> GetActivities([FromQuery] ActivityParams param, CancellationToken cancellationToken)
        {
            return HandlePagedResult(await Mediator.Send(new List.Query { Params = param }, cancellationToken));
        }

        [HttpGet("{id}")] //api/acitivities/[id]
        public async Task<IActionResult> GetActivity(Guid id, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(new Details.Query { Id = id }, cancellationToken);

            return HandleResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateActivity(Activity activity, CancellationToken cancellationToken)
        {
            return HandleResult(await Mediator.Send(new Create.Command { Activity = activity }, cancellationToken));
        }

        [Authorize(Policy = "IsActivityHost")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(Guid id, Activity activity, CancellationToken cancellationToken)
        {
            activity.Id = id;
            return HandleResult(await Mediator.Send(new Edit.Command { Activity = activity }, cancellationToken));
        }

        [Authorize(Policy = "IsActivityHost")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id, CancellationToken cancellationToken)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { Id = id }, cancellationToken));
        }

        [HttpPost("{id}/attend")]
        public async Task<IActionResult> Attend(Guid id, CancellationToken cancellationToken)
        {
            return HandleResult(await Mediator.Send(new UpdateAttendance.Command { Id = id }, cancellationToken));
        }
    }
}