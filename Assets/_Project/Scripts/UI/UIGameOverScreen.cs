
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Skoggy.LD45.UI
{
    public class UIGameOverScreen : MonoBehaviour
    {
        public Text TextResultScore;

        public void Initialize(int score)
        {
            TextResultScore.text = $"Your profit was ${score}";
        }

        public void PlayAgain()
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }
        
        public void Exit()
        {
            SceneManager.LoadScene("StartScene", LoadSceneMode.Single);
        }
    }
}