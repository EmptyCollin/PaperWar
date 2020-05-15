using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public void SceneLoader(int SceneIndex)
    {
        SceneManager.LoadSceneAsync(SceneIndex);
    }

    public void test()
    {
        Debug.Log("yes");
    }
}
