using UnityEngine;

public class InteractableMovie : MonoBehaviour
{
    public string movieName;
    public MoviePlayer moviePlayer;

    public void Interact()
    {
        Debug.Log("Seleccionaste: " + movieName);
        moviePlayer.PlayMovie(movieName);
    }
}