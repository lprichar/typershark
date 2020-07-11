namespace TypeShark2.Shared.Dtos
{
    public class SharkDto
    {
        public string Word { get; set; }
        public int Height { get; set; }
        public int SecondsToSolve { get; set; }

        public string CharsDone => Word.Substring(0, CorrectCharacters);
        public string CharsToGo => Word.Substring(CorrectCharacters);

        public int CorrectCharacters { get; set; }
        public bool IsSolved => CorrectCharacters == Word.Length;
    }
}
