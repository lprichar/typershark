using System;
using System.Timers;

namespace TypeShark2.Client.Data
{
    public class Shark : IDisposable
    {
        public Shark(string word, int height, int secondsToSolve)
        {
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

        public event EventHandler OnSolved;
        public event EventHandler OnFailed;

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
                OnSolved?.Invoke(this, EventArgs.Empty);
            }
        }

        private void TimeoutElapsed(object sender, ElapsedEventArgs e)
        {
            if (!IsSolved)
            {
                OnFailed?.Invoke(this, EventArgs.Empty);
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