using UnityEngine;
using System.Collections;

public class MazeGenerate : MonoBehaviour
{
    int randomSeed = 0;
    public int row;
    public int column;
    int[][] mapArray;
    Transform[][] mapGameObjectArray;
    public GameObject wallBlock;
    void Start()
    {
        Random.seed = randomSeed;
        initTransform(row, column);
        generateMap();
    }

    void generateMap()
    {
        initArray(row, column);
        generateWay();
        generateInAndOut();
        updateWall();
    }

    void OnGUI()
    {
        GUILayout.BeginVertical();
        randomSeed = int.Parse(GUILayout.TextField(randomSeed.ToString(), GUILayout.Width(100)));
        if (GUILayout.Button("生成", GUILayout.Width(100)))
        {
            Random.seed = randomSeed;
            generateMap();
        }
        GUILayout.EndVertical();
    }

    /// <summary>
    /// 初始化生成地图数组数据，交叉类型，边缘为墙1，内部为空白0
    /// </summary>
    /// <param name="row"></param>
    /// <param name="column"></param>
    void initArray(int row, int column)
    {
        mapArray = new int[row][];
        for (int i = 0; i < mapArray.Length; i++)
        {
            mapArray[i] = new int[column];
            for (int j = 0; j < mapArray[i].Length; j++)
            {
                if (i % 2 == 0)
                {
                    mapArray[i][j] = 1;
                }
                else
                {
                    if (j % 2 == 0)
                    {
                        mapArray[i][j] = 1;
                    }
                    else
                    {
                        mapArray[i][j] = 0;
                    }
                }
            }
        }
    }
    void initTransform(int row, int column)
    {
        mapGameObjectArray = new Transform[row][];
        for (int i = 0; i < mapGameObjectArray.Length; i++)
        {
            mapGameObjectArray[i] = new Transform[column];
            for (int j = 0; j < mapGameObjectArray[i].Length; j++)
            {
                mapGameObjectArray[i][j] = GameObject.Instantiate(wallBlock).transform;
                mapGameObjectArray[i][j].position = new Vector3(j, 0, i);
                mapGameObjectArray[i][j].SetParent(transform, false);
            }
        }
    }
    /// <summary>
    /// 根据数据生成地图
    /// </summary>
    void updateWall()
    {
        for (int i = 0; i < mapArray.Length; i++)
        {
            for (int j = 0; j < mapArray[i].Length; j++)
            {
                if (mapArray[i][j] == 0)
                {
                    mapGameObjectArray[i][j].gameObject.SetActive(false);
                }
                else
                {
                    mapGameObjectArray[i][j].gameObject.SetActive(true);
                }
            }
        }
    }
    /// <summary>
    /// 生成迷宫
    /// </summary>
    void generateWay()
    {
        int count = (row / 2) * (column / 2);
        ArrayList acc = new ArrayList();
        int[] noacc = new int[count];
        //初始化所有未访问对象
        for (int i = 0; i < noacc.Length; i++)
        {
            noacc[i] = 0;
        }
        //随机选取所有空白的一个点作为起点，并将此格设置为已访问
        var pos = Random.Range(0, count);
        noacc[pos] = 1;
        //已选择的格子加入列表
        acc.Add(pos);
        while (acc.Count < count)
        {
            //当前格子的行
            int r = pos / (column / 2);
            //当前格子的列
            int c = pos % (column / 2);
            int ii = 0;
            while (++ii < 5)
            {
                int tr = r, tc = c;
                int realR = 2 * r + 1, realC = 2 * c + 1;
                int tpos = pos;
                int arrow = Random.Range(0, 4);
                //顺序：上、下、左、右
                switch (arrow)
                {
                    case 0:
                        realR--;
                        tr--;
                        tpos -= column / 2;
                        break;
                    case 1:
                        realR++;
                        tr++;
                        tpos += column / 2;
                        break;
                    case 2:
                        realC--;
                        tc--;
                        tpos--;
                        break;
                    case 3:
                        realC++;
                        tc++;
                        tpos++;
                        break;
                }
                if (tr >= 0 && tc >= 0 && tr < (int)(row / 2) && tc < (int)(column / 2))
                {
                    if (tpos >= 0 && tpos < noacc.Length && noacc[tpos] == 0)
                    {
                        updateWall();
                        mapArray[realR][realC] = 0;
                        pos = tpos;
                        noacc[tpos] = 1;
                        acc.Add(tpos);
                        break;
                    }
                }
                if (ii == 4)
                {
                    pos = (int)acc[Random.Range(0, acc.Count)];
                    break;
                }
            }
        }
    }
    /// <summary>
    /// 生成出入口
    /// </summary>
    void generateInAndOut()
    {
        bool isOK = false;
        int inPoint = 0;
        int outPoint = 0;
        int inArrow = -1;
        int outArrow = -1;
        while (!isOK)
        {
            inArrow = Random.Range(0, 4);
            switch (inArrow)
            {
                case 0:
                    inPoint = Random.Range(1, column - 1);
                    if (mapArray[1][inPoint] == 0)
                    {
                        mapArray[0][inPoint] = 0;
                        isOK = true;
                    }
                    break;
                case 1:
                    inPoint = Random.Range(1, column - 1);
                    if (mapArray[row - 2][inPoint] == 0)
                    {
                        mapArray[row - 1][inPoint] = 0;
                        isOK = true;
                    }
                    break;
                case 2:
                    inPoint = Random.Range(1, row - 1);
                    if (mapArray[inPoint][1] == 0)
                    {
                        mapArray[inPoint][0] = 0;
                        isOK = true;
                    }
                    break;
                case 3:
                    inPoint = Random.Range(1, row - 1);
                    if (mapArray[inPoint][column - 2] == 0)
                    {
                        mapArray[inPoint][column - 1] = 0;
                        isOK = true;
                    }
                    break;
            }
        }
        isOK = false;
        while (!isOK)
        {
            outArrow = Random.Range(0, 4);
            if (outArrow == inArrow)
            {
                continue;
            }
            switch (outArrow)
            {
                case 0:
                    outPoint = Random.Range(1, column - 1);
                    if (mapArray[1][outPoint] == 0)
                    {
                        mapArray[0][outPoint] = 0;
                        isOK = true;
                    }
                    break;
                case 1:
                    outPoint = Random.Range(1, column - 1);
                    if (mapArray[row - 2][outPoint] == 0)
                    {
                        mapArray[row - 1][outPoint] = 0;
                        isOK = true;
                    }
                    break;
                case 2:
                    outPoint = Random.Range(1, row - 1);
                    if (mapArray[outPoint][1] == 0)
                    {
                        mapArray[outPoint][0] = 0;
                        isOK = true;
                    }
                    break;
                case 3:
                    outPoint = Random.Range(1, row - 1);
                    if (mapArray[outPoint][column - 2] == 0)
                    {
                        mapArray[outPoint][column - 1] = 0;
                        isOK = true;
                    }
                    break;
            }
        }
    }
}
