﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FundMeUpMVC.Controllers
{
    [Route("")]
    public class RootController : Controller
    {
        [HttpGet("")]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
