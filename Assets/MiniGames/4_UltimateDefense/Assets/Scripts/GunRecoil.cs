using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    public float recoilDistance = 0.2f;       // Qué tanto se mueve hacia atrás
    public float recoilSpeed = 10f;           // Qué tan rápido va hacia atrás
    public float returnSpeed = 5f;            // Qué tan rápido vuelve a su posición original

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

            // Cuando está cerca del punto más atrás, empieza a volver
            if (Vector3.Distance(transform.localPosition, targetPosition) < 0.01f)
            {
                isRecoiling = false;
            }
        }
        else
        {
            // Vuelve a la posición original
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, Time.deltaTime * returnSpeed);
        }
    }

    public void TriggerRecoil()
    {
        // Se mueve hacia atrás en el eje Z local
        targetPosition = initialPosition + -transform.forward * recoilDistance;
        isRecoiling = true;
    }
}
