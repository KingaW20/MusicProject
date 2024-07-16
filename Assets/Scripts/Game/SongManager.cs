using System;

namespace Scripts
{
    public static class SongManager
    {
        public static SongState State;

        public static void Setup()
        {
            State = new SongState();
        }

        public static bool IsAnswerCorrect(string answer)
        {
            string[] answerWords = answer.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            answer = string.Join(" ", answerWords);
            return answer.ToLower() == GetCurrentSong().Answer.ToLower();
        }

        public static string GetCategoryNameById(int id)
        {
            if (GameManager.State.SelectedCategories.Count < Constants.CATEGORY_NUMBER)
                return "";
            return GameManager.State.SelectedCategories[id].Name;
        }

        public static string GetCurrentCatSongTitleById(int id)
        {
            if (GameManager.State.SelectedCategories.Count < Constants.CATEGORY_NUMBER)
                return "";
            return GameManager.State.SelectedCategories[State.CurrentCategoryId].SelectedSongs[id].Title;
        }

        public static Song GetCurrentSong()
        {
            if (GameManager.State.SelectedCategories.Count < Constants.CATEGORY_NUMBER)
                return null;
            if (State.CurrentSongId < Constants.SONG_NUMBER)
                return GameManager.State.SelectedCategories[State.CurrentCategoryId].SelectedSongs[State.CurrentSongId];
            else
                return GameManager.State.HitSong;
        }
        
        public static string GetCurrentLine()
        {
            var line = GetCurrentSong().Lines[State.CurrentLineId];

            if (line.StartTime <= State.CurrentTime && State.CurrentTime < line.EndTime)
                return line.Text;
            else if (line.EndTime <= State.CurrentTime && State.CurrentLineId < GetCurrentSong().AnswerLineId - 1 &&
                GetCurrentSong().Lines[State.CurrentLineId + 1].StartTime <= State.CurrentTime)
            {
                State.CurrentLineId++;
                return GetCurrentSong().Lines[State.CurrentLineId].Text;
            }
            else if (line.EndTime <= State.CurrentTime && State.CurrentLineId == GetCurrentSong().AnswerLineId - 1)
                return line.Text;

            return "...";
        }

        public static string GetNextLine()
        {
            var currentLine = GetCurrentSong().Lines[State.CurrentLineId];
            if (currentLine.StartTime >= State.CurrentTime && State.CurrentLineId < GetCurrentSong().AnswerLineId)
                return currentLine.Text.Replace("\n", " ");

            var nextLineId = State.CurrentLineId + 1;
            if (nextLineId < GetCurrentSong().AnswerLineId)
            {
                var nextLine = GetCurrentSong().Lines[nextLineId];

                if (nextLine.StartTime >= State.CurrentTime)
                    return nextLine.Text.Replace("\n", " ");
                else
                    return nextLineId < GetCurrentSong().AnswerLineId ? GetCurrentSong().Lines[nextLineId + 1].Text.Replace("\n", " ") : "...";
            }

            return "...";
        }

        public static string GetWordFromCurrentSongAnswerById(int wordId)
        {
            string answer = GetCurrentSong().Answer;
            string[] answerWords = answer.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return answerWords[wordId].ToUpper();
        }

        public static bool IsSongEnded()
        {
            if (GetCurrentSong() == null)
                return false;
            return State.CurrentTime >= GetCurrentSong().StopTime;
        }
    }
}
