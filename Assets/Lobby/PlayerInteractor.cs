using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    public float distance = 5f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, distance))
            {
                Debug.Log("Golpeaste: " + hit.collider.name);
                hit.collider.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}