using UnityEngine;
using Unity;
using System.IO;
using System.Collections.Generic;

public class CSV
{
    static CSV csv;
    public List<string[]> m_ArrayData;
    public static CSV GetInstance()
    {
        if (csv == null)
        {
            csv = new CSV();
        }
        return csv;
    }
    private CSV() { m_ArrayData = new List<string[]>(); }
    public string GetString(int row, int col)
    {
        return m_ArrayData[row][col];
    }

    public void LoadFile(string path)
    {
        m_ArrayData.Clear();
        StreamReader sr = null;
        try
        {
            sr = File.OpenText(path);
            Debug.Log("file finded!");
        }
        catch(System.Exception e)
        {
            Debug.Log(e.ToString());
            return;
        }
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            m_ArrayData.Add(line.Split(','));
        }
        sr.Close();
        sr.Dispose();
    }
}
