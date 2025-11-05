using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "True_Or_False_Data", menuName = "Scriptable Objects/True_Or_False_Data")]
public class True_Or_False_Data : ScriptableObject
{
    public Layout language;
    public List<string> Quize_Text;
    public List<bool> chick_Correct_Or_Not;
    [ReadOnly]public List<int> imge_ID;
    [Button]
    public void AutoFill_Id()
    {
        imge_ID.Clear();
        for (int i = 0; i < Quize_Text.Count; i++)
        {
            imge_ID.Add(i);
        }
    }
    public enum Layout
    {
        left_To_right,
        right_To_left
    }
}

