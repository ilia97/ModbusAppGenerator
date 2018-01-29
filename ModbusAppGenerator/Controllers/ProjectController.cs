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
using ModbusAppGenerator.Core.Services.Interfaces;
using ModbusAppGenerator.ViewModels.ProjectViewModels;

namespace ModbusAppGenerator.Controllers
{
    [Authorize]
    public class ProjectController : BaseController
    {
        private readonly IProjectService _projectService;
        private readonly IMapper _mapper;

        public ProjectController(IProjectService projectService)
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
        
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateIpProject(CreateIpProjectViewModel model)
        {
            var project = _mapper.Map<CreateIpProjectViewModel, Project>(model);

            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }

            return RedirectToAction("Create", project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComProject(CreateComProjectViewModel model)
        {
            var project = _mapper.Map<CreateComProjectViewModel, Project>(model);

            if (!ModelState.IsValid)
            {
                return View("Create", model);
            }

            return RedirectToAction("Create", project);
        }

        public ActionResult Create(Project model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var newProjectId = _projectService.Add(model, GetCurrentUserId());

            return RedirectToAction("Details", new { id = newProjectId });
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