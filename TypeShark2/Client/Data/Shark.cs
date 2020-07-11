using System;
using System.Timers;

namespace TypeShark2.Client.Data
{
    public class Shark : IDisposable
    {
        private readonly Game _game;

        public Shark(Game game, string word, int height, int secondsToSolve)
        {
            _game = game;
            Word = word;
            Height = height;
            SecondsToSolve = secondsToSolve;
            _timer = new Timer(SecondsToSolve * 1000);
            _timer.Elapsed += TimeoutElapsed;
        }

        public void StartTimer()
        {
            _timer.Start();
        }

        public event EventHandler<Game> OnSolved;
        public event EventHandler<Game> OnFailed;

        public string CharsDone => Word.Substring(0, _correctCharacters);

        public string CharsToGo => Word.Substring(_correctCharacters);

        public bool IsSolved => _correctCharacters == Word.Length;

        public string Word { get; set; }
        public int Height { get; set; }
        public int SecondsToSolve { get; set; }

        private int _correctCharacters = 0;
        private Timer _timer;

        public void OnKeyPress(string key)
        {
            var isCorrect = Word[_correctCharacters].ToString() == key;
            if (isCorrect)
            {
                _correctCharacters++;
            }
            else
            {
                _correctCharacters = 0;
            }
            if (IsSolved)
            {
                OnSolved?.Invoke(this, _game);
            }
        }

        private void TimeoutElapsed(object sender, ElapsedEventArgs e)
        {
            if (!IsSolved)
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