using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

namespace ClientStructure
{
    public class JoyStickTouchJudge : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        private RectTransform joyStickBG;
        private JoyStickThum joyStickThum;
        private JoyStickType joyStickType;


        private void Awake()
        {
            joyStickBG = transform.Find("JoyStick").GetComponent<RectTransform>();
            joyStickThum = transform.Find("JoyStick/Thum").GetComponent<JoyStickThum>();
        }

        private void Start()
        {

        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Vector3 worldPosition;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(transform as RectTransform,
eventData.position, eventData.pressEventCamera, out worldPosition))

                if (joyStickType == JoyStickType.Dynamic)
                {
                    joyStickBG.position = worldPosition;
                }
                else
                {
                    joyStickThum.OnDrag(eventData);
                }
            joyStickThum.OnPointerDown(eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            joyStickThum.OnPointerUp(eventData);
            if (joyStickType == JoyStickType.Dynamic)
            {
                joyStickBG.anchoredPosition = Vector2.zero;
            }
        }
        public void OnDrag(PointerEventData eventData)
        {

            joyStickThum.OnDrag(eventData);
        }
        private void OnDisable()
        {
            OnPointerUp(null);
        }

        public JoyStickType JoyStickType
        {
            set
            {
                joyStickType = value;
            }
        }
    }
}


