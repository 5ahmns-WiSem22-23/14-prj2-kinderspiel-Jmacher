using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Interface
{
    [AddComponentMenu("Interface/Button/Button")]
    public class Button : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] UnityEvent onClick;
        [SerializeField] ButtonExtension[] extensions = new ButtonExtension[1];

        private bool active = true, pressed, outside;
        public bool Active
        {
            get => active;
            set
            {
                foreach (ButtonExtension ext in extensions) ext.SetState(value);
                active = value;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!active) return;
            if (pressed) return;
            foreach (ButtonExtension ext in extensions) ext.PointerEnter();
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            if (!active) return;
            if (pressed) { outside = true; return; }
            foreach (ButtonExtension ext in extensions) ext.PointerExit();
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            if (!active) return;
            pressed = true;
            foreach (ButtonExtension ext in extensions) ext.PointerDown();
            onClick.Invoke();
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            if (!active) return;
            foreach (ButtonExtension ext in extensions) ext.PointerUp(outside);
            pressed = outside = false;
        }
    }

    public abstract class ButtonExtension : MonoBehaviour
    {
        private void Awake() => Setup();
        public abstract void Setup();

        public abstract void PointerEnter();
        public abstract void PointerExit();
        public abstract void PointerDown();
        public abstract void PointerUp(bool outside);

        public abstract void SetState(bool state);
    }
}