using TMPro;
using UnityEngine;
using UnityEngine.Audio;

public class PlayerController : MonoBehaviour

{
    // limites y variables de movimiento   
    private float horizontalInput;
    private float verticalInput;
    private float limiteHorizontal = 7.0f;
    private float limiteSuperior = 10.0f;
    private float limiteInferior = 1.0f;
    private float anguloInclinacion = 45f;
    private Rigidbody rb;

    public AudioSource audioSourceGetGasolina;
    public AudioSource audioSourceEnemigo;
    public AudioSource audioSourceArmaduraBaja;
    public GameManagerAirboneDanger gameManagerAirboneDanger;
    public float speed;
    public FloatingJoystick floatingJoystick;
    public ParticleSystem getGasolinaParticle;
    public ParticleSystem enemigoParticle;
    



    public void Start()
    {
        rb = GetComponent<Rigidbody>();               
        gameManagerAirboneDanger = GameObject.Find("GameManagerAirboneDanger").GetComponent<GameManagerAirboneDanger>();
        gameManagerAirboneDanger.StartGame();
       
        
    }

    public void Update()
    {
        
        // guardamos los movimientos del jostick en variables
        horizontalInput = floatingJoystick.Horizontal;
        verticalInput = floatingJoystick.Vertical;

        // Si no hay input, no aplicamos velocidad
        if (horizontalInput == 0 && verticalInput == 0)
        {
            rb.linearVelocity = Vector3.zero; // Detener movimiento si no hay input
            return;
        }



        // creamos una vector movimineto con los movimientos vertival y horizontal
        Vector3 movimiento = new Vector3(horizontalInput, verticalInput, 0) * speed;

        rb.linearVelocity = movimiento;
        
        
        //le damos una inclinacion al movimiento horizontal
        float inclinacion = -horizontalInput * anguloInclinacion; 
        transform.rotation = Quaternion.Euler(0, 0, inclinacion);



        //Control de lo limites del avion
        
        if (transform.position.x > limiteHorizontal)
        {
            transform.position = new Vector3(limiteHorizontal, transform.position.y, transform.position.z);
        }
        if (transform.position.x < -limiteHorizontal)
        
        {
            transform.position = new Vector3(-limiteHorizontal, transform.position.y, transform.position.z);
        }
        
        if (transform.position.y > limiteSuperior)
        {
            transform.position = new Vector3(transform.position.x, limiteSuperior, transform.position.z);
        }
        if (transform.position.y < limiteInferior)
        {
            transform.position = new Vector3(transform.position.x, limiteInferior, transform.position.z);
        }

        


    }
    // recolectamos gasolina o actulizamos escudo y puntacion si colisionamos con enemigos
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("gasolina"))
        {
            gameManagerAirboneDanger.UpdateScore(5);
            
            Destroy(other.gameObject);
            Instantiate(getGasolinaParticle, gameObject.transform.position, getGasolinaParticle.transform.rotation);
            audioSourceGetGasolina.Play();
        }
        else if (other.CompareTag("Enemigo"))
        {
            gameManagerAirboneDanger.UpdateScore(-10);
            gameManagerAirboneDanger.UpdateEscudo(1);        
           
            Destroy(other.gameObject);
            Instantiate(enemigoParticle, gameObject.transform.position, enemigoParticle.transform.rotation);
            audioSourceEnemigo.Play();

            if(gameManagerAirboneDanger.Escudo == 1)
            {
                audioSourceArmaduraBaja.Play();
            }

            
            

        }
    }



}
