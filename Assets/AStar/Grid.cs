using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Grid : MonoBehaviour, IPointerClickHandler
{
    public ObstacleType Obstacle;
    public GameObject grid;
    public Color Color
    {
        get => grid.GetComponent<Image>().color;
        set => grid.GetComponent<Image>().color = value;
    }
    public Text F;
    public Text G;
    public Text H;
    public Text Num;
    [HideInInspector]
    public float FValue;

    public float GetF
    {
        get => float.Parse(G.text) + float.Parse(H.text);
    }

    [HideInInspector]
    public int GetX;
    [HideInInspector]
    public int GetY;

    [HideInInspector]
    public Grid gridParent;

  
    void Start()
    {

    }


    void Update()
    {
        Show(Obstacle);

        State();
    }

    public void Show(ObstacleType Obstacle)
    {

        switch (Obstacle)
        {
            case ObstacleType.Default:
                Color = Color.black / 2;
                break;
            case ObstacleType.Path:
                Color = Color.green / 2;
                break;
            case ObstacleType.Start:
                Color = Color.blue / 2;
                break;
            case ObstacleType.Obstacle:
                Color = Color.red / 2;
                break;
            case ObstacleType.End:
                Color = Color.yellow / 2;
                break;
            default:
                break;
        }
    }

    private void State()
    {
        if (((Obstacle == ObstacleType.Start && !AStar.start.Equals(this)) || (Obstacle == ObstacleType.End && !AStar.destination.Equals(this))))
        {
            Obstacle = ObstacleType.Default;
        }
    }


    public void SetFGH(float g, float h)
    {
        FValue = g + h;
        F.text = $"F:{FValue}";
        G.text = $"G:{g}";
        H.text = $"H:{h}";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Obstacle = AStar.obstacle;

        AStar.start = AStar.obstacle == ObstacleType.Start ? this : AStar.start;
        AStar.destination = AStar.obstacle == ObstacleType.End ? this : AStar.destination;
    }
}
public enum ObstacleType
{
    Default,
    Path,
    Start,
    Obstacle,
    End
}
