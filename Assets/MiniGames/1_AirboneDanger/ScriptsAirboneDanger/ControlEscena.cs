using UnityEngine;

public class ControlEscena : MonoBehaviour
{
    private float velocidadRotacion = 1 ;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * velocidadRotacion);
    }
}