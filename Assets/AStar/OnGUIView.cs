using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGUIView : MonoBehaviour
{
    string nowStr = "默认方格";
    void OnGUI()
    {
        if (GUI.Button(new Rect(Screen.width - 100, 40, 80, 30), "设置起点！"))
        {
            nowStr = "设置起点";
            AStar.obstacle = ObstacleType.Start;
        }

        if (GUI.Button(new Rect(Screen.width - 100, 100, 80, 30), "设置障碍"))
        {
            nowStr = "设置障碍";
            AStar.obstacle = ObstacleType.Obstacle;
        }

        if (GUI.Button(new Rect(Screen.width - 100, 160, 80, 30), "设置终点"))
        {
            nowStr = "设置终点";
            AStar.obstacle = ObstacleType.End;
        }

        if (GUI.Button(new Rect(Screen.width - 100, 220, 80, 30), "默认方格"))
        {
            nowStr = "默认方格";
            AStar.obstacle = ObstacleType.Default;
        }

        GUI.Box(new Rect(Screen.width - 200, 100, 80, 30), nowStr);
        GUI.Box(new Rect(Screen.width - 300, 280, 300, 40), "请点击右上角button再点击想设置的格子\n手动设置好节点之后，点击空格开始寻路");
    }

}
