using System;
using System.Timers;
using TypeShark2.Shared.Dtos;

namespace TypeShark2.Shared.Services
{
    /// <summary>
    /// Manages a single shark (SharkDto) via a timer.  This isn't technically a service since it's stateful.
    /// </summary>
    public class SharkManager : IDisposable
    {
        private readonly GameState _game;
        public readonly SharkDto SharkDto;
        private Timer _timer;

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

        public event EventHandler<GameState> OnSolved;
        public event EventHandler<GameState> OnFailed;

        public void OnKeyPress(string key)
        {
            var isCorrect = SharkDto.Word[SharkDto.CorrectCharacters].ToString() == key;
            if (isCorrect)
            {
                SharkDto.CorrectCharacters++;
            }
            else
            {
                SharkDto.CorrectCharacters = 0;
            }
            if (SharkDto.IsSolved)
            {
                OnSolved?.Invoke(this, _game);
            }
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