using UnityEngine;
using UnityEngine.Video;
using System.Collections;
using System.Collections.Generic;

public class MoviePlayer : MonoBehaviour
{
    public MovieDatabase database;
    public VideoPlayer videoPlayer;

    public float delayBeforePlay = 10f;

    // Luces del cine
    public List<Light> cinemaLights;
    public float dimmedIntensity = 20f;
    public float dimSpeed = 2f;

    private Coroutine currentCoroutine;

    // Guardar intensidades originales
    private Dictionary<Light, float> originalIntensities = new Dictionary<Light, float>();

    void Start()
    {
        // Guardar intensidades originales
        foreach (Light light in cinemaLights)
        {
            if (light != null && !originalIntensities.ContainsKey(light))
                originalIntensities[light] = light.intensity;
        }

        // Detectar cuando termina el video
        if (videoPlayer != null)
            videoPlayer.loopPointReached += OnVideoFinished;
    }

    // ESTE ES EL MÉTODO QUE USARÁ CONVAI / INTERACCIÓN
    public void PlayMovie(string movieName)
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(PlayMovieRoutine(movieName));
    }

    private IEnumerator PlayMovieRoutine(string movieName)
    {
        Debug.Log("Película seleccionada: " + movieName);

        // Espera antes de iniciar
        yield return new WaitForSeconds(delayBeforePlay);

        // Bajar luces (en paralelo)
        StartCoroutine(DimLights());

        // Buscar y reproducir video
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

    // BAJAR LUCES
    private IEnumerator DimLights()
    {
        float duration = 2f;
        float time = 0f;

        Dictionary<Light, float> initialIntensities = new Dictionary<Light, float>();

        foreach (Light light in cinemaLights)
        {
            if (light != null)
                initialIntensities[light] = light.intensity;
        }

        while (time < duration)
        {
            time += Time.deltaTime;

            foreach (Light light in cinemaLights)
            {
                if (light != null && initialIntensities.ContainsKey(light))
                {
                    float start = initialIntensities[light];
                    light.intensity = Mathf.Lerp(start, dimmedIntensity, time / duration);
                }
            }

            yield return null;
        }
    }

    // CUANDO TERMINA EL VIDEO
    private void OnVideoFinished(VideoPlayer vp)
    {
        Debug.Log("Película terminada, subiendo luces...");
        StartCoroutine(RestoreLights());
    }

    // SUBIR LUCES
    private IEnumerator RestoreLights()
    {
        float duration = 2f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            foreach (Light light in cinemaLights)
            {
                if (light != null && originalIntensities.ContainsKey(light))
                {
                    float target = originalIntensities[light];
                    light.intensity = Mathf.Lerp(light.intensity, target, time / duration);
                }
            }

            yield return null;
        }
    }
}