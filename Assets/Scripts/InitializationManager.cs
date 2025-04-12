using UnityEngine;

public class InitializationManager : MonoBehaviour
{
    public void InitializeGame(string gameType)
    {
        if (gameType == "VR")
        {
            Debug.Log("Iniciando configuración de VR...");
        }
        else if (gameType == "AR")
        {
            Debug.Log("Iniciando configuración de AR...");
        }
        else
        {
            Debug.Log("Iniciando configuración estándar...");
        }
    }
}