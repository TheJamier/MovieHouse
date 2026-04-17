using UnityEngine;

public class ConvaiMovieInterceptor : MonoBehaviour
{
    public MoviePlayer moviePlayer;

    public void HandleText(string text)
    {
        if (string.IsNullOrEmpty(text)) return;

        Debug.Log("Convai TEXT: " + text);

        text = text.ToLower();

        if (text.Contains("play") || text.Contains("reproduce"))
        {
            string movie = ExtractMovieName(text);
            moviePlayer.PlayMovie(movie);
        }
    }

    private string ExtractMovieName(string text)
    {
        return text
            .Replace("play", "")
            .Replace("reproduce", "")
            .Replace("movie", "")
            .Trim();
    }
}
