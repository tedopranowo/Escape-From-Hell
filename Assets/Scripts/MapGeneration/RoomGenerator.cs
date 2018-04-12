using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator {
    private int m_index = -1;
    private int m_path = 10000;
    private int m_key = 10000;
    private RoomGenerator m_parent;
    private MapGenerator.Direction m_parDir = MapGenerator.Direction.None;

    public int path { get { return m_path; } set { m_path = value; } }
    public int Key { get { return m_key; } set { m_key = value; } }
    public RoomGenerator Parent { get { return m_parent; } set { m_parent = value; } }
    public MapGenerator.Direction ParDir { get { return m_parDir; } set { m_parDir = value; } }

    public RoomGenerator(int mapIndex)
	{
		m_index = mapIndex;
    }

    Outlet m_top = null;
    Outlet m_bottom = null;
    Outlet m_right = null;
    Outlet m_left = null;

    public bool CheckAvailability(MapGenerator.Direction dir)
    {
        if ((dir == MapGenerator.Direction.Up && m_top != null) ||
            (dir == MapGenerator.Direction.Right && m_right != null) ||
            (dir == MapGenerator.Direction.Left && m_left != null) ||
            (dir == MapGenerator.Direction.Down && m_bottom != null))
        {
            return false;
        }

        return true;
    }

	public void SetRoom(Outlet outlet, MapGenerator.Direction dir)
	{
		switch(dir)
		{
		case MapGenerator.Direction.Up:
			m_top = outlet;
			return;
		case MapGenerator.Direction.Down:
			m_bottom = outlet;
			return;
		case MapGenerator.Direction.Left:
			m_left = outlet;
			return;
		case MapGenerator.Direction.Right:
			m_right = outlet;
			return;
		}
	}

    public int GetIndex()
    {
        return m_index;
    }
}
