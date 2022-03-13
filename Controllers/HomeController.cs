﻿using eVotingSystemWebJS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace eVotingSystemWebJS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly VotingDBContext _votingDB;
        public HomeController(ILogger<HomeController> logger, VotingDBContext votingDB)
        {
            _logger = logger;
            _votingDB = votingDB;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult LoginUser(User user)
        {
            if (ModelState.IsValid)
            {
                User userRetrieved = _votingDB.ValidateLogin(user);

                if (userRetrieved != null)
                {
                    if (userRetrieved.Voted == true)
                    {
                        return View("AlreadyVoted");
                    }
                    return RedirectToAction("Index", "Voting");
                }
                else
                {
                    ModelState.AddModelError("password", "The Login details are incorrect");
                    return View("Login");
                }
            }
            else
            {
                return View("Login");
            }
        }

        [HttpPost]
        public IActionResult Back()
        {
           return View("Login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}