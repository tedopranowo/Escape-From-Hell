using UnityEngine;

[System.Serializable]
public class CustomSerializedData
{
    [SerializeField] private string m_serializedData;

	public void AddInt(int data)
    {
        m_serializedData += data.ToString() + '|';
    }

    public void AddFloat(float data)
    {
        m_serializedData += data.ToString() + '|';
    }

    public void AddString(string data)
    {
        m_serializedData += data + '|';
    }

    public string[] GetData()
    {
        return m_serializedData.Split('|');
    }

    public void Reset()
    {
        m_serializedData = "";
    }

    public bool IsEmpty()
    {
        return m_serializedData.Length == 0 || m_serializedData == null;
    }
}
