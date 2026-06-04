using UnityEngine;

public class CineItem : MonoBehaviour
{
    private Rigidbody rb;
    private Collider col;
    private bool isHeld = false;
    public Vector3 ajusteEnMano; // Esto aparecerá en el Inspector

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void Interact()
    {
        if (!isHeld) PickUp();
        else Drop();
    }

    void PickUp()
    {
        // Buscamos el HoldPoint en el jugador
        Transform holdPoint = GameObject.Find("HoldPoint").transform;

        // En lugar de Vector3.zero, usamos nuestro ajuste
        transform.localPosition = ajusteEnMano;

        isHeld = true;
        rb.isKinematic = true;
        col.enabled = false;

        transform.SetParent(holdPoint);
        // transform.localPosition = Vector3.zero;
        transform.localPosition = ajusteEnMano;
        transform.localEulerAngles = new Vector3(-90, 0, 0);
    }

    void Drop()
    {
        isHeld = false;
        transform.SetParent(null);
        rb.isKinematic = false;
        col.enabled = true;
        // Un pequeño empujón hacia adelante al soltar
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
        // Aquí podrías añadir un sonido de "crunch" para las palomitas
        Destroy(gameObject);
    }
}
