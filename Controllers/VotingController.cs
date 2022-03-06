using eVotingSystemWebJS.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace eVotingSystemWebJS.Controllers
{
    public class VotingController : Controller
    {
        //GET
        public IActionResult Index()
        {
            VotingDBContext votingDB = new VotingDBContext();
            LoginPageModel loginPageModel = new LoginPageModel();
            loginPageModel.Campaign = votingDB.GetCampaign();

            if (loginPageModel.Campaign != null)
            {
                loginPageModel.CampaignVotes = votingDB.GetCampaignVotes(loginPageModel.Campaign);

                return View("Index", loginPageModel);
            }
            else
            {
                return View("NoCampaign");
            }
        }
        //POST
        [HttpPost]
        public IActionResult Vote(int VoteOptions)
        {
            VotingDBContext votingDB = new VotingDBContext();
            LoginPageModel loginPageModel = new LoginPageModel();
            loginPageModel.Campaign = votingDB.GetCampaign();
            loginPageModel.CampaignVotes = votingDB.GetCampaignVotes(loginPageModel.Campaign);

            var optionSelected2 = loginPageModel.CampaignVotes.Find(p => p.VoteNumber == VoteOptions);
            return View("Voted");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}