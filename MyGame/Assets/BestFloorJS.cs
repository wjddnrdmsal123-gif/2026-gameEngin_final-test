using UnityEngine;
using System.Runtime.InteropServices;

public static class BestFloorJS
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void SaveBestFloorJS(int floor);

    [DllImport("__Internal")]
    private static extern int GetBestFloorJS();

    [DllImport("__Internal")]
    private static extern void ResetBestFloorJS();
#endif

    private const string EditorBestFloorKey = "EditorBestFloor";

    public static void SaveBestFloor(int floor)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        SaveBestFloorJS(floor);
#else
        int currentBest = GetBestFloor();

        if (floor > currentBest)
        {
            PlayerPrefs.SetInt(EditorBestFloorKey, floor);
            PlayerPrefs.Save();

            Debug.Log("에디터용 최고 층수 저장됨: F-" + floor);
        }
#endif
    }

    public static int GetBestFloor()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        return GetBestFloorJS();
#else
        return PlayerPrefs.GetInt(EditorBestFloorKey, 0);
#endif
    }

    public static void ResetBestFloor()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        ResetBestFloorJS();
#else
        PlayerPrefs.DeleteKey(EditorBestFloorKey);
        PlayerPrefs.Save();

        Debug.Log("에디터용 최고 층수 초기화");
#endif
    }
}