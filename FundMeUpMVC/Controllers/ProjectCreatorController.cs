﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FundMeUp.Services;
using FundMeUpMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using X.PagedList;

namespace FundMeUpMVC.Controllers
{
    public class ProjectCreatorController : Controller
    {
        private readonly ILogger<ProjectController> logger;
        private IProjectCreatorManager projectCreatorManager;
        private IProjectManager projectMng;
        private IBackerProjectManager  backerprojectMng;

        public ProjectCreatorController(ILogger<ProjectController> logger, IProjectCreatorManager projectCreatorManager,
            IProjectManager projectMng, IBackerProjectManager backerprojectMng)
        {
            this.logger = logger;
            this.projectCreatorManager = projectCreatorManager;
            this.projectMng = projectMng;
            this.backerprojectMng = backerprojectMng;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreateProjectCreator()
        {
            var viewModel = new ProjectCreatorModel();
            return View(viewModel);
        }
        public IActionResult AllProjectCreators()
        {
            var viewModel = new ProjectCreatorModel();
            viewModel.ProjectCreators = projectCreatorManager.GetAllProjectCreators();
            return View(viewModel);
        }
        public IActionResult Dashboard(int? page)
        {
            int pageSize = 2;
            int pageNumber = (page ?? 1);

            var projectId = projectMng.FindProjectByProjectCreator(1).Id;

            PCDashboardViewModel pcdash = new PCDashboardViewModel()
            {
                PendingBackerProjects = backerprojectMng.GetPendingProjectFundings(projectId).ToList(), //Project - Startup
                AcceptedBackerProjects = backerprojectMng.GetAcceptedProjectFundings(projectId).ToPagedList(pageNumber, pageSize),
                ProjectId = projectId
            };
            return View(pcdash);
        }

        //Search for Accepted Fundings
        [HttpPost]
        public IActionResult Dashboard([FromBody] PCDashboardViewModel pcdashboard, int? page)
        {
            int pageSize = 2;
            int pageNumber = (page ?? 1);
            int projectId = projectMng.FindProjectByProjectCreator(1).Id;

            PCDashboardViewModel pcdash = new PCDashboardViewModel()
            {
                PendingBackerProjects = backerprojectMng.GetPendingProjectFundings(projectId).ToList(),
                AcceptedBackerProjects = backerprojectMng.GetAcceptedProjectFundings(projectId)
                                .Where(f => f.DoF >= pcdashboard.SearchStartDate && f.DoF <= pcdashboard.SearchEndDate).ToPagedList(pageNumber, pageSize),
                ProjectId = projectId,
                SearchStartDate = pcdashboard.SearchStartDate,
                SearchEndDate = pcdashboard.SearchEndDate
            };
            return PartialView("Dashboard", pcdash);
        }
    }
}
