using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class NPCSeguirJugador : MonoBehaviour
{
    [Header("Referencias")]
    public Transform jugador;

    [Header("Configuración")]
    public float distanciaMinima = 2f;
    public KeyCode teclaSeguir = KeyCode.F;

    private NavMeshAgent agente;
    private Animator animator;

    private bool seguirJugador = false;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agente.stoppingDistance = distanciaMinima;
        agente.isStopped = true;

        animator.SetFloat("Speed", 0f);
    }

    void Update()
    {
        if (jugador == null || !agente.isOnNavMesh) return;

        if (Input.GetKeyDown(teclaSeguir))
        {
            seguirJugador = !seguirJugador;
        }

        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (seguirJugador && distancia > distanciaMinima)
        {
            agente.isStopped = false;
            agente.SetDestination(jugador.position);
        }
        else
        {
            agente.isStopped = true;
        }

        float velocidad = agente.velocity.magnitude;

        if (agente.isStopped || velocidad < 0.1f)
        {
            animator.SetFloat("Speed", 0f);
        }
        else
        {
            animator.SetFloat("Speed", velocidad);
        }
    }
}
