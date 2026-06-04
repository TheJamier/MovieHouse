using UnityEngine;

public class CinemaVending : MonoBehaviour
{
    public GameObject prefabPalomitas;
    public GameObject prefabRefresco;

    // Necesitamos una referencia al script del jugador para saber si ya tiene algo
    private PlayerInteractor playerInteractor;

    void Start()
    {
        // Buscamos al jugador por su tag (asegúrate de que tu Player tenga el tag "Player")
        playerInteractor = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInteractor>();
    }

    public void ComprarItem(string tipo)
    {
        // 1. Verificamos si el jugador ya tiene algo en la mano
        if (playerInteractor.holdPoint.childCount > 0)
        {
            Debug.Log("Ya tienes las manos ocupadas");
            return;
        }

        GameObject objetoAInstanciar = (tipo == "palomitas") ? prefabPalomitas : prefabRefresco;

        // 2. Creamos el objeto directamente en la posición del HoldPoint
        GameObject nuevoItem = Instantiate(objetoAInstanciar, playerInteractor.holdPoint.position, playerInteractor.holdPoint.rotation);

        // 3. Le decimos al script del objeto que se autoejecute como si lo hubieran recogido
        CineItem scriptItem = nuevoItem.GetComponent<CineItem>();
        if (scriptItem != null)
        {
            scriptItem.Interact(); // Esto activará el PickUp() y lo pegará a la mano
        }
    }
}
