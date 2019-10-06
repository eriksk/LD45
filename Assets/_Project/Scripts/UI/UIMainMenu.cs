using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Skoggy.LD45.UI
{
    public class UIMainMenu : MonoBehaviour
    {
        public Button PlayButton, ExitButton;

        void Start()
        {   
            PlayButton.onClick.AddListener(() => 
            {
                SceneManager.LoadScene("Game", LoadSceneMode.Single);
            });
            ExitButton.onClick.AddListener(() => 
            {
                Debug.Log("Quitting");
                Application.Quit();
            });            
        }
    }
}