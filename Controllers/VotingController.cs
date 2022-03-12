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
            //Gets the Current Campaign

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
            //Gets the Current Campaign
            loginPageModel.Campaign = votingDB.GetCampaign();
            //Gets the Current Campaign Votes
            loginPageModel.CampaignVotes = votingDB.GetCampaignVotes(loginPageModel.Campaign);

            //Retrieves the Vote option selected by the User
            var optionSelected2 = loginPageModel.CampaignVotes.Find(p => p.VoteNumber == VoteOptions);

            //Casts Vote of options selected by the User
            votingDB.CastVote(loginPageModel.Campaign,optionSelected2.VoteDescription,(int)optionSelected2.VoteNumber);
            return View("Voted");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}