using UnityEngine;
using UnityEngine.UI;

namespace Interface
{
    [AddComponentMenu("Interface/Button/Button Image")]
    public class ButtonImage : ButtonExtension
    {
        [SerializeField] private Image targetElement;
        [SerializeField]
        private Color[] states = new Color[]
        {Color.white, Color.black, Color.black, Color.grey};

        public override void Setup() => targetElement.color = states[0];
        public override void PointerEnter() => targetElement.color = states[1];
        public override void PointerExit() => targetElement.color = states[0];
        public override void PointerDown() => targetElement.color = states[2];
        public override void PointerUp(bool outside) => targetElement.color = outside ? states[0] : states[1];
        public override void SetState(bool state) => targetElement.color = state ? states[0] : states[3];
    }
}