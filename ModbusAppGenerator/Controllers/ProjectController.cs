using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using Microsoft.AspNet.Identity;
using ModbusAppGenerator.Core.Models;
using ModbusAppGenerator.Core.Services.Interfaces;
using ModbusAppGenerator.DataAccess.Enums;
using ModbusAppGenerator.ViewModels.ProjectViewModels;

namespace ModbusAppGenerator.Controllers
{
    [Authorize]
    public class ProjectController : BaseController
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public ActionResult Index()
        {
            var currentUserId = User.Identity.GetUserId();

            var usersProjects = _projectService.GetUserProjects(currentUserId);

            var model = new List<ProjectListItemViewModel>();

            foreach (var project in usersProjects)
            {
                var viewProject = Mapper.Map<Project, ProjectListItemViewModel>(project);

                if (project.ConnectionSettings.GetType() == typeof(IpConnectionSettings))
                {
                    viewProject.ConnectionType = ConnectionTypes.Ip;
                    viewProject.Host = ((IpConnectionSettings)project.ConnectionSettings).Host;
                    viewProject.Port = ((IpConnectionSettings)project.ConnectionSettings).Port;
                }
                else if (project.ConnectionSettings.GetType() == typeof(ComConnectionSettings))
                {
                    viewProject.ConnectionType = ConnectionTypes.Com;
                    viewProject.BaudRate = ((ComConnectionSettings)project.ConnectionSettings).BaudRate;
                    viewProject.DataBits = ((ComConnectionSettings)project.ConnectionSettings).DataBits;
                    viewProject.Parity = ((ComConnectionSettings)project.ConnectionSettings).Parity;
                    viewProject.PortName = ((ComConnectionSettings)project.ConnectionSettings).PortName;
                    viewProject.StopBits = ((ComConnectionSettings)project.ConnectionSettings).StopBits;
                }

                model.Add(viewProject);
            }

            return View(model);
        }

        public ActionResult Details(int id)
        {
            var project = _projectService.Get(id, User.Identity.GetUserId());

            var model = Mapper.Map<Project, DetailsViewModel>(project);

            if (project.ConnectionSettings.GetType() == typeof(IpConnectionSettings))
            {
                model.ConnectionType = ConnectionTypes.Ip;
                model.Host = ((IpConnectionSettings)project.ConnectionSettings).Host;
                model.Port = ((IpConnectionSettings)project.ConnectionSettings).Port;
            }
            else if (project.ConnectionSettings.GetType() == typeof(ComConnectionSettings))
            {
                model.ConnectionType = ConnectionTypes.Com;
                model.BaudRate = ((ComConnectionSettings)project.ConnectionSettings).BaudRate;
                model.DataBits = ((ComConnectionSettings)project.ConnectionSettings).DataBits;
                model.Parity = ((ComConnectionSettings)project.ConnectionSettings).Parity;
                model.PortName = ((ComConnectionSettings)project.ConnectionSettings).PortName;
                model.StopBits = ((ComConnectionSettings)project.ConnectionSettings).StopBits;
            }

            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var project = Mapper.Map<CreateProjectViewModel, Project>(model);

            switch (model.ConnectionType)
            {
                case ConnectionTypes.Ip:
                    project.ConnectionSettings = new IpConnectionSettings();

                    var newIpProjectId = _projectService.Add(project, User.Identity.GetUserId());

                    return RedirectToAction("CreateIpProject", new { projectId = newIpProjectId });
                case ConnectionTypes.Com:
                    project.ConnectionSettings = new IpConnectionSettings();

                    var newComProjectId = _projectService.Add(project, User.Identity.GetUserId());

                    return RedirectToAction("CreateComProject", new { projectId = newComProjectId });
            }

            return View(model);
        }

