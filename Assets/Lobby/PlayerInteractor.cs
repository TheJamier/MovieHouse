using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public float distance = 5f;
    public Transform holdPoint;     // Arrastra aquí tu objeto HoldPoint
    public GameObject UI_Mirando;   // Arrastra el texto de "Presiona E para recoger"
    public GameObject UI_Cargando;  // Arrastra el texto de "Clic para consumir"

    void Update()
    {
        // 1. Verificamos si ya tenemos algo en la mano
        bool holdsSomething = holdPoint.childCount > 0;

        // Gestionar la UI de "Cargando"
        UI_Cargando.SetActive(holdsSomething);

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distance))
        {
            // Buscamos el script en el objeto golpeado O en sus padres
            CineItem item = hit.collider.GetComponentInParent<CineItem>();

            if (item != null)
                UI_Mirando.SetActive(true);
            else
                UI_Mirando.SetActive(false);
        }
        else
        {
            UI_Mirando.SetActive(false);
        }

        // 3. Lógica de Interacción (Tecla E)
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (holdsSomething)
            {
                // Si ya tenemos algo, le pedimos que se suelte
                holdPoint.GetComponentInChildren<CineItem>().Interact();
            }
            else if (Physics.Raycast(ray, out hit, distance))
            {
                // Si no tenemos nada, intentamos recoger lo que miramos
                hit.collider.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
