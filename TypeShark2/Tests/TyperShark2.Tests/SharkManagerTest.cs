using Shouldly;
using TypeShark2.Shared.Services;
using Xunit;

namespace TyperShark2.Tests
{
    public class SharkManagerTest
    {
        [Fact]
        public void GivenOneLetterWordSinglePlayer_WhenPressThatLetter_SharkIsSolved()
        {
            // ARRANGE
            var gameState = new GameState();
            var sharkManager = new SharkManager(gameState, "a", 1, 1);

            // ACT
            sharkManager.OnKeyPress(null, "a");

            // ASSERT
            sharkManager.SharkDto.CorrectCharacters.ShouldBe(1);
            sharkManager.SharkDto.IsSolved.ShouldBeTrue();
        }

        [Fact]
        public void GivenTwoLetterWord_WhenUser1PressesGoodAndUser2PressesBadAndUser1PressesGood_ThenSharkIsSolved()
        {
            // ARRANGE
            var gameState = new GameState();
            var sharkManager = new SharkManager(gameState, "ab", 1, 1);

            // ACT
            sharkManager.OnKeyPress("Bob", "a");
            sharkManager.OnKeyPress("Sally", "z");
            sharkManager.OnKeyPress("Bob", "b");

            // ASSERT
            sharkManager.SharkDto.CorrectCharacters.ShouldBe(2);
            sharkManager.SharkDto.IsSolved.ShouldBeTrue();
        }
    }
}
