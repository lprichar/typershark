using Microsoft.AspNetCore.Components;
using TypeShark2.Client.Data;

namespace TypeShark2.Client.Pages
{
    public partial class Index
    {
        [Inject]
        public IGameContext Context { get; set; }

        public string TempName { get; set; }

        public void SetName()
        {
            Context.PlayerName = TempName;
        }
    }
}
