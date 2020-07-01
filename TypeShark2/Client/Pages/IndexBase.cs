using Microsoft.AspNetCore.Components;
using TypeShark2.Client.Data;

namespace TypeShark2.Client.Pages
{
    public class IndexBase : ComponentBase
    {
        public static GameContext GameContext { get; set; } = new GameContext();

        public string TempName { get; set; }

        public void SetName()
        {
            GameContext.PlayerName = TempName;
        }
    }
}
