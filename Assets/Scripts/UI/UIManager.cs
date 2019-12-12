using Orbitality.Main;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Orbitality.UI
{
    public class UIManager : MonoBehaviour
    {
        public System.Action OnPauseChange;

        [Header("Button")]
        public Button Button_Pause;
        public Button Button_PauseResume;
        public Button Button_Restart_Pause;
        public Button Button_Restart_BattleResult;
        public Button Button_MainMenu_Pause;
        public Button Button_MainMenu_BattleResult;

        [Header("Text")]
        public Text Text_Score;
        public Text Text_BattleResult_Title;
        public Text Text_BattleResult_Score;

        [Header("Object")]
        public GameObject Window_Pause;
        public GameObject Window_BattleResult;


        public void Init()
        {
            Button_Pause.onClick.AddListener(ButtonPause_PressHandler);
            Button_PauseResume.onClick.AddListener(ButtonPauseResume_PressHandler);
            Button_Restart_Pause.onClick.AddListener(ButtonRestart_PressHandler);
            Button_Restart_BattleResult.onClick.AddListener(ButtonRestart_PressHandler);
            Button_MainMenu_Pause.onClick.AddListener(ButtonMainMenu_PressHandler);
            Button_MainMenu_BattleResult.onClick.AddListener(ButtonMainMenu_PressHandler);

            if (Window_Pause.activeSelf)
                Window_Pause.SetActive(false);

            if (Window_BattleResult.activeSelf)
                Window_BattleResult.SetActive(false);

            UpdateScore(0);
        }

        public void HandlePauseChangeState(bool isPaused)
        {
            Window_Pause.SetActive(isPaused);

            Text_Score.gameObject.SetActive(!isPaused);
            Button_Pause.gameObject.SetActive(!isPaused);
        }

        public void UpdateScore(int score) => Text_Score.text = $"Score: {score}";

        public void ShowBattleResult(bool isWinState, int score)
        {
            Text_Score.gameObject.SetActive(false);
            Button_Pause.gameObject.SetActive(false);

            Text_BattleResult_Title.text = isWinState ? "VICTORY" : "DEFEAT";
            Text_BattleResult_Title.color = isWinState ? GameManager.Instance.AssetsLibrary.Library_Colors.VictoryColor :
                                                         GameManager.Instance.AssetsLibrary.Library_Colors.DefeatColor;

            Text_BattleResult_Score.text = $"Score: {score}";

            Window_BattleResult.gameObject.SetActive(true);
        }


        void ButtonPause_PressHandler() => OnPauseChange?.Invoke();

        void ButtonPauseResume_PressHandler() => OnPauseChange?.Invoke();

        void ButtonRestart_PressHandler() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        void ButtonMainMenu_PressHandler() => SceneManager.LoadScene(0);
    }
}
