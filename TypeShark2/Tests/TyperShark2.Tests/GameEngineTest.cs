using Shouldly;
using Xunit;

namespace TyperShark2.Tests
{
    public class GameEngineTest
    {
        [Fact]
        public void GivenStoppedGame_WhenToggleGameState_ThenGameIsStarted()
        {
            // ARRANGE
            var gameEngine = new TypeShark2.Shared.Services.GameEngine();
            var gameState = gameEngine.CreateGame();
            gameState.GameDto.IsStarted.ShouldBeFalse();

            // ACT
            using var gameStartTask = gameEngine.ToggleGameState(gameState);

            // ASSERT
            gameState.GameDto.IsStarted.ShouldBeTrue();
            gameEngine.Stop(gameState);
            gameStartTask.Wait();
        }
    }
}