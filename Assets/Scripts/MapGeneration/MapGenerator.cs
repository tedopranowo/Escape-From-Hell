using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private class DoorPlacement
    {
        public Direction m_dir;
        public Quaternion m_quat;
        public Door.Orientation m_ori;
        public int m_offset;

        public DoorPlacement(Direction dir, Quaternion quat, Door.Orientation ori, int offset)
        {
            m_dir = dir;
            m_quat = quat;
            m_ori = ori;
            m_offset = offset;
        }
    }

    [System.Serializable]
    private struct GeneratedRoomData
    {
        public Room m_style;

        public int m_min;

        public int m_max;

        public int m_probability;

        [System.NonSerialized] public int m_count;
    }

    // Tunable values
    [SerializeField]
    private int m_rows = 10;
    [SerializeField]
    private int m_cols = 10;
    [SerializeField]
    private int m_objectsPerRoom = 5;
    [SerializeField] [Tooltip("True: the map will expand in all directions from the starting room.  False: the map will expand in a more linear direction.")]
    bool outwardExpand = true;
    [SerializeField]
    private int m_maxKeys = 3;

    // Room data
    [SerializeField]
    private Room m_startingRoom;
    [SerializeField]
    private Room m_finalRoom;
    [SerializeField]
    private GeneratedRoomData[] m_generatedRooms;

    // Required GameObjects
    [SerializeField]
    private GameObject m_door;
    [SerializeField]
    private GameObject m_player;
    [SerializeField]
    private GameObject m_key;
    [SerializeField]
    private GameObject m_lockedDoor;
    [SerializeField]
    private GameObject m_goal;

    // Variables
    private int m_startIndex;
    private int m_endIndex = -1;
    private int m_mapSize;
    private int m_roomCount = 0;
    private int m_farthestPath = 0;
    private int m_maxItemDrops = 5;

    // Permutations of Room Expansion
    int[,] m_roomPermutations = new int[,]
    {
        // 0, 1, 2, 3
        { 0, 1, 2, 3 },
        { 3, 0, 1, 2 },
        { 2, 3, 0, 1 },
        { 1, 2, 3, 0 },

        // 0, 1, 3, 2
        { 0, 1, 3, 2 },
        { 2, 0, 1, 3 },
        { 3, 2, 0, 1 },
        { 1, 3, 2, 0 },

        // 0, 2, 1, 3
        { 0, 2, 1, 3 },
        { 3, 0, 2, 1 },
        { 1, 3, 0, 2 },
        { 2, 1, 3, 0 },

        // 0, 2, 3, 1
        { 0, 2, 3, 1 },
        { 1, 0, 2, 3 },
        { 3, 1, 0, 2 },
        { 2, 3, 1, 0 },

        // 0, 3, 1, 2
        { 0, 3, 1, 2 },
        { 2, 0, 3, 1 },
        { 1, 2, 0, 3 },
        { 3, 1, 2, 0 },

        // 0, 3, 2, 1
        { 0, 3, 2, 1 },
        { 1, 0, 3, 2 },
        { 2, 1, 0, 3 },
        { 3, 2, 1, 0 }
    };

    [System.Serializable]
    struct RandObj
    {
        [SerializeField]
        public GameObject m_objToSpawn;
        [SerializeField]
        [Range(0, 100)]
        public float m_probability;
    }

    [SerializeField]
    private RandObj[] m_randomObjects;

    public enum Direction
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3,
        DirectionCount = 4,
        None = 5
    }

    private Dictionary<int, RoomGenerator> m_roomGenMap = new Dictionary<int, RoomGenerator>();
    Dictionary<int, GameObject> m_map = new Dictionary<int, GameObject>();

    private List<Outlet> m_outlets = new List<Outlet>();
    private Queue<RoomGenerator> toExpand = new Queue<RoomGenerator>();
    private List<int> m_finalRooms = new List<int>();

    /// <summary>
    /// Return the minimum total number of room
    /// </summary>
    private int minRoomCount
    {
        get
        {
            int min = 0;

            foreach(GeneratedRoomData grd in m_generatedRooms)
            {
                min += grd.m_min;
            }

            //We add 1 to include the starting room
            return min + 1;
        }
    }

    /// <summary>
    /// Return the maximum total number of room
    /// </summary>
    private int maxRoomCount
    {
        get
        {
            int max = 0;

            foreach(GeneratedRoomData grd in m_generatedRooms)
            {
                max += grd.m_max;
            }

            //We add 1 to include the starting room
            return max + 1;
        }
    }


    // Use this for initialization
    void Start()
    {
        ValidateTunableValues();
        CreateRooms();
        DrawMap();
        SetupSpawnLocations();
    }

    //-------------------------------------------------------------------
    // This checks the tunable values and makes sure that they are valid
    // each other and the requirements to generate the dungeon.
    //-------------------------------------------------------------------
    void ValidateTunableValues()
    {
        int minRoom = minRoomCount;
        int maxRoom = maxRoomCount;

        // calculate maximum number of possible rooms
        m_mapSize = m_rows * m_cols;

        // the minimum number or rooms cannot be greater than the map size
        if (minRoom > m_mapSize)
            minRoom = m_mapSize;

        // the maximum number of rooms cannot be greater than the map size
        // (technically it's okay, but let's be explicit)
        if (maxRoom > m_mapSize)
            maxRoom = m_mapSize;

        // the minimum number of rooms must be less than or equal to max
        if (minRoom > maxRoom)
            minRoom = maxRoom;

        // The maximum number of objects per room is 5
        if (m_objectsPerRoom > m_maxItemDrops)
            m_objectsPerRoom = m_maxItemDrops;

        else if (m_objectsPerRoom <= 0)
            m_objectsPerRoom = 1;

        // The maximum number of keys are 3
        if (m_maxKeys > 3)
            m_maxKeys = 3;
    }

    //-------------------------------------------------------------------
    // This generates the rooms in the dungeon
    //-------------------------------------------------------------------
    void CreateRooms()
    {
        int randomStart = Random.Range(0, m_mapSize);
        m_roomGenMap.Add(randomStart, new RoomGenerator(randomStart));
        ++m_roomCount;

        toExpand.Enqueue(m_roomGenMap[randomStart]);

        // Outward expand
        if (outwardExpand)
        {
            while (toExpand.Count > 0)
            {
                RoomGenerator roomGen = toExpand.Dequeue();
                Expand(roomGen.GetIndex());
            }
        }

        // Linear expand
        else
        {
            Expand(randomStart, true);
        }

        Dijkstra(randomStart);

        PrimsMST(randomStart);

        m_startIndex = randomStart;
    }

    //-------------------------------------------------------------------
    // This expands the room at the index given by linking to existing rooms
    // or creating additional rooms and expanding the map
    //      mapIndex: the index of the room's location in the map
    //      start: whether or not this is the starting room of the map
    //-------------------------------------------------------------------
    void Expand(int mapIndex, bool start = false)
    {
      //  if (m_minRooms > m_roomCount)
            ExpandAllAvailable(mapIndex);
      //  else
      //      RandomExpand(mapIndex, start);
    }

    //-------------------------------------------------------------------
    // This expands the room at the index in all available directions.
    //      mapIndex: the index of the room's location in the map
    //-------------------------------------------------------------------
    void ExpandAllAvailable(int mapIndex)
    {
        int permutationIndex = Random.Range(0, 24);

        for (int i = 0; i < (int)Direction.DirectionCount; ++i)
        {
            Direction dir = (Direction)m_roomPermutations[permutationIndex, i];
            if (m_roomGenMap[mapIndex].CheckAvailability(dir))
                LinkRooms(mapIndex, dir);
        }
    }

    //-------------------------------------------------------------------
    // This expands the room at the index in a random number of directions.
    //      mapIndex: the index of the room's location in the map
    //      start: whether or not this is the starting room of the map
    //-------------------------------------------------------------------
    void RandomExpand(int mapIndex, bool start = false)
    {
        // The maximum number of outlets to attempt to create
        int maxOutlets;

        if (!start)
            maxOutlets = Random.Range(0, (int)Direction.DirectionCount);
        else
            maxOutlets = (int)Direction.DirectionCount;

        if (maxOutlets == 0)
            return;

        // A random direction
        int randIndex = Random.Range(0, (int)Direction.DirectionCount);

        // loop through all the directions and create outlets
        for (int outletIndex = 0; outletIndex < (int)Direction.DirectionCount; ++outletIndex)
        {
            // if we are only creating one outlet, skip the outlet if it's not the outlet at randIndex direction
            if (maxOutlets == 1 && outletIndex != randIndex)
                continue;

            // If we are creating 2 outlets, we need to skip an outlet (randIndex direction)
            else if (maxOutlets == 2 && outletIndex == randIndex)
                continue;

            // Attempt to create a room in the given direction
            Debug.Assert(mapIndex >= 0 && mapIndex < m_mapSize);
            LinkRooms(mapIndex, (Direction)outletIndex);
        }
    }

    //-------------------------------------------------------------------
    // This checks if 2 rooms can be linked and then links them.
    //      mapIndex: the index of the room's location in the map
    //      dir: the direction of the room we want to link to
    //      return: whether we successfully linked a room
    //-------------------------------------------------------------------  
    bool LinkRooms(int mapIndex, Direction dir)
    {
        Debug.Assert(m_roomGenMap.ContainsKey(mapIndex));
        if (m_roomGenMap[mapIndex].CheckAvailability(dir))
        {
            return Link(mapIndex, dir);
        }
        return false;
    }

    //-------------------------------------------------------------------
    // This links 2 rooms together.
    //      mapIndex: the index of the room's location in the map
    //      dir: the direction of the room we want to link to
    //      TODO: this return value seems off, need to think about refactoring this
    //      return: whether we created a new room while linking
    //------------------------------------------------------------------- 
    bool Link(int mapIndex, MapGenerator.Direction dir)
    {
        // the offset from the mapIndex to reach the other room
        int offset = GetOffsetDifference(mapIndex, dir);

        bool createdNewRoom = false;
        if (offset != 0)
        {
            // The index to the room we are linking to
            int linkRoomIndex = mapIndex + offset;
            Debug.Assert(linkRoomIndex >= 0 && linkRoomIndex < m_mapSize);

            // The room hasn't been created yet
            if (!m_roomGenMap.ContainsKey(linkRoomIndex))
            {
                // If we've reached the maximum number of rooms allowed
                // we do NOT create any additional rooms
                if (maxRoomCount <= m_roomCount)
                    return false;

                // Create the room
                m_roomGenMap.Add(linkRoomIndex, new RoomGenerator(linkRoomIndex));
                createdNewRoom = true;
                ++m_roomCount;
            }

            // Link the rooms
            Debug.Assert(m_roomGenMap.ContainsKey(mapIndex));
            Debug.Assert(m_roomGenMap.ContainsKey(linkRoomIndex));

            m_outlets.Add(new Outlet(m_roomGenMap[mapIndex], m_roomGenMap[linkRoomIndex], dir));

            if (createdNewRoom)
            {
                // Outward Expand
                if (outwardExpand)
                    toExpand.Enqueue(m_roomGenMap[linkRoomIndex]);

                // Linear Expand
                else
                    Expand(linkRoomIndex);
            }
        }
        return createdNewRoom;
    }

    //-------------------------------------------------------------------
    // Checks if a room has a valid adjacent room
    //		mapIndex: the index of the room
    //		dir: the direction to the adjacent room we are checking for
    //			validity.
    //		return: the index offset to get to the room.  0 means no 
    //			valid room.
    //------------------------------------------------------------------- 
    int GetOffsetDifference(int mapIndex, Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                if ((mapIndex + m_cols) < m_mapSize)
                    return m_cols;
                break;
            case Direction.Down:
                if ((mapIndex - m_cols) > 0)
                    return -m_cols;
                break;
            case Direction.Left:
                if (mapIndex % m_cols != 0)
                    return -1;
                break;
            case Direction.Right:
                if (mapIndex % m_cols != m_cols - 1)
                    return 1;
                break;
            default:
                Debug.Assert(false);
                break;
        }

        // if we return 0 then invalid direction
        return 0;
    }

    //-------------------------------------------------------------------
    // Prim's Minimum Spanning Tree
    //-------------------------------------------------------------------
    void PrimsMST(int startIndex)
    {
        // We set the starting rooms key to 0, so we extract it from the
        // min-priority queue first.
        m_roomGenMap[startIndex].Key = 0;

        // Create pseudo min-priority queue
        List<int> rooms = new List<int>();
        for (int i = 0; i < m_mapSize; ++i)
        {
            // only add valid room indices 
            if (m_roomGenMap.ContainsKey(i))
                rooms.Add(i);
        }

        // Continue growing MST until queue is empty
        while (rooms.Count > 0)
        {
            int minKey = 100000;
            int indexToCheck = -1;
            int toRemove = -1;

            // Extract minimum value from queue
            for (int i = 0; i < rooms.Count; ++i)
            {
                if (m_roomGenMap[rooms[i]].Key < minKey)
                {
                    minKey = m_roomGenMap[rooms[i]].Key;
                    indexToCheck = rooms[i];
                    toRemove = i;
                }
            }
            rooms.RemoveAt(toRemove);

            int adjRoomIndex;
            Debug.Assert(m_roomGenMap.ContainsKey(indexToCheck), "Map does NOT have index.");

            bool notAParent = true;

            // Check Up
            if (indexToCheck + m_cols < m_mapSize && !m_roomGenMap[indexToCheck].CheckAvailability(Direction.Up))
            {
                adjRoomIndex = indexToCheck + m_cols;
                if (rooms.Contains(adjRoomIndex) && m_roomGenMap[indexToCheck].Key < m_roomGenMap[adjRoomIndex].Key)
                {
                    m_roomGenMap[adjRoomIndex].Parent = m_roomGenMap[indexToCheck];
                    m_roomGenMap[adjRoomIndex].ParDir = Direction.Down;
                    m_roomGenMap[adjRoomIndex].Key = m_roomGenMap[indexToCheck].Key;
                    notAParent = false;
                }
            }
            // Check Down
            if (indexToCheck - m_cols >= 0 && !m_roomGenMap[indexToCheck].CheckAvailability(Direction.Down))
            {
                adjRoomIndex = indexToCheck - m_cols;
                if (rooms.Contains(adjRoomIndex) && m_roomGenMap[indexToCheck].Key < m_roomGenMap[adjRoomIndex].Key)
                {
                    m_roomGenMap[adjRoomIndex].Parent = m_roomGenMap[indexToCheck];
                    m_roomGenMap[adjRoomIndex].ParDir = Direction.Up;
                    m_roomGenMap[adjRoomIndex].Key = m_roomGenMap[indexToCheck].Key;
                    notAParent = false;
                }
            }
            // Check Left
            if (indexToCheck % m_cols != 0 && !m_roomGenMap[indexToCheck].CheckAvailability(Direction.Left))
            {
                adjRoomIndex = indexToCheck - 1;
                if (rooms.Contains(adjRoomIndex) && m_roomGenMap[indexToCheck].Key < m_roomGenMap[adjRoomIndex].Key)
                {
                    m_roomGenMap[adjRoomIndex].Parent = m_roomGenMap[indexToCheck];
                    m_roomGenMap[adjRoomIndex].ParDir = Direction.Right;
                    m_roomGenMap[adjRoomIndex].Key = m_roomGenMap[indexToCheck].Key;
                    notAParent = false;
                }
            }
            // Check Right
            if ((indexToCheck + 1) % m_cols != 0 && !m_roomGenMap[indexToCheck].CheckAvailability(Direction.Right))
            {
                adjRoomIndex = indexToCheck + 1;
                if (rooms.Contains(adjRoomIndex) && m_roomGenMap[indexToCheck].Key < m_roomGenMap[adjRoomIndex].Key)
                {
                    m_roomGenMap[adjRoomIndex].Parent = m_roomGenMap[indexToCheck];
                    m_roomGenMap[adjRoomIndex].ParDir = Direction.Left;
                    m_roomGenMap[adjRoomIndex].Key = m_roomGenMap[indexToCheck].Key;
                    notAParent = false;
                }
            }

            if (notAParent)
            {
                CompareFinalRoom(indexToCheck);
            }
        }
    }

    void CompareFinalRoom(int index)
    {
        if (m_finalRooms.Count < m_maxKeys + 1)
            m_finalRooms.Add(index);
        else
        {
            int minPath = 100000;
            int closest = -1;
            // find the room closest to the start
            foreach (int i in m_finalRooms)
            {
                if (m_roomGenMap[i].path < minPath)
                {
                    closest = i;
                    minPath = m_roomGenMap[i].path;
                }
            }

            if (closest != -1 && m_roomGenMap[index].path > minPath)
            {
                m_finalRooms.Remove(closest);
                m_finalRooms.Add(index);
            }
        }
    }

    //-------------------------------------------------------------------
    // Dijkstra implementation
    //-------------------------------------------------------------------
    void Dijkstra(int startIndex)
    {
        InitializeSingleSource(startIndex);

        // Create queue of all rooms(vertices)
        List<int> rooms = new List<int>();
        for (int i = 0; i < m_mapSize; ++i)
        {
            // only add valid room indices 
            if (m_roomGenMap.ContainsKey(i))
                rooms.Add(i);
        }

        // Create shortest path
        while (rooms.Count != 0)
        {
            // check for shortest path
            int shortestPath = 100000;
            int indexToCheck = -1;
            foreach (int index in rooms)
            {
                if (m_roomGenMap[index].path < shortestPath)
                {
                    shortestPath = m_roomGenMap[index].path;
                    indexToCheck = index;
                }
            }

            // Relax edges and check for shortest path
            // Relax right
            if ((indexToCheck + 1) % m_cols != 0 && !m_roomGenMap[indexToCheck].CheckAvailability(Direction.Right))
                Relax(indexToCheck, indexToCheck + 1);
            // Relax down
            if (indexToCheck - m_cols >= 0 && !m_roomGenMap[indexToCheck].CheckAvailability(Direction.Down))
                Relax(indexToCheck, indexToCheck - m_cols);
            // Relax up (remember origin is at BOTTOM-left of screen)
            if (indexToCheck + m_cols < m_mapSize && !m_roomGenMap[indexToCheck].CheckAvailability(Direction.Up))
                Relax(indexToCheck, indexToCheck + m_cols);
            // Relax left
            if (indexToCheck % m_cols != 0 && !m_roomGenMap[indexToCheck].CheckAvailability(Direction.Left))
                Relax(indexToCheck, indexToCheck - 1);

            // Update the farthest path
            if (m_roomGenMap[indexToCheck].path > m_farthestPath)
            {
                m_farthestPath = m_roomGenMap[indexToCheck].path;
                m_endIndex = indexToCheck;
            }

            rooms.Remove(indexToCheck);
        }
    }

    void InitializeSingleSource(int startIndex)
    {
        m_roomGenMap[startIndex].path = 0;
    }

    void Relax(int pointA, int pointB)
    {
        Debug.Assert(m_roomGenMap.ContainsKey(pointA));
        Debug.Assert(m_roomGenMap.ContainsKey(pointB));

        // Every edge(outlet) has a weight of 1
        int weight = 1;
        if (m_roomGenMap[pointB].path > m_roomGenMap[pointA].path + weight)
            m_roomGenMap[pointB].path = m_roomGenMap[pointA].path + weight;
    }

    void SortFinalRooms()
    {
        List<int> buffer = new List<int>();
        
        while (m_finalRooms.Count > 0)
        {
            int minpath = 100000;
            int closest = -1;

            // find the room closest to the start
            for (int i = 0; i < m_finalRooms.Count; ++i)
            {
                if (m_roomGenMap[m_finalRooms[i]].path < minpath)
                {
                    closest = m_finalRooms[i];
                    minpath = m_roomGenMap[m_finalRooms[i]].path;
                }
            }

            buffer.Add(closest);
            m_finalRooms.Remove(closest);
        }

        m_finalRooms = buffer;
    }

    //-------------------------------------------------------------------
    // This instantiates the actual game objects into the game world
    //-------------------------------------------------------------------  
    void DrawMap()
    {
        Vector3 rotationAxis = new Vector3(0, 0, 1);

        SortFinalRooms();

        DoorPlacement up = new DoorPlacement(Direction.Up, Quaternion.identity, Door.Orientation.Horizontal, m_cols);
        DoorPlacement down = new DoorPlacement(Direction.Down, Quaternion.AngleAxis(180, rotationAxis), Door.Orientation.Horizontal, -m_cols);
        DoorPlacement left = new DoorPlacement(Direction.Left, Quaternion.AngleAxis(90, rotationAxis), Door.Orientation.Vertical, -1);
        DoorPlacement right = new DoorPlacement(Direction.Right, Quaternion.AngleAxis(270, rotationAxis), Door.Orientation.Vertical, 1);

        //-------------------------------------------------------------------
        // Generated List of room style that's going to be used
        //-------------------------------------------------------------------
        //Track the total count of room style
        int totalRoom = 1;          //We start from 1 because we need to include the starting rooom
        int totalProbability = 0;

        //Set the room count to the minimum number
        for (int i = 0; i < m_generatedRooms.Length; ++i)
        {
            //Set the room count to minimum
            m_generatedRooms[i].m_count = m_generatedRooms[i].m_min;
            totalRoom += m_generatedRooms[i].m_count;

            //Count the total probability (For randomization)
            totalProbability += m_generatedRooms[i].m_probability;
        }

        //Add a random room style until it reaches the number we want
        while(totalRoom < m_roomGenMap.Count)
        {
            //Choose a random room
            int randomValue = Random.Range(0, totalProbability);
            int roomIndex = -1;

            while (randomValue >= 0)
            {
                ++roomIndex;
                randomValue -= m_generatedRooms[roomIndex].m_probability;
            }

            //If the room hasn't reached it's max limit
            if (m_generatedRooms[roomIndex].m_count < m_generatedRooms[roomIndex].m_max)
            {
                //Add it as a new room style
                ++m_generatedRooms[roomIndex].m_count;
                ++totalRoom;
            }
        }

        //Store list of room style into an array of index
        int[] roomIndexArray = new int[totalRoom];
        int curIndex = 0;

        for (int i=0; i<m_generatedRooms.Length; ++i)
        {
            for (int j=0; j<m_generatedRooms[i].m_count; ++j)
            {
                roomIndexArray[curIndex] = i;
                ++curIndex;
            }
        }

        //Shuffle the array
        for (int i=roomIndexArray.Length - 1; i>0; --i)
        {
            int swapIndex = Random.Range(0, i);

            //Swap
            int temp = roomIndexArray[swapIndex];
            roomIndexArray[swapIndex] = roomIndexArray[i];
            roomIndexArray[i] = temp;
        }

        curIndex = 0;
        foreach (KeyValuePair<int, RoomGenerator> kvp in m_roomGenMap)
        {
            //-------------------------------------------------------------------
            // Draw Rooms
            //-------------------------------------------------------------------
            Room room = m_startingRoom;

            // Set the final room
            if (kvp.Key == m_endIndex)
            {
                Debug.Assert(m_finalRoom != null, "Boss room cannot be empty!");
                room = m_finalRoom;
            }
            // If it is neither the starting room nor the final room, 
            // use the room style from the shuffled array
            else if (kvp.Key != m_startIndex)
            {
                int roomIndex = roomIndexArray[curIndex];
                room = m_generatedRooms[roomIndex].m_style;

                ++curIndex;
            }

            // The cell of the room
            int cellX = kvp.Key % m_cols;
            int cellY = kvp.Key / m_cols;

            // The dimensions of the room
            float floorWidth = room.GetComponent<SpriteRenderer>().bounds.size.x;
            float floorHeight = room.GetComponent<SpriteRenderer>().bounds.size.y;

            // The position of the room in world space
            float renderX = cellX * floorWidth;
            float renderY = cellY * floorHeight;

            // Create the room
            GameObject newRoom = Instantiate(room.gameObject, new Vector3(renderX, renderY, 1), Quaternion.identity);
            newRoom.SetActive(false);
            m_map.Add(kvp.Value.GetIndex(), newRoom);

            // Setup the starting room with the player and have camera focus on it
            if (kvp.Key == m_startIndex)
            {
                FindObjectOfType<CameraManager>().targetRoom = newRoom.GetComponent<SpriteRenderer>();

                if (SceneManagerSingleton.instance.hasPlayer == false)
                {
                    Debug.Log("Creating new player");
                    SceneManagerSingleton.instance.player = Instantiate(m_player, new Vector3(renderX, renderY, 1), Quaternion.identity);
                    SceneManagerSingleton.instance.hasPlayer = true;
                }
                else
                {
                    Debug.Log("Teleporting old player");
                    SceneManagerSingleton.instance.player.transform.position = new Vector3(renderX, renderY, 1);
                }
            }

            // find the room closest to the start
            for (int i = 0; i < m_finalRooms.Count; ++i)
            {
                if (kvp.Key == m_finalRooms[i])
                {
                    if (i == m_finalRooms.Count - 1)
                    {
                        Instantiate(m_goal, new Vector3(renderX, renderY, 1), Quaternion.identity, newRoom.transform);
                    }
                    else
                    {
                        GameObject keyInstance = Instantiate(m_key, new Vector3(renderX, renderY, 1), Quaternion.identity, newRoom.transform);
                        keyInstance.GetComponent<Key>().SetType((Key.Type)i);
                    }
                }
            }
        }

        //-------------------------------------------------------------------
        // Draw Doors
        //-------------------------------------------------------------------
        foreach (KeyValuePair<int, RoomGenerator> kvp in m_roomGenMap)
        {
            // The cell of the room
            int cellX = kvp.Key % m_cols;
            int cellY = kvp.Key / m_cols;

            // The dimensions of the room
            //[TEDO]:
            //I temporarily use the room index 0 as reference. This is NOT the correct solution
            float floorWidth = m_generatedRooms[0].m_style.GetComponent<SpriteRenderer>().bounds.size.x;
            float floorHeight = m_generatedRooms[0].m_style.GetComponent<SpriteRenderer>().bounds.size.y;

            // The position of the room in world space
            float renderX = cellX * floorWidth;
            float renderY = cellY * floorHeight;

            DoorPlacement toPlace = null;
            DoorPlacement adjoiningDoor = null;

            float doorX = renderX;
            float doorY = renderY;
            float adjX = renderX;
            float adjY = renderY;

            if (kvp.Key == m_startIndex)
                continue;

            Debug.Assert(kvp.Value.Parent != null && kvp.Value.ParDir == Direction.None);
            
            if (kvp.Value.ParDir == Direction.Up)
            {
                toPlace = up;
                adjoiningDoor = down;
                doorX = renderX;
                adjX = renderX;
                doorY = renderY + (floorHeight / 2);
                adjY = renderY + (floorHeight / 2);
            }
            else if (kvp.Value.ParDir == Direction.Down)
            {
                toPlace = down;
                adjoiningDoor = up;
                doorX = renderX;
                doorY = renderY - (floorHeight / 2);
                adjY = renderY - floorHeight / 2;
            }
            else if (kvp.Value.ParDir == Direction.Left)
            {
                toPlace = left;
                adjoiningDoor = right;
                doorX = renderX - (floorWidth / 2);
                adjX = renderX - floorWidth / 2;
                doorY = renderY;
                adjY = renderY;

            }
            else if (kvp.Value.ParDir == Direction.Right)
            {
                toPlace = right;
                adjoiningDoor = left;
                doorX = renderX + (floorWidth / 2);
                adjX = renderX + floorWidth / 2;
                doorY = renderY;
                adjY = renderY;
            }

            if (kvp.Key != m_startIndex)
            {
                Debug.Assert(toPlace != null);
                Drawdoor(kvp.Value, new Vector3(doorX, doorY, 1), toPlace.m_quat, toPlace.m_ori, kvp.Key, toPlace.m_offset);
                Drawdoor(kvp.Value.Parent, new Vector3(adjX, adjY, 1), adjoiningDoor.m_quat, adjoiningDoor.m_ori, kvp.Key + toPlace.m_offset, adjoiningDoor.m_offset);
            }
        }
    }

    //-------------------------------------------------------------------
    // This checks if a door is needed in the room at a specific direction.
    //      roomGen: the roomgenerator that the door is part of.
    //      doorPlace: a doorPlace that gives us information about the door
    //      index: the index of the room in the map
    //      x: the x position of the door
    //      y: the y position of the door
    //-------------------------------------------------------------------  
    void Checkdoor(RoomGenerator roomGen, DoorPlacement doorPlace, int index, float x, float y)
    {
        if (!roomGen.CheckAvailability(doorPlace.m_dir))
        {
            Drawdoor(roomGen, new Vector3(x, y, 1), doorPlace.m_quat, doorPlace.m_ori, index, doorPlace.m_offset);
        }
    }

    //-------------------------------------------------------------------
    // This creates the door game object at the correct location and orientation.
    //      roomGen: the roomgenerator that the door is part of
    //      position: the position to spawn the door
    //      quat: the rotation to spawn the door
    //      ori: this determines which direction the door needs to teleport the player
    //      index: the index of the room
    //      offset: the offset to find the index of the room the door leads to
    //-------------------------------------------------------------------  
    void Drawdoor(RoomGenerator roomGen, Vector3 position, Quaternion quat, Door.Orientation ori, int index, int offset)
    {
        GameObject door;
        int i = 1;
        bool success = false;

        // We start i at 1 because, we do NOT want to put the first key behind a locked door.
        for (; i < m_finalRooms.Count; ++i)
        {
            if (index + offset == m_finalRooms[i])
            {
                success = true;
                break;
            }
        }
        if (success)
        {
            door = Instantiate(m_lockedDoor, position, quat, m_map[index].transform);
            door.GetComponent<Door>().type = (Door.Type)i;
        }
        else
        {
            door = Instantiate(m_door, position, quat, m_map[index].transform);
        }

        Door doorComp = door.GetComponent<Door>();

        doorComp.orientation = ori;

        Debug.Assert(m_map.ContainsKey(roomGen.GetIndex()));
        doorComp.curRoom = m_map[roomGen.GetIndex()].GetComponent<SpriteRenderer>();

        Debug.Assert(m_map.ContainsKey(roomGen.GetIndex() + offset), offset);
        doorComp.nextRoom = m_map[roomGen.GetIndex() + offset].GetComponent<SpriteRenderer>();
    }

    //-------------------------------------------------------------------
    // This loops through each room and randomly spawns objects in the room.
    //-------------------------------------------------------------------  
    void SetupSpawnLocations()
    {
        // Loop through every room in the map
        foreach (KeyValuePair<int, GameObject> kvp in m_map)
        {
            if (kvp.Key == m_startIndex)
                continue;

            // Setup the locations where the objects can spawn in the room
            Vector3[] spawnPositions = kvp.Value.GetComponent<Room>().spawnPosition;

            SpawnObjects(spawnPositions, kvp.Value.transform);
        }
    }

 
    //-------------------------------------------------------------------
    // Spawn Objects in the room
    //      spawnPositions: an array of positions to spawn objects
    //      trans: the transform of the object to create
    //-------------------------------------------------------------------  
    void SpawnObjects(Vector3[] spawnPositions, Transform trans)
    {
        // The object to spawn
        GameObject toSpawn;

        // Randomly select an object to spawn at each location
        foreach (Vector3 vec in spawnPositions)
        {
            toSpawn = m_randomObjects[GetRandomObjectFromProbability()].m_objToSpawn;
            if(toSpawn != null)
            {
                Instantiate(toSpawn, vec, Quaternion.identity, trans);
            }
        }
    }
    //-------------------------------------------------------------------
    // Get random object from their probability
    //-------------------------------------------------------------------  
    int GetRandomObjectFromProbability()
    {
        float total = 0;

        for (int i = 0; i < m_randomObjects.Length; i++)
        {
            total += m_randomObjects[i].m_probability;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < m_randomObjects.Length; i++)
        {
            if (randomPoint < m_randomObjects[i].m_probability)
            {
                return i;
            }
            else {
                randomPoint -= m_randomObjects[i].m_probability;
            }
        }
        return m_randomObjects.Length - 1;
    }

}
