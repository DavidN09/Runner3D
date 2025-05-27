using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Enum que representa las direcciones de entrada del jugador.
/// </summary>
public enum DireccionesInput
{
    Null,
    Arriba,
    Izquierda,
    Derecha,
    Abajo,
}

/// <summary>
/// Controlador principal del jugador.
/// Maneja el movimiento, salto, deslizamiento, cambio de carril y colisiones.
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float valorSalto = 15f;
    [SerializeField] private float gravedad = 20f;

    [Header("Carril")]
    [SerializeField] private float posicionCarrilIzquierdo = -3.1f;
    [SerializeField] private float posicionCarrilDerecho = 3.1f;

    /// <summary>
    /// Indica si el jugador está actualmente en el aire.
    /// </summary>
    public bool EstaSaltando { get; private set; }

    /// <summary>
    /// Indica si el jugador está deslizándose.
    /// </summary>
    public bool EstaDeslizando { get; private set; }

    private DireccionesInput direccionInput;
    private Coroutine coroutineDeslizar;
    private CharacterController characterController;
    private PlayerAnimaciones playerAnimaciones;
    private float posicionVertical;
    private int carrilActual;
    private Vector3 direccionDeseada;

    private float controllerRadio;
    private float controllerAltura;
    private float controllPosicionY;

    /// <summary>
    /// Inicializa componentes del jugador al despertar.
    /// </summary>
    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimaciones = GetComponent<PlayerAnimaciones>();
    }

    /// <summary>
    /// Inicializa valores al comenzar el juego.
    /// </summary>
    void Start()
    {
        controllerRadio = characterController.radius;
        controllerAltura = characterController.height;
        controllPosicionY = characterController.center.y;
    }


    /// <summary>
    /// Ejecuta la lógica principal de movimiento del jugador si el juego está activo.
    /// </summary>
    void Update()
    {
        if (GameManager.Instancia.EstadoActual == EstadosDelJuego.Inicio ||
            GameManager.Instancia.EstadoActual == EstadosDelJuego.GameOver)
        {
            return;
        }

        DetectarInput();
        ControlarCarriles();
        CalcularMovimientoVertical();
        MoverPersonaje();
    }

    /// <summary>
    /// Aplica el movimiento final del personaje basado en dirección deseada y posición vertical.
    /// </summary>
    private void MoverPersonaje()
    {
        Vector3 nuevaPos = new Vector3(direccionDeseada.x, y: posicionVertical, z: velocidadMovimiento);
        characterController.Move(motion: nuevaPos * Time.deltaTime);
    }

    /// <summary>
    /// Calcula el movimiento vertical (salto, caída, deslizamiento).
    /// </summary>
    private void CalcularMovimientoVertical()
    {
        if (characterController.isGrounded)
        {
            EstaSaltando = false;
            posicionVertical = 0f;

            if (!EstaDeslizando && !EstaSaltando)
            {
                playerAnimaciones.MostrarAnimacionCorrer();
            }

            if (direccionInput == DireccionesInput.Arriba)
            {
                posicionVertical = valorSalto;
                EstaSaltando = true;
                playerAnimaciones.MostrarAnimacionSaltar();

                if (coroutineDeslizar != null)
                {
                    StopCoroutine(coroutineDeslizar);
                    EstaDeslizando = false;
                    ModificarColliderDesllizar(false);
                }
            }

            if (direccionInput == DireccionesInput.Abajo)
            {
                if (EstaDeslizando) return;

                if (coroutineDeslizar != null)
                {
                    StopCoroutine(coroutineDeslizar);
                }

                DeslizarPersonaje();
            }
        }
        else
        {
            if (direccionInput == DireccionesInput.Abajo)
            {
                posicionVertical -= valorSalto;
                DeslizarPersonaje();
            }
        }

        posicionVertical -= gravedad * Time.deltaTime;
    }

    /// <summary>
    /// Controla el movimiento horizontal del jugador entre carriles.
    /// </summary>
    private void ControlarCarriles()
    {
        switch (carrilActual)
        {
            case -1:
                LogicaCarrilIzquierdo();
                break;
            case 0:
                logicaCarrilCentral();
                break;
            case 1:
                LogicaCarrilDerecho();
                break;
        }
    }

    /// <summary>
    /// Ajusta la posición del jugador hacia el carril central.
    /// </summary>
    private void logicaCarrilCentral()
    {
        if (transform.position.x > 0.1f)
        {
            MoverHorizontal(0f, Vector3.left);
        }
        else if (transform.position.x < 0.1f)
        {
            MoverHorizontal(0f, Vector3.right);
        }
        else
        {
            direccionDeseada = Vector3.zero;
        }
    }

    /// <summary>
    /// Mueve al jugador hacia el carril izquierdo.
    /// </summary>
    private void LogicaCarrilIzquierdo()
    {
        MoverHorizontal(posicionCarrilIzquierdo, Vector3.left);
    }

    /// <summary>
    /// Mueve al jugador hacia el carril derecho.
    /// </summary>
    private void LogicaCarrilDerecho()
    {
        MoverHorizontal(posicionCarrilDerecho, Vector3.right);
    }

    /// <summary>
    /// Lógica para interpolar el movimiento horizontal hacia una posición deseada.
    /// </summary>
    private void MoverHorizontal(float posicionX, Vector3 dirMovimiento)
    {
        float posicionHorizontal = Mathf.Abs(transform.position.x - posicionX);
        if (posicionHorizontal > 0.1f)
        {
            direccionDeseada = Vector3.Lerp(direccionDeseada, dirMovimiento * 20f, Time.deltaTime * 500f);
        }
        else
        {
            direccionDeseada = Vector3.zero;
            transform.position = new Vector3(posicionX, transform.position.y, transform.position.z);
        }
    }

    /// <summary>
    /// Inicia la corrutina para deslizamiento.
    /// </summary>
    private void DeslizarPersonaje()
    {
        coroutineDeslizar = StartCoroutine(CODeslizarPersonaje());
    }

    /// <summary>
    /// Corrutina que maneja el tiempo y efectos del deslizamiento.
    /// </summary>
    private IEnumerator CODeslizarPersonaje()
    {
        EstaDeslizando = true;
        playerAnimaciones.MostrarAnimacionDeslizar();
        ModificarColliderDesllizar(true);
        yield return new WaitForSeconds(2f);
        EstaDeslizando = false;
        ModificarColliderDesllizar(false);
    }

    /// <summary>
    /// Modifica el collider del personaje para adaptarse al deslizamiento.
    /// </summary>
    /// <param name="modificar">Si es true, aplica valores reducidos al collider.</param>
    private void ModificarColliderDesllizar(bool modificar)
    {
        if (modificar)
        {
            characterController.radius = 0.3f;
            characterController.height = 0.6f;
            characterController.center = new Vector3(0f, 0.35f, 0f);
        }
        else
        {
            characterController.radius = controllerRadio;
            characterController.height = controllerAltura;
            characterController.center = new Vector3(0f, controllPosicionY, 0f);
        }
    }

    /// <summary>
    /// Detecta el input del jugador y ajusta la dirección y carril actual.
    /// </summary>
    private void DetectarInput()
    {
        direccionInput = DireccionesInput.Null;

        if (Input.GetKeyDown(KeyCode.A))
        {
            direccionInput = DireccionesInput.Izquierda;
            carrilActual--;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            direccionInput = DireccionesInput.Derecha;
            carrilActual++;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            direccionInput = DireccionesInput.Abajo;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            direccionInput = DireccionesInput.Arriba;
        }

        carrilActual = Mathf.Clamp(carrilActual, -1, 1);
    }

    /// <summary>
    /// Detecta colisiones con objetos usando CharacterController.
    /// </summary>
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Obstaculo"))
        {
            if (GameManager.Instancia.EstadoActual == EstadosDelJuego.GameOver)
            {
                return;
            }

            playerAnimaciones.MostrarAnimacionColision();
            GameManager.Instancia.CambiarEstado(EstadosDelJuego.GameOver);
        }
    }

    /// <summary>
    /// Detecta si el jugador entra en un trigger, como una zona de salto forzado.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Salto"))
        {
            posicionVertical = valorSalto;
            EstaSaltando = true;
            playerAnimaciones.MostrarAnimacionSaltar();

            if (coroutineDeslizar != null)
            {
                StopCoroutine(coroutineDeslizar);
                EstaDeslizando = false;
                ModificarColliderDesllizar(false);
            }
        }
    }
}
