using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    public VideoPlayer videoPlayer;

    public void PlayVideo()
    {
        if (videoPlayer != null)
        {
            Debug.Log("Reproduciendo video...");
            videoPlayer.Play();
        }
        else
        {
            Debug.Log("No hay VideoPlayer asignado");
        }
    }
}
