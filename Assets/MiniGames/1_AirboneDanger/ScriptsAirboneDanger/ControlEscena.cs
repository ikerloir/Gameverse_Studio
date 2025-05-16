using UnityEngine;

public class ControlEscena : MonoBehaviour
{
    private float velocidadRotacion = 1 ;
    
    // movimineto del SkyBox
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * velocidadRotacion);
    }
}