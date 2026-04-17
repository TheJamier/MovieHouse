using UnityEngine;
using System.Collections;

public class MovimientosClaw : MonoBehaviour
{
    public float alturaMaxima;
    public float alturaMinima;
    public float correctorSeparacionTubos;

    public GameObject Tubos;
    public GameObject Motor;
    public GameObject Gancho;

    [SerializeField] Animator animatorClaw;

    public float speed;
    public float speed_Claw;

    Rigidbody rigidBody;

    public Transform AlturaGancho;

    public Transform limitadorLeft;
    public Transform limitadorRight;
    public Transform limitadorFront;
    public Transform limitadorBack;

    public bool PuedeControlarse;
    bool SoltarPremio;

    public bool[] LlegadoALaCesta;

    bool dentroDeLaCesda;
    bool bajarGanchoYSoltarPremio;
    bool subirGanchoDeLaCesta;

    // INPUT
    float inputX;
    float inputZ;
    bool inputBajar;

    void Start()
    {
        AbrirClaw();
        rigidBody = GetComponent<Rigidbody>();
    
        // Inicializar array
        LlegadoALaCesta = new bool[3];

        PuedeControlarse = true;
        SoltarPremio = false;

        // Buscar animator si no está asignado
        if (animatorClaw == null)
        {
            animatorClaw = GetComponentInChildren<Animator>();
        }

        if (animatorClaw == null)
        {
            Debug.LogError("Animator NO encontrado");
        }
    }

    void Update()
    {
        // Leer input aquí (correcto en Unity 6)
        inputX = 0;
        inputZ = 0;
        inputBajar = false;

        if (Input.GetKey(KeyCode.A)) inputX = -1;
        if (Input.GetKey(KeyCode.D)) inputX = 1;

        if (Input.GetKey(KeyCode.W)) inputZ = 1;
        if (Input.GetKey(KeyCode.S)) inputZ = -1;

        if (Input.GetKey(KeyCode.Space)) inputBajar = true;
    }

    void FixedUpdate()
    {
        // Validar referencias para evitar crash
        if (Motor == null || Tubos == null || AlturaGancho == null) return;

        // Sincronizar piezas
        Motor.transform.position = new Vector3(transform.position.x, Motor.transform.position.y, transform.position.z);
        Tubos.transform.position = new Vector3(Tubos.transform.position.x, Tubos.transform.position.y, Motor.transform.position.z + correctorSeparacionTubos);

        // SOLTAR PREMIO (movimiento automático)
        if (SoltarPremio)
        {
            if (AlturaGancho.position.y <= alturaMaxima)
                transform.Translate(0, speed_Claw * Time.deltaTime, 0);
            else
                LlegadoALaCesta[0] = true;

            if (transform.position.x >= limitadorLeft.position.x + 0.5f)
                transform.Translate(-speed * Time.deltaTime, 0, 0);
            else
                LlegadoALaCesta[1] = true;

            if (transform.position.z >= limitadorFront.position.z + 0.5f)
                transform.Translate(0, 0, -speed * Time.deltaTime);
            else
                LlegadoALaCesta[2] = true;

            if (LlegadoALaCesta[0] && LlegadoALaCesta[1] && LlegadoALaCesta[2])
            {
                dentroDeLaCesda = false;
                StartCoroutine(SoltarPremioEnLaCesta(1.5f));
            }
        }

        // BAJAR PARA SOLTAR
        if (bajarGanchoYSoltarPremio)
        {
            if (AlturaGancho.position.y > alturaMinima)
                transform.Translate(0, -speed_Claw * Time.deltaTime, 0);
            else
            {
                StartCoroutine(AbrirClawEnlaCesta(1.0f));
                bajarGanchoYSoltarPremio = false;
            }
        }

        // SUBIR DESPUÉS
        if (subirGanchoDeLaCesta)
        {
            if (AlturaGancho.position.y <= alturaMaxima)
                transform.Translate(0, speed_Claw * Time.deltaTime, 0);
            else
            {
                PuedeControlarse = true;
                subirGanchoDeLaCesta = false;
            }
        }

        // CONTROL DEL JUGADOR
        if (PuedeControlarse)
        {
            if (transform.position.x > limitadorLeft.position.x && inputX < 0)
                transform.Translate(-speed * Time.deltaTime, 0, 0);

            if (transform.position.x < limitadorRight.position.x && inputX > 0)
                transform.Translate(speed * Time.deltaTime, 0, 0);

            if (transform.position.z < limitadorBack.position.z && inputZ > 0)
                transform.Translate(0, 0, speed * Time.deltaTime);

            if (transform.position.z > limitadorFront.position.z && inputZ < 0)
                transform.Translate(0, 0, -speed * Time.deltaTime);

            if (AlturaGancho.position.y > alturaMinima && inputBajar)
                transform.Translate(0, -speed_Claw * Time.deltaTime, 0);
            else if (AlturaGancho.position.y <= alturaMinima)
            {
                StartCoroutine(CerrarClaw(2.0f));
                PuedeControlarse = false;
            }
        }
    }

    IEnumerator CerrarClaw(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        CerrarClawAnim();

        yield return new WaitForSeconds(waitTime);

        SoltarPremio = true;
    }

    IEnumerator SoltarPremioEnLaCesta(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        bajarGanchoYSoltarPremio = true;
        SoltarPremio = false;
    }

    IEnumerator AbrirClawEnlaCesta(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        AbrirClaw();

        yield return new WaitForSeconds(waitTime);

        subirGanchoDeLaCesta = true;

        LlegadoALaCesta[0] = false;
        LlegadoALaCesta[1] = false;
        LlegadoALaCesta[2] = false;
    }

    public void AbrirClaw()
    {
        if (animatorClaw == null) return;

        animatorClaw.SetBool("Abrir", true);
        animatorClaw.SetBool("Cerrar", false);
    }

    public void CerrarClawAnim()
    {
        if (animatorClaw == null) return;

        animatorClaw.SetBool("Abrir", false);
        animatorClaw.SetBool("Cerrar", true);
    }
}