using UnityEngine;
using System.Collections;

public class MazeGenerate2D : MonoBehaviour
{
    int randomSeed = 0;
    int row = 21;
    int column = 21;
    int[][] mapArray;
    Transform[][] mapGameObjectArray;
    Transform[][] floorGameObjectArray;
    public GameObject player;
    public GameObject wallBlock;
    public GameObject floorBlock;

    bool isGenerateReady = false;

    Vector3 inPos = Vector3.zero;
    Vector3 outPos = Vector3.zero;

    public static float cameraMinX;
    public static float cameraMaxX;
    public static float cameraMinY;
    public static float cameraMaxY;

    void Start()
    {
        init();
    }

    void OnEnable()
    {
        init();
    }

    public void init()
    {
        float gridSize = Screen.height / (Camera.main.orthographicSize * 2f);
        float showColumn = Screen.width / gridSize;
        cameraMinX = showColumn / 2f - 0.5f;
        cameraMaxX = column - (showColumn / 2f) - 0.5f;
        cameraMinY = Camera.main.orthographicSize - 0.5f;
        cameraMaxY = row - Camera.main.orthographicSize - 0.5f;
        initTransform(row, column);

        if (mapGameObjectArray == null)
        {
            return;
        }
        isGenerateReady = false;
        inPos = Vector3.zero;
        outPos = Vector3.zero;
        //Random.seed = Mathf.RoundToInt(Time.time * 10000 * Random.Range(1, 1000));
        Random.seed = randomSeed;
        generateMap();
    }

    void Update()
    {
        checkWin();
    }

    void generateMap()
    {
        initArray(row, column);
        generateWay();
        generateInAndOut();
        player.transform.position = inPos;
        updateWall();
    }

    void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.Label("迷宫参数");
        GUILayout.Label("行数：");
        row = int.Parse(GUILayout.TextField(row.ToString(), GUILayout.Width(100)));
        GUILayout.Label("列数：");
        column = int.Parse(GUILayout.TextField(column.ToString(), GUILayout.Width(100)));
        GUILayout.Label("随机种子：");
        randomSeed = int.Parse(GUILayout.TextField(randomSeed.ToString(), GUILayout.Width(100)));
        if (GUILayout.Button("生成", GUILayout.Width(100)))
        {
            Random.seed = randomSeed;
            init();
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
        foreach (Transform item in transform)
        {
            GameObject.Destroy(item.gameObject);
        }

        floorGameObjectArray = new Transform[row][];
        mapGameObjectArray = new Transform[row][];
        for (int i = 0; i < mapGameObjectArray.Length; i++)
        {
            floorGameObjectArray[i] = new Transform[column];
            mapGameObjectArray[i] = new Transform[column];
            for (int j = 0; j < mapGameObjectArray[i].Length; j++)
            {
                floorGameObjectArray[i][j] = GameObject.Instantiate(floorBlock).transform;
                floorGameObjectArray[i][j].position = new Vector3(j, i, 0);
                floorGameObjectArray[i][j].SetParent(transform, false);

                mapGameObjectArray[i][j] = GameObject.Instantiate(wallBlock).transform;
                mapGameObjectArray[i][j].position = new Vector3(j, i, 0);
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
                        inPos = mapGameObjectArray[0][inPoint].transform.position;
                        isOK = true;
                    }
                    break;
                case 1:
                    inPoint = Random.Range(1, column - 1);
                    if (mapArray[row - 2][inPoint] == 0)
                    {
                        mapArray[row - 1][inPoint] = 0;
                        inPos = mapGameObjectArray[row - 1][inPoint].transform.position;
                        isOK = true;
                    }
                    break;
                case 2:
                    inPoint = Random.Range(1, row - 1);
                    if (mapArray[inPoint][1] == 0)
                    {
                        mapArray[inPoint][0] = 0;
                        inPos = mapGameObjectArray[inPoint][0].transform.position;
                        isOK = true;
                    }
                    break;
                case 3:
                    inPoint = Random.Range(1, row - 1);
                    if (mapArray[inPoint][column - 2] == 0)
                    {
                        mapArray[inPoint][column - 1] = 0;
                        inPos = mapGameObjectArray[inPoint][column - 1].transform.position;
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
                        outPos = mapGameObjectArray[0][outPoint].transform.position;
                        isOK = true;
                    }
                    break;
                case 1:
                    outPoint = Random.Range(1, column - 1);
                    if (mapArray[row - 2][outPoint] == 0)
                    {
                        mapArray[row - 1][outPoint] = 0;
                        outPos = mapGameObjectArray[row - 1][outPoint].transform.position;
                        isOK = true;
                    }
                    break;
                case 2:
                    outPoint = Random.Range(1, row - 1);
                    if (mapArray[outPoint][1] == 0)
                    {
                        mapArray[outPoint][0] = 0;
                        outPos = mapGameObjectArray[outPoint][0].transform.position;
                        isOK = true;
                    }
                    break;
                case 3:
                    outPoint = Random.Range(1, row - 1);
                    if (mapArray[outPoint][column - 2] == 0)
                    {
                        mapArray[outPoint][column - 1] = 0;
                        outPos = mapGameObjectArray[outPoint][column - 1].transform.position;
                        isOK = true;
                    }
                    break;
            }
        }
        isGenerateReady = true;
    }

    void checkWin()
    {
        if (!isGenerateReady)
        {
            return;
        }
        if (Vector3.Distance(player.transform.position, outPos) < 0.2f)
        {
            Camera.main.GetComponent<GameManager>().winGame();
            randomSeed++;
        }
    }
}
