using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextRefs : MonoBehaviour
{
    public Text sensText;
    public Text pointText;
    public Text prevAdjusText;

    public void Awake()
    {
        GameManager.instance.sensText = sensText;
        GameManager.instance.pointText = pointText;
        GameManager.instance.prevAdjusText = prevAdjusText;
        GameManager.instance.UpdateStats();
    }

    public void Quit()
    {
        if (Cursor.visible)
            Application.Quit();
    }
}
