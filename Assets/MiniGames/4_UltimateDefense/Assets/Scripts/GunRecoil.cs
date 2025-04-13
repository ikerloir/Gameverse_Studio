using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    public float recoilDistance = 0.2f;       // Qu� tanto se mueve hacia atr�s
    public float recoilSpeed = 10f;           // Qu� tan r�pido va hacia atr�s
    public float returnSpeed = 5f;            // Qu� tan r�pido vuelve a su posici�n original

    private Vector3 initialPosition;
    private Vector3 targetPosition;
    private bool isRecoiling = false;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        if (isRecoiling)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, Time.deltaTime * recoilSpeed);

            // Cuando est� cerca del punto m�s atr�s, empieza a volver
            if (Vector3.Distance(transform.localPosition, targetPosition) < 0.01f)
            {
                isRecoiling = false;
            }
        }
        else
        {
            // Vuelve a la posici�n original
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, Time.deltaTime * returnSpeed);
        }
    }

    public void TriggerRecoil()
    {
        // Se mueve hacia atr�s en el eje Z local
        targetPosition = initialPosition + -transform.forward * recoilDistance;
        isRecoiling = true;
    }
}
