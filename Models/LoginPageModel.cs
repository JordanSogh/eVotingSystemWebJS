using System.Collections.Generic;

namespace eVotingSystemWebJS.Models
{
    public class LoginPageModel
    {
        public Campaign Campaign { get; set; }
        public List<CampaignVote> CampaignVotes { get; set; }
        public string SelectedAnswer { get; set; }
    }
}