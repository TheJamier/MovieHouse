using System.Collections;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public float distance = 5f;
    public Transform holdPoint;     
    public GameObject UI_Mirando;   
    public GameObject UI_Cargando;  

    [Header("Configuración de Animación")]
    [Tooltip("Tiempo en segundos que tardan los brazos en estirarse antes de que el objeto aparezca o se recoja")]
    public float tiempoEsperaInteraccion = 0.5f;

    private Animator brazosAnim;
    private bool interactuando = false;

    void Start()
    {
        // Buscamos el Animator en los brazos del jugador automáticamente
        brazosAnim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // 1. Verificamos si ya tenemos algo en la mano usando tu misma lógica
        bool holdsSomething = holdPoint.childCount > 0;
        UI_Cargando.SetActive(holdsSomething);

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        RaycastHit hit;

        // 2. Control de la interfaz al mirar un objeto
        if (Physics.Raycast(ray, out hit, distance))
        {
            CineItem item = hit.collider.GetComponentInParent<CineItem>();

            if (item != null && !item.isHeld)
                UI_Mirando.SetActive(true);
            else
                UI_Mirando.SetActive(false);
        }
        else
        {
            UI_Mirando.SetActive(false);
        }

        // Clic izquierdo al aire (Manos Vacías) si no estás picando la interfaz
        if (Input.GetMouseButtonDown(0) && !holdsSomething && !interactuando)
        {
            if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                if (brazosAnim != null) brazosAnim.SetTrigger("HacerGrab"); 
            }
        }

        // 3. Lógica de Interacción (Tecla E)
        if (Input.GetKeyDown(KeyCode.E) && !interactuando)
        {
            if (holdsSomething)
            {
                // Si ya tenemos algo, usamos tu método Drop() original
                CineItem itemEnMano = holdPoint.GetComponentInChildren<CineItem>();
                if (itemEnMano != null) itemEnMano.Drop();
            }
            else if (Physics.Raycast(ray, out hit, distance))
            {
                CineItem itemAMirar = hit.collider.GetComponentInParent<CineItem>();
                
                if (itemAMirar != null && !itemAMirar.isHeld)
                {
                    // Iniciamos la animación y la espera para recoger del suelo
                    StartCoroutine(SecuenciaInteraccion(itemAMirar, null));
                }
            }
        }
    }

    // Llamado por los botones de la Taquilla
    public void ComprarDesdeTaquilla(GameObject prefabItem)
    {
        bool holdsSomething = holdPoint.childCount > 0;
        if (interactuando || holdsSomething) return;

        StartCoroutine(SecuenciaInteraccion(null, prefabItem));
    }

    IEnumerator SecuenciaInteraccion(CineItem itemDelSuelo, GameObject prefabDeTaquilla)
    {
        interactuando = true;
        UI_Mirando.SetActive(false); 

        // A) Disparar animación en tus brazos
        if (brazosAnim != null) brazosAnim.SetTrigger("HacerGrab");

        // B) Esperar a que la mano llegue a su destino
        yield return new WaitForSeconds(tiempoEsperaInteraccion);

        // C) Acoplar el objeto correspondiente
        if (itemDelSuelo != null)
        {
            // Caso Suelo: Recoge el objeto existente
            itemDelSuelo.RecogerEnMano(holdPoint);
        }
        else if (prefabDeTaquilla != null)
        {
            // Caso Taquilla: Instancia el nuevo prefab y lo acopla
            GameObject nuevoItem = Instantiate(prefabDeTaquilla, holdPoint.position, holdPoint.rotation);
            CineItem scriptItem = nuevoItem.GetComponent<CineItem>();
            if (scriptItem != null)
            {
                scriptItem.RecogerEnMano(holdPoint);
            }
        }

        interactuando = false;
    }
}