using UnityEngine;
using UnityEngine.SceneManagement;

namespace Orbitality.MainMenu
{
    public class MainMenuSceneController : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
