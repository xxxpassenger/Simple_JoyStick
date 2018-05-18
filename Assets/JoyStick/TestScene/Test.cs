using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ClientStructure;

public class Test : MonoBehaviour {

    // Use this for initialization
    private JoyStick joyStick;
	void Start () {
        joyStick = GameObject.Find("Canvas/JoyStick").GetComponent<JoyStick>();
        //摇杆移动半径是中间圆按钮拖动范围，按下生效范围（通过改变JoyStick的RectTransform的大小改变）
        //摇杆最大移动半径 
        joyStick.JoyStickMaxRadius = 200;
        //摇杆最小生效半径
        joyStick.JoyStickMinRadius = 20;
        //按动时中间按钮的颜色
        joyStick.OnTouchTumbColor = Color.red;
        //摇杆动态还是静态
        joyStick.JoyStickType = JoyStickType.Dynamic;
        //摇杆方向改变时回调
        joyStick.OnJoyStickTouchChangeDir += TestJoyStick;
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void TestJoyStick(JoyStickDirection joyStickDir)
    {
        //摇杆返回的方向：        None = 摇杆抬起,  Forward = 方向右,  Backward = 方向左,  Down = 方向下；
        Debug.Log("joyStickDir: " + joyStickDir);
    }
}
