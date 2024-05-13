using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.UI;

public class Save
{
    public string name;
    public float firstLikeability;
    public float secondLikeability;
    public int curProgress;
    public string saveDate;
}

public class DialogStatus : MonoBehaviour
{
    private string playerName;
    public int curProgress;
}
