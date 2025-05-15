using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Button))]
public class StartGame : MonoBehaviour
{
    private Button button;
    

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
        
    }

    void OnButtonClick()
    {
        SceneManager.LoadScene("MortalBagsII");
    }

    void OnDestroy()
    {
        if (button != null)
        {
            button.onClick.RemoveListener(OnButtonClick);
        }
    }
}
//public void StartPlay()
//    {
//        SceneManager.LoadScene("MortalBagsII");
//    }

