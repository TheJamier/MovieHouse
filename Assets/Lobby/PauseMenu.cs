using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject panelPausa;
    public GameObject preguntasPanel;

    private bool pausado = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausado)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }
    }

    public void Pausar()
    {
        panelPausa.SetActive(true);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        pausado = true;
    }

    public void Reanudar()
    {
        panelPausa.SetActive(false);
        preguntasPanel.SetActive(false);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pausado = false;
    }

    public void AbrirPreguntas()
    {
        panelPausa.SetActive(false);
        preguntasPanel.SetActive(true);
    }

    public void RegresarMenu()
    {
        preguntasPanel.SetActive(false);
        panelPausa.SetActive(true);
    }

    public void Salir()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}