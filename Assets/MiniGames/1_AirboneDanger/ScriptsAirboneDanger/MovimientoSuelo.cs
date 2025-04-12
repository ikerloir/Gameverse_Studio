using UnityEngine;

public class MovimientoSuelo : MonoBehaviour
{
    private float speed = 60.0f; // Velocidad de movimiento del suelo
    public Transform mar1; // Referencia al suelo1
    public Transform mar2; // Referencia al suelo2
    public Transform contenedorMar; // Referencia al contenedor que tiene ambos suelos
    private bool iniciarMovimiento = false;
    private Animation avionAnimation;
    public Transform avion;
    public GameManagerAirboneDanger gameManagerAirboneDanger;


    private Vector3 startPosition; // Posición inicial del mar
    private float repeatWidth; // Ancho del suelo 

    void Start()
    {
        // Guardamos la posición inicial del primer suelo
        startPosition = mar1.position;
        gameManagerAirboneDanger = GameObject.Find("GameManagerAirboneDanger").GetComponent<GameManagerAirboneDanger>();
        repeatWidth = mar1.GetComponent<Renderer>().bounds.size.z;
        avionAnimation = avion.GetComponent<Animation>(); // Obtener el Animator del avión
    }

    void Update()
    {
        //comprobamos si la animacion a acabado
        if (avionAnimation != null)
        {
            if (!avionAnimation.isPlaying || !avionAnimation.IsPlaying("Despegue"))
            {
                iniciarMovimiento = true; //  moveremos el suelo cuando acabe la animacion 
            }
        }
    }

    private void LateUpdate()
    {
        //cuando acabe la animacion empezamos
        if (iniciarMovimiento && gameManagerAirboneDanger.isGameActive)
        {
            // Mover ambos suelos hacia atrás usando el contenedor
            contenedorMar.Translate(Vector3.back * speed * Time.deltaTime);

            // Si el primer suelo ha salido de la pantalla, lo reposicionamos detrás del segundo
            if (mar1.position.z <= startPosition.z - repeatWidth)
            {
                mar1.position = mar2.position + new Vector3(0, 0, repeatWidth ); // Reposicionamos el suelo1
            }

            // Si el segundo suelo ha salido de la pantalla, lo reposicionamos detrás del primero
            if (mar2.position.z <= startPosition.z - repeatWidth)
            {
                mar2.position = mar1.position + new Vector3(0, 0, repeatWidth ); // Reposicionamos el suelo2
            }
        }
    } 
    

}