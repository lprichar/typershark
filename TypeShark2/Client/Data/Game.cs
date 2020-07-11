using System;
using System.Collections.Generic;
using System.Linq;

namespace TypeShark2.Client.Data
{
    public class Game
    {
        public int Score { get; set; }
        public List<Shark> Sharks = new List<Shark>();
        public bool IsStarted { get; set; } = false;
        public bool IsEasy { get; set; }

        DateTime? _lastKeypress = null;

        public void OnKeyPress(string key)
        {
            var durationSinceLastKeypress = DateTime.UtcNow - _lastKeypress;
            bool isDuplicateKeystroke = durationSinceLastKeypress != null && durationSinceLastKeypress.Value.TotalMilliseconds < 10;
            if (isDuplicateKeystroke)
            {
                return;
            }
            _lastKeypress = DateTime.UtcNow;

            Sharks
                .Where(i => !i.IsSolved)
                .ToList()
                .ForEach(s => s.OnKeyPress(key));
        }

    }
}