        public ActionResult CreateIpProject(int projectId)
        {
            var project = _projectService.Get(projectId, User.Identity.GetUserId());

            var ipProject = Mapper.Map<Project, CreateIpProjectViewModel>(project);

            return View(ipProject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateIpProject(CreateIpProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var project = _projectService.Get(model.Id, User.Identity.GetUserId());

            project.ConnectionSettings = new IpConnectionSettings()
            {
                Host = model.Host,
                Port = model.Port.Value
            };

            _projectService.Edit(project, User.Identity.GetUserId());

            return RedirectToAction("UpdateActions", new { id = project.Id });
        }

        public ActionResult CreateComProject(int projectId)
        {
            var project = _projectService.Get(projectId, User.Identity.GetUserId());

            var comProject = Mapper.Map<Project, CreateComProjectViewModel>(project);

            return View(comProject);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateComProject(CreateComProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var project = _projectService.Get(model.Id, User.Identity.GetUserId());

            project.ConnectionSettings = new ComConnectionSettings()
            {
                BaudRate = model.BaudRate.Value,
                DataBits = model.DataBits.Value,
                Parity = model.Parity.Value,
                PortName = model.PortName,
                StopBits = model.StopBits.Value
            };

            _projectService.Edit(project, User.Identity.GetUserId());

            return RedirectToAction("UpdateActions", new { id = project.Id });
        }

        public ActionResult ActionForm(int id)
        {
            var project = _projectService.Get(id, User.Identity.GetUserId());

            var model = Mapper.Map<Project, AddProjectActionsViewModel>(project);

            for (int i = 0; i < model.Actions.Count; i++)
            {
                model.Actions[i].Number = i;
            }

            return PartialView(model);
        }

        public ActionResult UpdateActions(int id)
        {
            ViewData["ProjectId"] = id;

            return View();
        }

        [HttpPost]
        public ActionResult UpdateAction(ActionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var action = Mapper.Map<ActionViewModel, SlaveAction>(model);

            if (model.Id == null || model.Id == 0)
            {
                _projectService.AddAction(model.ProjectId, action, User.Identity.GetUserId());
            }
            else
            {
                _projectService.EditAction(model.ProjectId, action, User.Identity.GetUserId());
            }

            return RedirectToAction("ActionForm", new { id = model.ProjectId });
        }
        
        [HttpPost]
        public ActionResult DeleteAction(int id, int projectId)
        {
            _projectService.DeleteAction(projectId, id, User.Identity.GetUserId());

            return RedirectToAction("ActionForm", new { id = projectId });
        }

        public ActionResult Edit(int id)
        {
            var project = _projectService.Get(id, User.Identity.GetUserId());

            if (project.ConnectionSettings.GetType() == typeof(IpConnectionSettings))
            {
                return RedirectToAction("EditIpProject", new { id });
            }

            if (project.ConnectionSettings.GetType() == typeof(ComConnectionSettings))
            {
                return RedirectToAction("EditComProject", new { id });
            }

            return null;
        }

        public ActionResult EditIpProject(int id)
        {
            var project = _projectService.Get(id, User.Identity.GetUserId());

            var model = Mapper.Map<Project, EditIpProjectViewModel>(project);

            return View(model);
        }

        [HttpPost]
        public ActionResult EditIpProject(EditIpProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var connectionSettings = (IpConnectionSettings)_projectService.Get(model.Id, User.Identity.GetUserId()).ConnectionSettings;
            connectionSettings.Host = model.Host;
            connectionSettings.Port = model.Port;

            var project = Mapper.Map<EditIpProjectViewModel, Project>(model);

            project.ConnectionSettings = connectionSettings;

            _projectService.Edit(project, User.Identity.GetUserId());

            return RedirectToAction("EditIpProject", new { id = project.Id });
        }

        public ActionResult EditComProject(int id)
        {
            var project = _projectService.Get(id, User.Identity.GetUserId());

            var model = Mapper.Map<Project, EditComProjectViewModel>(project);

            return View(model);
        }

        [HttpPost]
        public ActionResult EditComProject(EditComProjectViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var connectionSettings = (ComConnectionSettings)_projectService.Get(model.Id, User.Identity.GetUserId()).ConnectionSettings;
            connectionSettings.BaudRate = model.BaudRate.Value;
            connectionSettings.DataBits = model.DataBits.Value;
            connectionSettings.Parity = model.Parity.Value;
            connectionSettings.PortName = model.PortName;
            connectionSettings.StopBits = model.StopBits.Value;

            var project = Mapper.Map<EditComProjectViewModel, Project>(model);

            project.ConnectionSettings = connectionSettings;

            _projectService.Edit(project, User.Identity.GetUserId());

            return RedirectToAction("EditComProject", new { id = project.Id });
        }
        
        public ActionResult EditActions(int id)
        {
            var project = _projectService.Get(id, User.Identity.GetUserId());

            var model = Mapper.Map<Project, EditActionsViewModel>(project);

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            _projectService.Delete(id, User.Identity.GetUserId());

            return RedirectToAction("Index");
        }

        public ActionResult Download(int id, ApplicationType type)
        {
            var zipFile = _projectService.DownloadProject(id, type, User.Identity.GetUserId(), Server.MapPath("~/"));
            var project = _projectService.Get(id, User.Identity.GetUserId());

            var stringApplictionType = "";

            switch (type)
            {
                case ApplicationType.Console:
                    stringApplictionType = "Console Application";
                    break;
                case ApplicationType.Service:
                    stringApplictionType = "Windows Service Application";
                    break;
            }

            string contentType = "application/zip";
            HttpContext.Response.ContentType = contentType;
            var result = new FileContentResult(zipFile, contentType)
            {
                FileDownloadName = $"{project.Name} ({ stringApplictionType }).zip"
            };

            return result;
        }
        
        [HttpPost]
        public JsonResult Test(int id, int cyclesCount)
        {
            var results = _projectService.TestProject(id, cyclesCount, User.Identity.GetUserId());

            return Json(results);
        }
    }
}