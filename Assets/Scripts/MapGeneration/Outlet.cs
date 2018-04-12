using UnityEngine;
using System.Collections;

public class Outlet {

    private RoomGenerator m_prev;
    private RoomGenerator m_next;

    private float m_x, m_y;

    public RoomGenerator prev { get { return m_prev; } set { m_prev = value; } }
    public RoomGenerator next { get { return m_next; } set { m_next = value; } }
    public float X { get { return m_x; } set { m_x = value; } }
    public float Y { get { return m_y; } set { m_y = value; } }

    public Outlet(RoomGenerator prev, RoomGenerator next, MapGenerator.Direction dir)
    {
        m_prev = prev;
        m_next = next;

        m_prev.SetRoom(this, dir);
        m_next.SetRoom(this, MapGenerationHelpers.Opposite(dir));
    }
}