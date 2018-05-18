using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ClientStructure;

namespace ClientStructure
{
    /// <summary>
    /// 摇杆类型
    /// </summary>
    public enum JoyStickType
    {
        Static = 0,
        Dynamic,
    }

    /// <summary>
    /// 战斗ui方向枚举
    /// </summary>
    public enum JoyStickDirection
    {
        None = 0,
        Forward,
        Backward,
        Up,
        Down,
    }

    [RequireComponent(typeof(JoyStickTouchJudge))]
    public class JoyStick : MonoBehaviour
    {
        public float joyStickMaxRadius, joyStickMinRadius;
        public Color onTouchTumbColor;
        public JoyStickType joyStickType;
        private Transform arrow;
        private JoyStickThum joyStickThum;
        private JoyStickDirection curJoyStickDir, lastJoyStickDir;
        private JoyStickTouchJudge joyStickTouchJudge;

        /// <summary>
        /// 定义触摸开始事件委托
        /// </summary>
        public delegate void JoyStickTouchBegin(JoyStickDirection joyStickDir);
        /// <summary>
        /// 定义触摸方向改变委托
        /// </summary>
        /// <param name="joyStickDir"></param>
        public delegate void JoyStickTouchChangeDir(JoyStickDirection joyStickDir);
        /// <summary>
        /// 定义触摸过程事件委托
        /// </summary>
        /// <param name="vec">虚拟摇杆的移动方向</param>
        public delegate void JoyStickTouchMove(JoyStickDirection joyStickDir);
        /// <summary>
        /// 定义触摸结束事件委托
        /// </summary>
        public delegate void JoyStickTouchEnd();
        /// <summary>
        /// 注册触摸开始事件
        /// </summary>
        public event JoyStickTouchBegin OnJoyStickTouchBegin;
        /// <summary>
        /// 注册触摸方向改变事件
        /// </summary>
        public event JoyStickTouchChangeDir OnJoyStickTouchChangeDir;
        /// <summary>
        /// 注册触摸过程事件
        /// </summary>
        public event JoyStickTouchMove OnJoyStickTouchMove;
        /// <summary>
        /// 注册触摸结束事件
        /// </summary>
        public event JoyStickTouchEnd OnJoyStickTouchEnd;

        // Use this for initialization
        private void Awake()
        {
            joyStickThum = transform.Find("JoyStick/Thum").GetComponent<JoyStickThum>();
            arrow = transform.Find("JoyStick/Arrow");
            joyStickTouchJudge = GetComponent<JoyStickTouchJudge>();
            curJoyStickDir = JoyStickDirection.None;
            lastJoyStickDir = JoyStickDirection.None;
            joyStickThum = GetComponentInChildren<JoyStickThum>();
            joyStickThum.JoyStickMaxRadius = joyStickMaxRadius;
            joyStickThum.JoyStickMinRadius = joyStickMinRadius;
            joyStickThum.ThumOnTouchColor = onTouchTumbColor;
            joyStickTouchJudge.JoyStickType = joyStickType;
        }

        private void Start()
        {
            joyStickThum.OnJoyStickTouchBegin += OnJoyStickThumTouchBegin;
            joyStickThum.OnJoyStickTouchMove += OnJoyStickThumTouchMove;
            joyStickThum.OnJoyStickTouchEnd += OnJoyStickThumTouchEnd;
        }

        public void OnJoyStickThumTouchBegin(Vector2 vec)
        {
            if (OnJoyStickTouchBegin!=null)
            {
                OnJoyStickTouchBegin.Invoke(curJoyStickDir);
            }
        }

        public void OnJoyStickThumTouchMove(Vector2 vec)
        {
            lastJoyStickDir = curJoyStickDir;

            float _angle = Vector2.Angle(vec, Vector2.up);

            if (vec.x >= 0 && _angle <= 135)
                curJoyStickDir = JoyStickDirection.Forward;
            else if (vec.x < 0 && _angle <= 135)
                curJoyStickDir = JoyStickDirection.Backward;
            else if (_angle > 135)
                curJoyStickDir = JoyStickDirection.Down;

            if (OnJoyStickTouchMove != null)
            {
                OnJoyStickTouchMove.Invoke(curJoyStickDir);
            }

            if (lastJoyStickDir!=curJoyStickDir)
            {
                switch (curJoyStickDir)
                {
                    case JoyStickDirection.Forward:
                        arrow.localRotation = Quaternion.Euler(new Vector3(0,0,-90));
                        break;
                    case JoyStickDirection.Backward:
                        arrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 90));
                        break;
                    case JoyStickDirection.Down:
                        arrow.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
                        break;
                }

                if (!arrow.gameObject.activeSelf)
                {
                    arrow.gameObject.SetActive(true);
                }

                if (OnJoyStickTouchChangeDir!=null)
                {
                    lastJoyStickDir = curJoyStickDir;
                    OnJoyStickTouchChangeDir.Invoke(curJoyStickDir);
                }
            }
        }

        public void OnJoyStickThumTouchEnd()
        {
            if (arrow.gameObject.activeSelf)
            {
                arrow.gameObject.SetActive(false);
            }

            if (OnJoyStickTouchEnd!=null)
            {
                OnJoyStickTouchEnd.Invoke();
            }

            curJoyStickDir = JoyStickDirection.None;
            if (curJoyStickDir!=lastJoyStickDir)
            {
                if (OnJoyStickTouchChangeDir != null)
                {
                    lastJoyStickDir = curJoyStickDir;
                    OnJoyStickTouchChangeDir.Invoke(curJoyStickDir);
                }
            }
        }

        private void OnDisable()
        {
            if (OnJoyStickTouchEnd != null)
            {
                OnJoyStickTouchEnd.Invoke();
            }
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
                if (joyStickThum!=null)
                {
                    joyStickThum.JoyStickMaxRadius = joyStickMaxRadius;
                }
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
                if (joyStickThum != null)
                {
                    joyStickThum.JoyStickMinRadius = joyStickMinRadius;
                }
            }
        }

        public Color OnTouchTumbColor
        {
            get
            {
                return onTouchTumbColor;
            }
            set
            {
                onTouchTumbColor = value;
                if (joyStickThum!=null)
                {
                    joyStickThum.ThumOnTouchColor = onTouchTumbColor;
                }
            }
        }

        public JoyStickType JoyStickType
        {
            get
            {
                return joyStickType;
            }
            set
            {
                joyStickType = value;
                if (joyStickTouchJudge != null)
                {
                    joyStickTouchJudge.JoyStickType = value;
                }
            }
        }
    }
}
