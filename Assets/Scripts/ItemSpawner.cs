using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject prefabPalomitas;
    public GameObject prefabRefresco;
    public Transform puntoDeAparicion; // Un objeto vacío donde quieres que aparezcan

    public void GenerarPalomitas()
    {
        Instantiate(prefabPalomitas, puntoDeAparicion.position, puntoDeAparicion.rotation);
    }

    public void GenerarRefresco()
    {
        Instantiate(prefabRefresco, puntoDeAparicion.position, puntoDeAparicion.rotation);
    }
}
