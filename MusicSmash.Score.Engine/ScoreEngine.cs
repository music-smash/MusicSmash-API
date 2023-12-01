using MusicSmash.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicSmash.Score.Engine
{
    public class ScoreEngine
    {
        public readonly int K = 24;

        public ScoreEngine(int k)
        {
            K = k;
        }

        public (int deltaScoreLeft, int deltaScoreRight) CalculateGameScoreDelta(Game game)
        {
            (double expectationLeft, double expectationRight) CalculateExpectation(Game game)
            {
                var expectationLeft = 1 / (1 + Math.Pow(10, (game.Right.Score - game.Left.Score) / 400));
                var expectationRight = 1 / (1 + Math.Pow(10, (game.Left.Score - game.Right.Score) / 400));
                return (expectationLeft, expectationRight);
            }

            int CalculateDeltaScore(double expectation, bool isWinner)
            {
                var delta = K * ( (isWinner ? 1 : 0) - expectation);
                return (int)(isWinner ? Math.Ceiling(delta) : Math.Floor(delta));
            }

            (double expectationLeft, double expectationRight) expectations = CalculateExpectation(game);

            return  (
                        CalculateDeltaScore(expectations.expectationLeft, game.Winner == game.Left),
                        CalculateDeltaScore(expectations.expectationRight, game.Winner == game.Right)
                    );
        }

        public IDictionary<Album, int> GetScoreDeltaForAlbumsInRound(Round round)
        {
            var scoreDelta = new Dictionary<Album, int>();

            foreach (var game in round.Games)
            {
                var (deltaScoreLeft, deltaScoreRight) = CalculateGameScoreDelta(game);
                scoreDelta[game.Left] = deltaScoreLeft;
                scoreDelta[game.Right] = deltaScoreRight;
            }

            return scoreDelta;
        }

    }
}
