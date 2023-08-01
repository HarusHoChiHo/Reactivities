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

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {

        [HttpGet] //api/activities
        public async Task<ActionResult<List<Activity>>> GetActivities(CancellationToken cancellationToken)
        {
            return await Mediator.Send(new List.Query(), cancellationToken);
        }

        [HttpGet("{id}")] //api/acitivities/[id]
        public async Task<ActionResult<Activity>> GetActivity(Guid id, CancellationToken cancellationToken)
        {
            return await Mediator.Send(new Details.Query { Id = id}, cancellationToken);
        }

        [HttpPost]
        public async Task<IActionResult> CreateActivity(Activity activity, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new Create.Command { Activity = activity }, cancellationToken));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditActivity(Guid id, Activity activity, CancellationToken cancellationToken)
        {
            activity.Id = id;
            return Ok(await Mediator.Send(new Edit.Command { Activity = activity }, cancellationToken));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id, CancellationToken cancellationToken)
        {
            return Ok(await Mediator.Send(new Delete.Command { Id = id }, cancellationToken));
        }
    }
}