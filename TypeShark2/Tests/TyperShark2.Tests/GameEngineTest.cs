using Moq;
using Shouldly;
using System.Threading.Tasks;
using TypeShark2.Shared.Services;
using Xunit;

namespace TyperShark2.Tests
{
    public class GameEngineTest
    {
        [Fact]
        public async Task GivenStoppedGame_WhenToggleGameState_ThenGameIsStarted()
        {
            // ARRANGE
            var gameEngineEventHandler = new Mock<IGameEngineEventHandler>();
            var gameEngine = new GameEngine(gameEngineEventHandler.Object);
            var gameState = gameEngine.CreateGame();
            gameState.GameDto.IsStarted.ShouldBeFalse();

            // ACT
            using var gameStartTask = gameEngine.ToggleGameState(gameState);

            // ASSERT
            gameState.GameDto.IsStarted.ShouldBeTrue();
            await gameEngine.ToggleGameState(gameState);
            gameState.GameDto.IsStarted.ShouldBeFalse();
            gameStartTask.Wait();
        }

        [Fact]
        public async Task GivenStoppedGame_WhenToggleGameState_ThenSharkIsAdded()
        {
            // ARRANGE
            var gameEngineEventHandler = new Mock<IGameEngineEventHandler>();
            var gameEngine = new GameEngine(gameEngineEventHandler.Object);
            var gameState = gameEngine.CreateGame();
            gameState.GameDto.IsStarted.ShouldBeFalse();
            gameEngineEventHandler.Setup(i => i.SharkAdded(It.IsAny<SharkChangedEventArgs>()));

            // ACT
            using var gameStartTask = gameEngine.ToggleGameState(gameState);

            // ASSERT
            gameStartTask.Wait();
            gameEngineEventHandler.VerifyAll();
        }
    }
}