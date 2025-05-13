using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum DireccionesInput
{   
    Null,
    Arriba,
    Izquierda,
    Derecha,
    Abajo,

}


public class PlayerController : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float valorSalto = 15f;
    [SerializeField] private float gravedad = 20f;

    [Header("Carril")]
    [SerializeField] private float posicionCarrilIzquierdo = -3.1f; 
    [SerializeField] private float posicionCarrilDerecho = 3.1f;

    public bool EstaSaltando { get;private set; }
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



    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimaciones = GetComponent<PlayerAnimaciones>();
    }
    // Start is called before the first frame update
    void Start()
    {
        controllerRadio = characterController.radius;
        controllerAltura = characterController.height;
        controllPosicionY = characterController.center.y;

    }

    // Update is called once per frame
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

    private void MoverPersonaje()
    {
        Vector3 nuevaPos = new Vector3(direccionDeseada.x, y: posicionVertical, z: velocidadMovimiento);
        characterController.Move(motion: nuevaPos * Time.deltaTime);

    }

    private void CalcularMovimientoVertical()
    {
        if (characterController.isGrounded)
        {
            EstaSaltando = false; 
            posicionVertical = 0f;

            if (EstaDeslizando == false && EstaSaltando == false)
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

            if(direccionInput == DireccionesInput.Abajo)
            {
                if (EstaDeslizando)
                {
                    return;
                }

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

    private void LogicaCarrilIzquierdo()
    {
        MoverHorizontal(posicionCarrilIzquierdo, Vector3.left);
    }

    private void LogicaCarrilDerecho()
    {
        MoverHorizontal(posicionCarrilDerecho, Vector3.right);
    }


    private void MoverHorizontal(float posicionX, Vector3 dirMovimiento) 
    {
        float posicionHorizontal = Mathf.Abs(transform.position.x - posicionX);
        if(posicionHorizontal > 0.1f) 
        {
            direccionDeseada = Vector3.Lerp(direccionDeseada,dirMovimiento * 20f, Time.deltaTime * 500f);
        
        }

        else 
        {
            direccionDeseada = Vector3.zero;
            transform.position = new Vector3(posicionX, transform.position.y, transform.position.z);
        
        }
           
    }

    private void DeslizarPersonaje()
    {
      coroutineDeslizar = StartCoroutine(CODeslizarPersonaje());
    }

    private IEnumerator CODeslizarPersonaje()
    {
        EstaDeslizando = true;
        playerAnimaciones.MostrarAnimacionDeslizar();
        ModificarColliderDesllizar(true);
        yield return new WaitForSeconds(2f);
        EstaDeslizando = false;
        ModificarColliderDesllizar(false);
             
    }

    private void ModificarColliderDesllizar(bool modificar)
    {
        if (modificar)
        {
            //modificar collider
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
        else if (Input.GetKeyDown(KeyCode.W))
        {
            direccionInput = DireccionesInput.Arriba;

        
        }

            carrilActual = Mathf.Clamp(carrilActual, -1, 1);
       
    }

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
