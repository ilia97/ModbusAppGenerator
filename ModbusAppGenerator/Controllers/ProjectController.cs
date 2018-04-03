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

            var model = Mapper.Map<IList<Project>, IList<ProjectListItemViewModel>>(usersProjects);

            return View(model);
        }

        public ActionResult Details(int id)
        {
            var project = _projectService.Get(id, User.Identity.GetUserId());

            var model = Mapper.Map<Project, DetailsViewModel>(project);

            if (project.ConnectionSettings.GetType() == typeof(IpConnectionSettings))
            {
                model.ConnectionType = DataAccess.Enums.ConnectionTypes.Ip;
                model.Host = ((IpConnectionSettings)project.ConnectionSettings).Host;
                model.Port = ((IpConnectionSettings)project.ConnectionSettings).Port;
            }
            else if (project.ConnectionSettings.GetType() == typeof(ComConnectionSettings))
            {
                model.ConnectionType = DataAccess.Enums.ConnectionTypes.Com;
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

            var newProjectId = _projectService.Add(project, User.Identity.GetUserId());

            switch (model.ConnectionType)
            {
                case DataAccess.Enums.ConnectionTypes.Ip:
                    return RedirectToAction("CreateIpProject", new { projectId = newProjectId });
                case DataAccess.Enums.ConnectionTypes.Com:
                    return RedirectToAction("CreateComProject", new { projectId = newProjectId });
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

        public ActionResult UpdateActions(int id)
        {
            var project = _projectService.Get(id, User.Identity.GetUserId());

            var model = Mapper.Map<Project, AddProjectActionsViewModel>(project);

            for (int i = 0; i < model.Actions.Count; i++)
            {
                model.Actions[i].Number = i;
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult UpdateActions(AddProjectActionsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var actions = Mapper.Map<List<ActionViewModel>, List<SlaveAction>>(model.Actions);

            _projectService.UpdateActions(model.Id, actions, User.Identity.GetUserId());

            return RedirectToAction("Details", new { id = model.Id });
        }

        public ActionResult Edit(int id)
        {
            var project = _projectService.Get(id, User.Identity.GetUserId());

            if (project.ConnectionSettings.GetType() == typeof(IpConnectionSettings))
            {
                return RedirectToAction("EditIpProject", new { id });
            }
            else if (project.ConnectionSettings.GetType() == typeof(ComConnectionSettings))
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

            return RedirectToAction("UpdateActions", new { id = project.Id });
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

            return RedirectToAction("UpdateActions", new { id = project.Id });
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
        
        public ActionResult Test(int id)
        {
            var results = _projectService.TestProject(id, User.Identity.GetUserId());

            var jsonResult = new JsonResult();
            jsonResult.Data = results;

            return jsonResult;
        }
    }
}