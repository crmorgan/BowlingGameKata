using NUnit.Framework;

namespace BowingGame
{
	[TestFixture]
	public class BowlingGameTest
	{
		private Game _game;

		[SetUp]
		public void SetUp()
		{
			_game = new Game();
		}

		private void RollMany(int n, int pins)
		{
			for (var i = 0; i < n; i++)
				_game.Roll(pins);
		}


		[Test]
		public void Gutter_game_score_is_zero()
		{
			RollMany(20, 0);
			Assert.That(_game.Score(), Is.EqualTo(0));
		}

		[Test]
		public void All_ones_score_is_20()
		{
			RollMany(20, 1);
			Assert.That(_game.Score(), Is.EqualTo(20));
		}

		[Test]
		public void One_spare_score_is_16()
		{
			RollSpare();
			_game.Roll(3);
			RollMany(17, 0);
			Assert.That(_game.Score(), Is.EqualTo(16));
		}

		[Test]
		public void One_strike_score_is_24()
		{
			RollStrike();
			_game.Roll(3);
			_game.Roll(4);
			RollMany(16, 0);
			Assert.That(_game.Score(), Is.EqualTo(24));
		}

		[Test]
		public void Perfect_game_score_is_300()
		{
			RollMany(12, 10);
			Assert.That(_game.Score(), Is.EqualTo(300));
		}

		private void RollStrike()
		{
			_game.Roll(10);
		}


		private void RollSpare()
		{
			_game.Roll(5);
			_game.Roll(5);
		}
	}

	public class Game
	{
		private int[] _rolls = new int[21];
		private int _currentRoll;

		public void Roll(int pins)
		{
			_rolls[_currentRoll++] = pins;
		}

		public int Score()
		{
			var score = 0;
			var frameIndex = 0;

			for (var frame = 0; frame < 10; frame++)
			{
				if (IsStrike(frameIndex))
				{
					score += 10 + StrikeBonus(frameIndex);
					frameIndex++;
				}
				else if (IsSpareFrame(frameIndex))
				{
					score += SpareBonus(frameIndex);
					frameIndex += 2;
				}
				else
				{
					score += SumOfBallsInFrame(frameIndex);
					frameIndex += 2;
				}
			}

			return score;
		}

		private bool IsStrike(int frameIndex)
		{
			return _rolls[frameIndex] == 10;
		}

		private int SumOfBallsInFrame(int frameIndex)
		{
			return _rolls[frameIndex] + _rolls[frameIndex + 1];
		}

		private int SpareBonus(int frameIndex)
		{
			return 10 + _rolls[frameIndex + 2];
		}

		private int StrikeBonus(int frameIndex)
		{
			return _rolls[frameIndex + 1] + _rolls[frameIndex + 2];
		}

		private bool IsSpareFrame(int frameIndex)
		{
			return _rolls[frameIndex] + _rolls[frameIndex + 1] == 10;
		}
	}
}