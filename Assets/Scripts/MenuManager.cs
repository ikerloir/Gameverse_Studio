using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void LoadGameSelect()
    {
        
        SceneManager.LoadScene("GameSelect");
    }
}