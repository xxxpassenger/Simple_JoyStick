using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ClientStructure
{
    public class ButtonExtension : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        private bool isPressDown = false;

        public delegate void ButtonDown();

        public event ButtonDown OnButtonDown;

        public delegate void ButtonUp();

        public event ButtonUp OnButtonUp;

        public delegate void TouchExitButton();

        public event TouchExitButton OnTouchExitButton;

        public void OnPointerDown(PointerEventData eventData)
        {
            IsPressDown = true;

            if (OnButtonDown != null)
            {
                OnButtonDown.Invoke();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            IsPressDown = false;

            if (OnButtonUp != null)
            {
                OnButtonUp.Invoke();
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IsPressDown = false;

            if (OnButtonUp != null)
            {
                OnButtonUp.Invoke();
            }
        }

        public bool IsPressDown
        {
            get
            {
                return isPressDown;
            }

            set
            {
                isPressDown = value;
            }
        }
    }
}
