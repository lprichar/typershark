using System;
using System.Threading.Tasks;
using TypeShark2.Client.Data;
using TypeShark2.Client.JsInterop;

namespace TypeShark2.Client.Pages
{
    public partial class GameComponent
    {
        public static Game CurrentGame;
        protected static string Message { get; set; }

        private async void OnKeyPress(object sender, string key)
        {
            await InvokeAsync(() =>
            {
                CurrentGame.OnKeyPress(key);
                this.StateHasChanged();
            });
        }

        protected override void OnInitialized()
        {
            InteropKeyPress.KeyPress += OnKeyPress;
            GameInit();
        }

        private void GameInit()
        {
            if (CurrentGame == null)
            {
                CurrentGame = new Game();
                Console.WriteLine("Starting a new game");
            }
            else
            {
                Console.WriteLine("Game already in progress");
            }
            CurrentGame.SharkAdded += SharkAdded;
            CurrentGame.GameOver += GameOver;
        }

        private void SharkAdded(object sender, EventArgs e)
        {
            InvokeAsync(this.StateHasChanged);
        }

        private void GameOver(object sender, EventArgs e)
        {
            InvokeAsync(() =>
            {
                Message = "Game over, congratulations you scored " + CurrentGame.Score;
                this.StateHasChanged();
            });
        }

        private void Test()
        {
            CurrentGame.Test();
        }

        public async Task ToggleGameState()
        {
            if (CurrentGame.IsStarted)
            {
                CurrentGame.Stop();
            }
            else
            {
                CurrentGame.Clear();
                Message = "";
                await CurrentGame.Start();
            }
        }
    }
}
