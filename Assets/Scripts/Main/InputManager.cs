using UnityEngine;
using UnityEngine.EventSystems;

namespace Orbitality.Main
{
    public class InputManager
    {
        public System.Action OnPlayerShoot;

        public void UpdateInput()
        {
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                OnPlayerShoot?.Invoke();
        }
    }
}
