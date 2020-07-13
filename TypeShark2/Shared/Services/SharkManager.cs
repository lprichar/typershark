using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using TypeShark2.Shared.Dtos;

namespace TypeShark2.Shared.Services
{
    /// <summary>
    /// Manages a single shark (SharkDto) via a timer.  This isn't technically a service since it's stateful.
    /// </summary>
    public class SharkManager : IDisposable
    {
        public event EventHandler<GameState> OnSolved;
        public event EventHandler<GameState> OnFailed;

        public readonly SharkDto SharkDto;

        private readonly GameState _game;
        private Timer _timer;
        private Dictionary<string, int> CorrectCharactersByPlayer = new Dictionary<string, int>();

        public SharkManager(GameState game, string word, int height, int secondsToSolve)
        {
            _timer = new Timer(secondsToSolve * 1000);
            _timer.Elapsed += TimeoutElapsed;

            _game = game;
            SharkDto = new SharkDto()
            {
                Word = word,
                Height = height,
                SecondsToSolve = secondsToSolve,
                CorrectCharacters = 0,
            };
        }

        public void StartTimer()
        {
            _timer.Start();
        }

        public SharkDto OnKeyPress(string user, string key)
        {
            user ??= "";
            var oldCorrectCharacters = GetCorrectCharactersForUser(user);

            var newCorrectCharacters = GetNewCorrectCharacters(key, oldCorrectCharacters);

            CorrectCharactersByPlayer[user] = newCorrectCharacters;

            var newGroupCorrectCharacters = CorrectCharactersByPlayer.Values.Max();
            var oldGroupCorrectCharacters = SharkDto.CorrectCharacters;
            SharkDto.CorrectCharacters = newGroupCorrectCharacters;
            var sharkChanged = oldGroupCorrectCharacters != newGroupCorrectCharacters;

            if (SharkDto.IsSolved)
            {
                OnSolved?.Invoke(this, _game);
            }

            return sharkChanged ? SharkDto : null;
        }

        private int GetNewCorrectCharacters(string key, int oldCorrectCharacters)
        {
            var isCorrect = SharkDto.Word[oldCorrectCharacters].ToString() == key;
            return isCorrect ? SharkDto.CorrectCharacters + 1 : 0;
        }

        private int GetCorrectCharactersForUser(string user)
        {
            if (user == null)
            {
                return SharkDto.CorrectCharacters;
            }

            if (CorrectCharactersByPlayer.TryGetValue(user, out int correctCharacters))
            {
                return correctCharacters;
            }

            return 0;
        }

        private void TimeoutElapsed(object sender, ElapsedEventArgs e)
        {
            if (!SharkDto.IsSolved)
            {
                OnFailed?.Invoke(this, _game);
            }
        }

        public void Dispose()
        {
            if (_timer != null)
            {
                _timer.Elapsed -= TimeoutElapsed;
                _timer.Dispose();
                _timer = null;
            }
        }
    }
}