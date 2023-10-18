using TryBets.Odds.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Globalization;

namespace TryBets.Odds.Repository;

public class OddRepository : IOddRepository
{
    protected readonly ITryBetsContext _context;
    public OddRepository(ITryBetsContext context)
    {
        _context = context;
    }

    public Match Patch(int MatchId, int TeamId, string BetValue)
    {
        string BetValueConverted = BetValue.Replace(',', '.');
        decimal BetValueDecimal = Decimal.Parse(BetValueConverted, CultureInfo.InvariantCulture);
        var match = _context.Matches.FirstOrDefault(m => m.MatchId == MatchId);
        var team = _context.Teams.FirstOrDefault(t => t.TeamId == TeamId);
        if (match != null && team != null) {
            if (TeamId != match.MatchTeamBId && TeamId != match.MatchTeamAId) throw new Exception("Team is not in this match");

            var bet = _context.Bets.FirstOrDefault(b => b.MatchId == MatchId && b.TeamId == TeamId);

            if (TeamId == match.MatchTeamAId) {
                match.MatchTeamAValue += BetValueDecimal;
            } else {
                match.MatchTeamBValue += BetValueDecimal;
            }

            if (bet != null) {
                bet.BetValue = bet.BetValue + BetValueDecimal;
                _context.Bets.Update(bet);
            } else {
                bet = new Bet {
                    MatchId = MatchId,
                    TeamId = TeamId,
                    BetValue = BetValueDecimal
                };
                _context.Bets.Add(bet);
            }
            _context.SaveChanges();
            var updatedMatch = _context.Matches.FirstOrDefault(m => m.MatchId == MatchId);
            return updatedMatch!;
            } else {
                throw new Exception("Match or Team not founded");
            }
    }
}