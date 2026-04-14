using UnityEngine;
using UnityEngine.Video;

[System.Serializable]
public class Movie
{
    public string nombre;
    public VideoClip clip;
}

public class MovieDatabase : MonoBehaviour
{
    public Movie[] peliculas;

    public VideoClip GetMovie(string nombre)
    {
        foreach (var m in peliculas)
        {
            if (m.nombre.ToLower() == nombre.ToLower())
                return m.clip;
        }

        return null;
    }
}
