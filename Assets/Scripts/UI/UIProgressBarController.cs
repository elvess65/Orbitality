using UnityEngine;

namespace Orbitality.UI
{
    public class UIProgressBarController : MonoBehaviour
    {
        public SpriteRenderer BarSprite;

        private float m_InitValue;

        public void Init(float initValue)
        {
            m_InitValue = initValue;
            UpdateValue(m_InitValue);
        }

        public void UpdateValue(float currentValue)
        {
            float progress = Mathf.Clamp01(currentValue / m_InitValue);
            BarSprite.transform.localScale = new Vector3(progress, BarSprite.transform.localScale.y, BarSprite.transform.localScale.z);
        }
    }
}
