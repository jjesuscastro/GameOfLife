using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    private static int SCREEN_WIDTH = 64;
    private static int SCREEN_HEIGHT = 36;

    public Transform cellContainer;
    public Cell cellPrefab;
    public float speed;
    public float minSpeed = 0.1f;
    public float maxSpeed = 100f;

    [HideInInspector]
    public UnityEvent onPopulationUpdate;
    [HideInInspector]
    public UnityEvent onSpeedChanged;
    [HideInInspector]
    public UnityEvent onSizeChanged;

    Camera mainCamera;
    Cell[,] grid = new Cell[SCREEN_WIDTH, SCREEN_HEIGHT];
    int liveCells;
    int generation = 1;
    float timer;
    bool isPlaying;

    #region Singleton
    public static GameManager instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Multiple GameManager(s) found!");
            Destroy(this.gameObject);
            return;
        }

        instance = this;
    }
    #endregion

    private void Start()
    {
        mainCamera = Camera.main;
        PlaceCells();
    }

    private void Update()
    {
        GetInput();

        if (isPlaying)
        {
            if (timer >= 1)
            {
                timer = 0;
                CountNeighbors();
                ControlPopulation();
                CountPopulation(false);
                generation++;

                if (onPopulationUpdate != null)
                    onPopulationUpdate.Invoke();
            }
            timer += Time.deltaTime * speed;
        }
    }

    public void ChangeSize(int size)
    {
        int xCount = 1920 / size;
        int yCount = 1080 / size;

        int cameraSize = yCount / 2;
        float cameraX = xCount / 2 - 0.5f;
        float cameraY = yCount / 2 - 0.5f;
        mainCamera.orthographicSize = cameraSize;
        mainCamera.transform.position = new Vector3(cameraX, cameraY, mainCamera.transform.position.z);

        Debug.Log("Screen: " + SCREEN_WIDTH + " " + SCREEN_HEIGHT);

        DestroyCells();

        SCREEN_HEIGHT = yCount;
        SCREEN_WIDTH = xCount;
        grid = new Cell[SCREEN_WIDTH, SCREEN_HEIGHT];
        Debug.Log("Screen: " + SCREEN_WIDTH + " " + SCREEN_HEIGHT);

        PlaceCells();

        if (onSizeChanged != null)
            onSizeChanged.Invoke();
    }

    public void CountPopulation(bool immediatePopulationUpdate)
    {
        liveCells = 0;
        for (int y = 0; y < SCREEN_HEIGHT; y++)
        {
            for (int x = 0; x < SCREEN_WIDTH; x++)
            {
                Cell cell = grid[x, y];

                if (cell.isAlive)
                    liveCells++;
            }
        }

        if (immediatePopulationUpdate)
            if (onPopulationUpdate != null)
                onPopulationUpdate.Invoke();
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;

        if (onSpeedChanged != null)
            onSpeedChanged.Invoke();
    }

    public float GetSpeed()
    {
        return speed;
    }

    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PlayPause();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            PlaceCells();
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            ClearCells();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SetSpeed(speed + 5);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            SetSpeed(speed - 5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeSize(10);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeSize(12);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeSize(15);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeSize(20);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ChangeSize(30);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            ChangeSize(60);
        }
    }

    public int GetGeneration()
    {
        return generation;
    }

    public int GetPopulation()
    {
        return liveCells;
    }

    public void PlayPause()
    {
        isPlaying = !isPlaying;
    }

    void DestroyCells()
    {
        foreach (Transform child in cellContainer)
            Destroy(child.gameObject);

        Debug.Log("AAAAAA");
    }

    void PlaceCells()
    {
        for (int y = 0; y < SCREEN_HEIGHT; y++)
        {
            for (int x = 0; x < SCREEN_WIDTH; x++)
            {
                Cell cell = Instantiate(cellPrefab, new Vector2(x, y), Quaternion.identity, cellContainer);
                grid[x, y] = cell;
            }
        }
    }

    public void RandomizeCells()
    {
        liveCells = 0;
        generation = 1;
        for (int y = 0; y < SCREEN_HEIGHT; y++)
        {
            for (int x = 0; x < SCREEN_WIDTH; x++)
            {
                Cell cell = grid[x, y];

                cell.SetAlive(RandomLife());

                if (cell.isAlive)
                    liveCells++;
            }
        }

        generation = 1;

        if (onPopulationUpdate != null)
            onPopulationUpdate.Invoke();
    }

    public void ClearCells()
    {
        for (int y = 0; y < SCREEN_HEIGHT; y++)
        {
            for (int x = 0; x < SCREEN_WIDTH; x++)
            {
                Cell cell;
                cell = grid[x, y];
                cell.SetAlive(false);
            }
        }

        liveCells = 0;
        generation = 1;

        if (onPopulationUpdate != null)
            onPopulationUpdate.Invoke();

        isPlaying = false;
    }

    void CountNeighbors()
    {
        for (int y = 0; y < SCREEN_HEIGHT; y++)
        {
            for (int x = 0; x < SCREEN_WIDTH; x++)
            {
                int numNeighbors = 0;

                //North
                if (y + 1 < SCREEN_HEIGHT && grid[x, y + 1].isAlive)
                    numNeighbors++;

                //East
                if (x + 1 < SCREEN_WIDTH && grid[x + 1, y].isAlive)
                    numNeighbors++;

                //West
                if (x - 1 >= 0 && grid[x - 1, y].isAlive)
                    numNeighbors++;

                //South
                if (y - 1 >= 0 && grid[x, y - 1].isAlive)
                    numNeighbors++;

                //North East
                if (y + 1 < SCREEN_HEIGHT && x + 1 < SCREEN_WIDTH && grid[x + 1, y + 1].isAlive)
                    numNeighbors++;

                //North West
                if (y + 1 < SCREEN_HEIGHT && x - 1 >= 0 && grid[x - 1, y + 1].isAlive)
                    numNeighbors++;

                //South East
                if (y - 1 >= 0 && x + 1 < SCREEN_WIDTH && grid[x + 1, y - 1].isAlive)
                    numNeighbors++;

                //South West
                if (y - 1 >= 0 && x - 1 >= 0 && grid[x - 1, y - 1].isAlive)
                    numNeighbors++;

                grid[x, y].numNeighbors = numNeighbors;
            }
        }
    }

    void ControlPopulation()
    {
        for (int y = 0; y < SCREEN_HEIGHT; y++)
        {
            for (int x = 0; x < SCREEN_WIDTH; x++)
            {
                //Any live cell with fewer than two live neighbours dies, as if by underpopulation.
                //Any live cell with two or three live neighbours lives on to the next generation.
                //Any live cell with more than three live neighbours dies, as if by overpopulation.
                //Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.

                Cell cell = grid[x, y];

                if (cell.isAlive)
                {
                    if (cell.numNeighbors != 2 && cell.numNeighbors != 3)
                        cell.SetAlive(false);
                }
                else
                {
                    if (cell.numNeighbors == 3)
                        cell.SetAlive(true); ;
                }
            }
        }
    }

    public int GetScreenWidth()
    {
        return SCREEN_WIDTH;
    }

    public int GetScreenHeight()
    {
        return SCREEN_HEIGHT;
    }

    bool RandomLife()
    {
        int rand = Random.Range(0, 100);
        return (rand > 75) ? true : false;
    }
}
