using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCTask.Interfaces;
using MVCTask.Models;

namespace MVCTask.Controllers {
    [Route("api/tasks")]
    public class TasksController(ApplicationDbContext context, IUsersService usersService, IMapper mapper): ControllerBase {


        [HttpGet]
        public async Task<ActionResult<List<TaskDTO>>> Get() {
            var userId = usersService.GetUserId();

            var selectTasks = await context.Tasks
                .Where(t => t.CreatorUserId == userId)
                .OrderBy(t => t.Order)
                .ProjectTo<TaskDTO>(mapper.ConfigurationProvider)
                .ToListAsync();

            return selectTasks;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Entities.Task>> Get(int id) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var userId = usersService.GetUserId();

            var task = await context.Tasks
                .Include(t => t.Steps.OrderBy(s => s.Order))
                .FirstOrDefaultAsync(t => t.Id == id &&
                    t.CreatorUserId == userId);

            if (task is null) {
                return NotFound();
            }

            var completedSteps = task.Steps.Count(s => s.IsCompleted);
            task.IsCompleted = completedSteps == task.Steps.Count;

            return task;
        }

        [HttpPost]
        public async Task<ActionResult<Entities.Task>> Post([FromBody] string title) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var userId = usersService.GetUserId();

            var tasksExists = await context.Tasks.AnyAsync(t => t.CreatorUserId == userId);

            var biggerOrder = 0;
            if (tasksExists) {
                biggerOrder = await context.Tasks.Where(t => t.CreatorUserId == userId)
                    .Select(t => t.Order).MaxAsync();
            }         

            var task = new Entities.Task {
                Title = title,
                CreatorUserId = userId,
                CreatedAt = DateTime.UtcNow,
                Order = biggerOrder + 1
            };

            context.Add(task);
            await context.SaveChangesAsync();

            return task;
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> EditTask(int id, [FromBody] TaskEditDTO taskEdit) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var userId = usersService.GetUserId();

            var task = await context.Tasks.FirstOrDefaultAsync(t => t.Id == id &&
            t.CreatorUserId == userId);

            if (task is null) {
                return NotFound();
            }

            if (taskEdit.Steps is not null) {
                var completedSteps = taskEdit.Steps.Count(s => s.IsCompleted);
                task.IsCompleted = completedSteps == taskEdit.Steps.Count;
               
            }

            task.Title = taskEdit.Title;
            task.Description = taskEdit.Description;

            await context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var userId = usersService.GetUserId();

            var task = await context.Tasks.FirstOrDefaultAsync(t => t.Id == id &&
            t.CreatorUserId == userId);

            if (task is null) {
                return NotFound();
            }

            context.Remove(task);
            await context.SaveChangesAsync();
            return Ok();
        }


        [HttpPost("sort")]
        public async Task<IActionResult> Sort([FromBody] int[] ids) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var userId = usersService.GetUserId();

            var tasks = await context.Tasks
                .Where(t => t.CreatorUserId == userId).ToListAsync();

            var tasksId = tasks.Select(t => t.Id);

            var idsNotAreInTasksOfUser = ids.Except(tasksId).ToList();

            if (idsNotAreInTasksOfUser.Count != 0) {
                return Forbid();
            }

            var tasksToDictionary = tasks.ToDictionary(x => x.Id);

            for (int i = 0; i < ids.Length; i++) {
                var id = ids[i];
                var task = tasksToDictionary[id];
                task.Order = i + 1;
            }

            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
