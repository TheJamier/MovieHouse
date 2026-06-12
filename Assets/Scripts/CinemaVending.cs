using UnityEngine;

public class CinemaVending : MonoBehaviour
{
    public GameObject prefabPalomitas;
    public GameObject prefabRefresco;
    
    private PlayerInteractor playerInteractor;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            playerInteractor = playerObj.GetComponent<PlayerInteractor>();
    }

    // Conectado al OnClick() del botón del Canvas de la taquilla
    public void ComprarItem(string tipo)
    {
        if (playerInteractor == null) return;

        GameObject prefabAInstanciar = (tipo == "palomitas") ? prefabPalomitas : prefabRefresco;

        if (prefabAInstanciar != null)
        {
            // Le manda el molde al Player para que haga la animación de "Grab" antes de que aparezca
            playerInteractor.ComprarDesdeTaquilla(prefabAInstanciar);
        }
    }
}