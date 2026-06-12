using UnityEngine;

public class CineItem : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;
    public bool isHeld = false;
    public Vector3 ajusteEnMano; // Esto aparecerá en el Inspector

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void Interact()
    {
        // Nota: El PlayerInteractor ahora llamará directamente a RecogerEnMano para meter la animación.
        // Dejamos esto por si necesitas suelta/drop manual desde la E.
        if (isHeld) Drop();
    }

    // Este método lo llama el PlayerInteractor cuando la mano llega al objeto
    public void RecogerEnMano(Transform holdPoint)
    {
        if (isHeld) return;

        isHeld = true;
        rb.isKinematic = true;
        col.enabled = false;

        transform.SetParent(holdPoint);
        
        // Usamos tu ajuste personalizado y rotación base
        transform.localPosition = ajusteEnMano;
        transform.localEulerAngles = new Vector3(0, 0, 0);
    }

    public void Drop()
    {
        isHeld = false;
        transform.SetParent(null);
        rb.isKinematic = false;
        col.enabled = true;
        
        // Tu pequeño empujón hacia adelante al soltar
        rb.AddForce(Camera.main.transform.forward * 2f, ForceMode.Impulse);
    }

    void Update()
    {
        // Consumir solo si está en la mano
        if (isHeld && Input.GetMouseButtonDown(0))
        {
            Consume();
        }
    }

    void Consume()
    {
        Destroy(gameObject);
    }
}