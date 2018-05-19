using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using ClientStructure;

public class Test : MonoBehaviour {

    // Use this for initialization
    private JoyStick joyStick;
    private Toggle typeToggle;
    private Text dirText;

    private void Awake()
    {
        joyStick = GameObject.Find("Canvas/JoyStick").GetComponent<JoyStick>();
        typeToggle = GameObject.Find("Canvas/Toggle").GetComponent<Toggle>();
        dirText = GameObject.Find("Canvas/Text/Text").GetComponent<Text>();
    }

    void Start () {

        //摇杆移动半径是中间圆按钮拖动范围，按下生效范围（通过改变JoyStick的RectTransform的大小改变）
        //摇杆最大移动半径 
        joyStick.JoyStickMaxRadius = 110;
        //摇杆最小生效半径
        joyStick.JoyStickMinRadius = 0;
        //按动时中间按钮的颜色
        joyStick.OnTouchTumbColor = new Color(1f, 0.588f, 0.008f);
        //摇杆动态还是静态
        joyStick.JoyStickType = (typeToggle.isOn ? JoyStickType.Dynamic : JoyStickType.Static); //JoyStickType.Static;
        //摇杆方向改变时回调
        joyStick.OnJoyStickTouchChangeDir += TestJoyStick;
        //
        typeToggle.onValueChanged.AddListener(value=> ChangeJoyStickType(value));
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void TestJoyStick(JoyStickDirection joyStickDir)
    {
        //摇杆返回的方向：        None = 摇杆抬起,  Forward = 方向右,  Backward = 方向左,  Down = 方向下；
        dirText.text = joyStickDir.ToString();
        //Debug.Log("joyStickDir: " + joyStickDir);
    }

    private void ChangeJoyStickType(bool value)
    {
        joyStick.JoyStickType = (value ? JoyStickType.Dynamic : JoyStickType.Static);
    }
}
