using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ClientStructure
{
    public class JoyStickThum : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        /// <summary>
        /// 摇杆最大半径
        /// </summary>
        private float joyStickMaxRadius = 100;

        private float joyStickMinRadius = 200;
        /// <summary>
        /// 当摇杆按下时颜色/正常颜色
        /// </summary>
        private Color thumOnTouchColor, thumNormalColor;
        /// <summary>
        /// thum图片
        /// </summary>
        private Image thumImage;
        /// <summary>
        /// 当前物体的Transform组件
        /// </summary>
        private RectTransform thumRT;
        /// <summary>
        /// 是否触摸了虚拟摇杆
        /// </summary>
        private bool isOnTouch = false;
        /// <summary>
        /// 虚拟摇杆的默认位置
        /// </summary>
        //private Vector2 originPosition;
        /// <summary>
        /// 虚拟摇杆的移动方向
        /// </summary>
        private Vector2 touchedAxis;

        /// <summary>
        /// 定义触摸开始事件委托
        /// </summary>
        public delegate void JoyStickTouchBegin(Vector2 vec);
        /// <summary>
        /// 定义触摸过程事件委托
        /// </summary>
        /// <param name="vec">虚拟摇杆的移动方向</param>
        public delegate void JoyStickTouchMove(Vector2 vec);
        /// <summary>
        /// 定义触摸结束事件委托
        /// </summary>
        public delegate void JoyStickTouchEnd();
        /// <summary>
        /// 注册触摸开始事件
        /// </summary>
        public event JoyStickTouchBegin OnJoyStickTouchBegin;
        /// <summary>
        /// 注册触摸过程事件
        /// </summary>
        public event JoyStickTouchMove OnJoyStickTouchMove;
        /// <summary>
        /// 注册触摸结束事件
        /// </summary>
        public event JoyStickTouchEnd OnJoyStickTouchEnd;
        void Start()
        {
            //初始化虚拟摇杆的默认方向
            thumRT = GetComponent<RectTransform>();
            thumImage = thumRT.GetComponent<Image>();
            thumNormalColor = thumImage.color;

        }
        public void OnPointerDown(PointerEventData eventData)
        {
            thumImage.color = thumOnTouchColor;    
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            isOnTouch = false;
            thumImage.color = thumNormalColor;
            thumRT.anchoredPosition = Vector2.zero;
            touchedAxis = Vector2.zero;
            if (OnJoyStickTouchEnd != null)
                OnJoyStickTouchEnd();
        }
        public void OnDrag(PointerEventData eventData)
        {
            touchedAxis = GetJoyStickAxis(eventData);
            if (touchedAxis.magnitude > joyStickMinRadius)
            {
                if (!isOnTouch)
                {
                    isOnTouch = true;
                    if (OnJoyStickTouchBegin != null)
                    {
                        OnJoyStickTouchBegin(TouchedAxis);
                    }
                }
                if (OnJoyStickTouchMove != null)
                    OnJoyStickTouchMove(TouchedAxis);
            }
        }
        void Update()
        {

        }
        /// <summary>
        /// 返回虚拟摇杆的偏移量
        /// </summary>
        /// <returns>The joy stick axis.</returns>
        /// <param name="eventData">Event data.</param>
        private Vector2 GetJoyStickAxis(PointerEventData eventData)
        {
            //获取手指位置的世界坐标
            Vector3 worldPosition;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(thumRT,
                     eventData.position, eventData.pressEventCamera, out worldPosition))
                thumRT.position = worldPosition;
            //获取摇杆的偏移量
            Vector2 touchAxis = thumRT.anchoredPosition - Vector2.zero;
            //摇杆偏移量限制
            if (touchAxis.magnitude >= joyStickMaxRadius)
            {
                touchAxis = touchAxis.normalized * joyStickMaxRadius;
                thumRT.anchoredPosition = touchAxis;
            }
            return touchAxis;
        }

        public Vector2 TouchedAxis
        {
            get
            {
                return touchedAxis.normalized;
            }
        }

        private void OnDisable()
        {
            OnPointerUp(null);
        }

        public float JoyStickMaxRadius
        {
            get
            {
                return joyStickMaxRadius;
            }
            set
            {
                joyStickMaxRadius = value;
            }
        }

        public float JoyStickMinRadius
        {
            get
            {
                return joyStickMinRadius;
            }
            set
            {
                joyStickMinRadius = value;
            }
        }

        public Color ThumOnTouchColor
        {
            set
            {
                thumOnTouchColor = value;
            }
        }
    }
}