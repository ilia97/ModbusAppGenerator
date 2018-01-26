using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ModbusAppGenerator.Core.Models;
using ModbusAppGenerator.Core.Services;
using ModbusAppGenerator.ViewModels.ProjectViewModels;

namespace ModbusAppGenerator.Controllers
{
    [Authorize]
    public class ProjectController : BaseController
    {
        private readonly ProjectService _projectService;
        private readonly IMapper _mapper;

        public ProjectController(ProjectService projectService)
        {
            _projectService = projectService;
        }
        
        public ActionResult Index()
        {
            var currentUserId = GetCurrentUserId();

            var usersProjects = _projectService.GetUserProjects(currentUserId);

            return View(usersProjects);
        }
        
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Project/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult CreateIpProject(CreateIpProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var project = _mapper.Map<CreateIpProjectViewModel, Project>(model);

            project.UserId = GetCurrentUserId();

            var newProjectId = _projectService.Add(project);

            return RedirectToAction("Details", new { id = newProjectId });
        }

        public ActionResult CreateComProject(CreateComProjectViewModel model)
        {
            if (ModelState.IsValid)
            {

            }

            return View();
        }

        // POST: Project/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Project/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Project/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Project/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Project/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}