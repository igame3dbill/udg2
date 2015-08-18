﻿using UnityEngine;
using System.Collections;
using System.IO;

public class MainGameManager : MainGameInit
{

    // global score 
    private int currentScore;

    // global text box update
    private string guiText;

    // UDGInit.lua to C# 07 25 2015
    // -- UDGInit with copies from RTS Init 01 03 09; 
    // 
    // 
    /* turning things off to  debug 08 27 2015
     * 
     /*this should inherit from MainGameInit
    private string gameroot;//= Application.dataPath;
    private string levelPath;// = Application.dataPath  + "/Data/Levels/"; 
    private string gameRoot;// = gameroot; //break and fix this all at once ...later;
    private string currentLevel;// = Application.loadedLevelName;
    */

    /*this is old udeadgame Lua to C# should ineherit from the MainGameInit or Maybe Sub Manager scripts? Tracking.cs etc perhaps?
    private bool doShadows = true;
    private string masterAnimSource = "mcTrueBones.wtf";
    private string selectionRectModel = "selectionRect.wtf";
    private string statusString = "";
    private int trackingLight = 3;
    private int objCycle = 2;
    private int teamSelect = 0;
    private int lastClick = 0;
    private float doubleClickInterval = 0.4f;

    private int cameraSpeed = 5;
    private float velocityClamp = 1.16f;
    private bool levelIsLit = false;
    private bool handleUDGAI = true;
    private bool trackLightEnabled = false;
    private string nextUDGLevel = null;
    private bool CAPSLOCKKEY = false;

    // -- this tracks the screamer with  light 5; 
    private GameObject[] screamerObject;
    private Vector3[] screamVector3;
    //ScreamX,ScreamY,gScreamZ = new Transform.position(); // nil,nil,nil; 
    private Vector3[] trackedPosition;

    // -- counts how long the gun flare is on; 
    private int gunLightCount = 0;

    private bool reloadLevel = false; //-- added 02 08 2009 ; 
    private float startPositionTimer = 0.0f;

    // -- added 02 26 2009 keyboard teleport; 
    private bool teleportTimerActive = false;
    private float teleportTime = 0.0f;

    private int trackedObject = 3; //TrackedPosition.x,TrackedPosition.y,TrackedPosition.z = 0,0,0; 
    private bool displayLoading = false;
    private bool restartLevel = false;
    private float timeBeforeLoad = 0.0f;
    private int loadingTextBoxNum = 1;
    private string loadingBoxMessage = "";
    private float textualTimer = 0.0f;
    private int currentText = 1;
    private bool hideHUD = false;
    private bool wonThisLevel = false;
  

    //public string CurrentLevel = Application.loadedLevelName; //local tCurrentLevel=ig3dGetLevels()
    //--write currentlevel.lua to UDG folder
    */
    private char Quote = '\"';
    //this is from a tutorial, it's a working example while I breakthings
    public static MainGameManager instance; //local tCurrentLevel=ig3dGetLevelNames()

    public GameObject[] zombies;
    public GameObject[] humans;
    struct PopulationData
    {
        uint m_poolSize;
        uint m_targetCount;
        float m_spawnInterval;
        float m_lastSpawnTime;
        float m_time;
        string m_factionTag;
        string m_spawnPointTag;
        GameObject[] m_entities;
        GameObject[] m_prefabs;

        public void setup(uint poolSize, uint targetCount, float spawnInterval, string factionTag, GameObject[] prefabs, string spawnPointTag)
        {
            m_poolSize = poolSize;
            m_targetCount = targetCount;
            m_spawnInterval = spawnInterval;
            m_factionTag = factionTag;
            m_spawnPointTag = spawnPointTag;
            m_prefabs = prefabs;
            m_lastSpawnTime = -m_spawnInterval;
            m_time = 0.0f;
            if (prefabs.Length == 0)
            {
                print("There are no prefabs, spawning is disabled");
                m_poolSize = 0;
            }
        }

        public void update(float timeStep)
        {
            m_time += timeStep;
            m_entities = GameObject.FindGameObjectsWithTag(m_factionTag);

            if (m_entities.Length < m_targetCount && m_poolSize != 0u)
            {
                if (m_time - m_lastSpawnTime >= m_spawnInterval)
                {
                    m_lastSpawnTime += m_spawnInterval;
                    GameObject[] spawnPoints = GameObject.FindGameObjectsWithTag(m_spawnPointTag);
                    if (spawnPoints.Length == 0)
                    {
                        print("No spawn points with tag '" + m_spawnPointTag + "' found in scene! Cannot spawn any '" + m_factionTag + "s'!");
                    }
                    else
                    {
                        GameObject spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                        Instantiate(m_prefabs[Random.Range(0, m_prefabs.Length)], spawnPoint.transform.position, spawnPoint.transform.rotation);
                        --m_poolSize;
                    }

                }
            }
        }
    }

    private PopulationData m_zombies;
    private PopulationData m_humans;


    void Awake()
    {
        instance = this;
        gameroot = Application.dataPath;
        gameRoot = gameroot; // fix them all at once later.
        levelPath = Application.dataPath + "/Data/Levels/";
        currentLevel = Application.loadedLevelName;


    }

    // OnGUI is auto updating.

    public void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 700, 200), guiText);

    }

    public void AdjustScore(int num)
    {
        currentScore += num;
    }
    // end working tutorial


    //here there be Bill code.

    private void Start()
    {

        guiText = "Data Path: " + gameRoot + "\n" + "Scene " + currentLevel + "\n";
        if (currentLevel != "UDGInstructions")
        {
            writeCurrentLevel(currentLevel);
        }

        m_humans.setup(2u, 2u, 4.0f, "Human", humans, "SpawnPoint_Human");
        m_zombies.setup(2u, 2u, 4.0f, "Zombie", zombies, "SpawnPoint_Zombie");

    }

    public void Update()
    {
        m_humans.update(Time.deltaTime);
        m_zombies.update(Time.deltaTime);
    }

    // this saves the current level , in theory, back when it was a text file.
    public void writeCurrentLevel(string currentLevel)
    {// Add some text to the file.
        using (StreamWriter currentLevelFile = new StreamWriter(gameroot + "/Data/Levels/UDG/" + currentLevel + ".txt"))
        {
            string thislevelout = "currentLevel =" + Quote + currentLevel + Quote + "\n";
            currentLevelFile.Write(thislevelout);
            if (System.IO.File.Exists(gameroot + "/Data/Levels/UDG/" + currentLevel + ".txt"))
            {
                print(thislevelout); //do stuff
            }
        }


    }


    //end MainGameManager
}