﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/New Dialogue")]
[System.Serializable]
public class DialogueEventSO : ScriptableObject
{
    public virtual void RunEvent() {
        Debug.Log("Event Was Called");
    }
}
