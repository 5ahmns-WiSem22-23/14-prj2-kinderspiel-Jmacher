using TMPro;
using UnityEngine;

namespace Interface
{
    public class ButtonText : ButtonExtension
    {
        [SerializeField] private TextMeshProUGUI targetElement;
        [SerializeField]
        private Color[] states = new Color[]
        {Color.black, Color.white, Color.grey, Color.black};

        public override void Setup() => targetElement.color = states[0];
        public override void PointerEnter() => targetElement.color = states[1];
        public override void PointerExit() => targetElement.color = states[0];
        public override void PointerDown() => targetElement.color = states[2];
        public override void PointerUp(bool outside) => targetElement.color = outside ? states[0] : states[1];
        public override void SetState(bool state) => targetElement.color = state ? states[0] : states[3];
    }
}