using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class MoviePlayer : MonoBehaviour
{
    public MovieDatabase database;
    public VideoPlayer videoPlayer;

    public float delayBeforePlay = 10f;

    private Coroutine currentCoroutine;

    // ESTE ES EL MÉTODO QUE USARÁ CONVAI
    public void PlayMovie(string movieName)
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(PlayMovieRoutine(movieName));
    }

    private IEnumerator PlayMovieRoutine(string movieName)
    {
        Debug.Log("Película seleccionada: " + movieName);

        yield return new WaitForSeconds(delayBeforePlay);

        VideoClip clip = database.GetMovie(movieName);

        if (clip != null)
        {
            videoPlayer.clip = clip;
            videoPlayer.Play();
            Debug.Log("Reproduciendo: " + movieName);
        }
        else
        {
            Debug.LogError("No se encontró la película: " + movieName);
        }
    }
}
