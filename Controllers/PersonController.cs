using MVCTask.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVCTask.Entities;

namespace BudgetManagement.Controllers {
    [Authorize]
    public class PersonController: Controller {
        private readonly IPersonRepository personRepository;

        public PersonController(IPersonRepository personRepository) {
            this.personRepository = personRepository;
        }

        // GET: PersonController
        public async Task<ActionResult> Index(string id) {
            if (!ModelState.IsValid) {
                return View(ModelState);
            }
            var model = await personRepository.GetPersonById(id);
            if (model == null) {
                return RedirectToAction("create");
            }
            return View("Index", model);
        }

        public ActionResult Create() {
            if (!ModelState.IsValid) {
                return View(ModelState);
            }
            var model = new Person();
            return View("Index", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection) {
            if (!ModelState.IsValid) {
                return View(ModelState);
            }
            var model = new Person(); // Create model
            return RedirectToAction("index", "person", new { id = model.Id });
        }

        public async Task<ActionResult> InfoAddress(int personId) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var model = await personRepository.GetAddressByPersonId(personId);
            if (model == null) {
                return NotFound();
            }
            return View("Address", model);
        }

        // GET: PersonController/Edit/5
        public ActionResult Edit(int id) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return View();
        }

        // POST: PersonController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            try {
                return RedirectToAction(nameof(Index));
            } catch {
                return View();
            }
        }

        // GET: PersonController/Delete/5
        public ActionResult Delete(int id) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            return View();
        }

        // POST: PersonController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection) {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            try {
                return RedirectToAction(nameof(Index));
            } catch {
                return View();
            }
        }
    }
}
