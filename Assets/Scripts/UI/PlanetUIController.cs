using UnityEngine;

namespace Orbitality.UI
{
    public class PlanetUIController : MonoBehaviour
    {
        public UIProgressBarController HPBarController;
        public UIProgressBarController ReloadBarController;

        public void Init(int initHP)
        {
            HPBarController.Init(initHP);
            ReloadBarController.Init(1);

            ShowHPBar(false);
            ShowReloadBar(false);
        }

        public void ShowHPBar(bool isShow) => HPBarController.gameObject.SetActive(isShow);

        public void ShowReloadBar(bool isShow) => ReloadBarController.gameObject.SetActive(isShow);
    }
}
