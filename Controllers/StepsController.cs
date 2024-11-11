using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using MVCTask.Entities;
using MVCTask.Interfaces;
using MVCTask.Models;

namespace MVCTask.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class StepsController(ApplicationDbContext context, IUsersService usersService, IStringLocalizer<StepsController> localizer): ControllerBase {

        [HttpPost("{taskId:int}")]
        public async Task<ActionResult<Step>> CreateStep(int taskId, CreateStepDTO createStepDTO) {
            var userId = usersService.GetUserId();
            var task = await context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
            if (task == null) {
                return NotFound();
            }

            if (userId != task.CreatorUserId) {
                return Forbid();
            }

            var stepsExists = await context.Steps.AnyAsync(s => s.TaskId == taskId);

            var biggerOrder = stepsExists ? await context.Steps.Where(s => s.TaskId == taskId).MaxAsync(s => s.Order) : 0;

            var step = new Step {
                TaskId = taskId,
                Description = createStepDTO.Description,
                IsCompleted = createStepDTO.IsCompleted,
                Order = biggerOrder + 1
            };
            context.Steps.Add(step);
            await context.SaveChangesAsync();
            return step;
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Step>> UpdateStep(Guid id,[FromBody] CreateStepDTO updateStepDTO) {

            var userId = usersService.GetUserId();
      
            var step = await context.Steps
                .Include(s => s.Task)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (step == null) {
                return NotFound();
            }

            if (userId != step.Task.CreatorUserId) {
                return Forbid();
            }

            step.Description = updateStepDTO.Description;
            step.IsCompleted = updateStepDTO.IsCompleted;
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteStep(Guid id) {

            var userId = usersService.GetUserId();

            var step = await context.Steps
                .Include(s => s.Task)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (step == null) {
                return NotFound();
            }

            if (userId != step.Task.CreatorUserId) {
                return Forbid();
            }

            context.Steps.Remove(step);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("sort/{taskId:int}")]
        public async Task<ActionResult> Sort(int taskId, [FromBody] Guid[] ids) {

            var userId = usersService.GetUserId();

            var task = await context.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);

            if (task == null) {
                return NotFound();
            }

            if (userId != task.CreatorUserId) {
                return Forbid();
            }

            var steps = await context.Steps.Where(s => s.TaskId == taskId).ToListAsync();

            var stepsIds = steps.Select(s => s.Id);

            var stepsNotAreInTask = ids.Except(stepsIds).ToList();

            if (stepsNotAreInTask.Count != 0) {
                return BadRequest(localizer["ErrorInSteps"]);
            }

            var stepsDictionary = steps.ToDictionary(s => s.Id);

            for (int i = 0; i < ids.Length; i++) {
                var stepId = ids[i];
                var step = stepsDictionary[stepId];
                step.Order = i + 1;
            }

         
            await context.SaveChangesAsync();
            return Ok();
        }

    }
}
