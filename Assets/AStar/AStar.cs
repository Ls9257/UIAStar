using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Linq;
public class AStar : MonoBehaviour
{
    public Grid ima;
    public Transform parent;
    public Grid[,] images;

    [HideInInspector]
    public List<Grid> openGrid = new List<Grid>();
    [HideInInspector]
    public List<Grid> colseGrid = new List<Grid>();

    //起点
    [HideInInspector]
    public static Grid start;
    //终点
    [HideInInspector]
    public static Grid destination;

    //缓存的寻路结果
    private List<Grid> pathList = new List<Grid>();

    public static ObstacleType obstacle;
    public int width = 10;
    public int height = 10;

    void Start()
    {
        obstacle = ObstacleType.Default;
        CreatGrid(width, height);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (start == null || destination == null)
                return;
            ResetList();
            openGrid.Add(start);
            while (!FindMin())
            {

            }
        }
    }

    private void ResetList()
    {
        for (int i = 0; i < pathList.Count; i++)
        {
            if (pathList[i].Obstacle == ObstacleType.Path)
            {
                pathList[i].Obstacle = ObstacleType.Default;
            }
        }

        pathList.Clear();
        openGrid.Clear();
        colseGrid.Clear();
    }

    //创建格子
    private void CreatGrid(int width, int height)
    {
        images = new Grid[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                var temp = Instantiate<Grid>(ima, parent);
                temp.gameObject.SetActive(true);
                temp.name = string.Format("{0},{1}", i, j);
                images[i, j] = temp;
                temp.GetX = i;
                temp.GetY = j;
            }
        }
    }

    //以当前节点为中心，找到其它方向的可行走节点
    public void StartFind(Grid grid)
    {
        
        int letfDeviation = 0, UpDeviation = 0;
        //增加一些权重，尽量使当前节点朝着正确方向前进
        Weighitng(ref letfDeviation, ref UpDeviation, grid);


        //如果可以斜着走，就寻找八个方向，否则就是找四个方向

        ////左上
        //if (grid.GetX - 1 >= 0 && grid.GetY - 1 >= 0)
        //    AddOpenList(grid.GetX - 1, grid.GetY - 1, grid, 14);
        //左
        if (grid.GetY - 1 >= 0)
            AddOpenList(grid.GetX, grid.GetY - 1, grid, 10 + letfDeviation);
        ////左下
        //if (grid.GetX + 1 < height && grid.GetY - 1 >= 0)
        //    AddOpenList(grid.GetX + 1, grid.GetY - 1, grid, 14);
        //上
        if (grid.GetX - 1 >= 0)
            AddOpenList(grid.GetX - 1, grid.GetY, grid, 10 + UpDeviation);
        //下
        if (grid.GetX + 1 < height)
            AddOpenList(grid.GetX + 1, grid.GetY, grid, 10 - UpDeviation);
        ////右上
        //if (grid.GetX - 1 >= 0 && grid.GetY + 1 < height)
        //    AddOpenList(grid.GetX - 1, grid.GetY + 1, grid, 14);
        //右
        if (grid.GetY + 1 < height)
            AddOpenList(grid.GetX, grid.GetY + 1, grid, 10 - letfDeviation);
        ////右下
        //if (grid.GetX + 1 < height && grid.GetY + 1 < height)
        //    AddOpenList(grid.GetX + 1, grid.GetY + 1, grid, 14);;
        colseGrid.Add(grid);
    }


    private void Weighitng(ref int letfDeviation, ref int UpDeviation, Grid grid)
    {
        //如果终点在当前节点的右边，向左走会有+1的代价,向右走会有-1的代价
        if (destination.GetY > grid.GetY)
        {
            letfDeviation = 1;
        }
        else if (destination.GetY < grid.GetY)
        {
            letfDeviation = -1;
        }

        //如果终点在当前节点的上边，向下走会有+1的代价,向上走会有-1的代价
        if (destination.GetX > grid.GetX)
        {
            UpDeviation = 1;
        }
        else if (destination.GetX < grid.GetX)
        {
            UpDeviation = -1;
        }
    }

    public void AddOpenList(int x, int y, Grid parent, int G)
    {
        Grid grid = images[x, y];
        if (colseGrid.Contains(grid) || grid.Obstacle == ObstacleType.Obstacle)
        {
            return;
        }

        ////曼哈顿式
        //var H = Mathf.Abs(destination.GetX - x) + Mathf.Abs(destination.GetY - y);
        //grid.SetFGH(G, H * 10);

        ////欧几里得式
        //var H = Mathf.Sqrt((Mathf.Pow(destination.GetX - x, 2) + Mathf.Pow(destination.GetY - y, 2)));
        //grid.SetFGH(G, H * 10);

        //最短路径，为了避免遍历所有节点，建议增加一些其它权重（如上）
        grid.SetFGH(G, 0);

        if (!openGrid.Contains(grid))
        {
            grid.gridParent = parent;
            openGrid.Add(grid);
        }


    }

    public bool FindMin()
    {
      
        if (openGrid.Count < 1)
        {
            return true;
        }
        Grid grid = openGrid.Find(o => o.FValue == openGrid.Min(a => a.FValue));
        StartFind(grid);
        if (grid.transform.name == destination.transform.name)
        {
            while (grid.gridParent != null)
            {
                grid = grid.gridParent;
                if (grid.gridParent != null)
                {
                    grid.Obstacle = ObstacleType.Path;
                    pathList.Add(grid);
                }
            }
            return true;
        }
        openGrid.Remove(grid);
       
        return false;
    }

}
