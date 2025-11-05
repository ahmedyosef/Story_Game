using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ho_Is_Me_Data", menuName = "Scriptable Objects/Ho_Is_Me_Data")]
public class Ho_Is_Me_Data : ScriptableObject
{
    public Layout language;
    public List<Sprite> game_Imge;
    public Sprite point;
    public List<string> Quize;
    [ReadOnly] public List<int> imge_ID;
    [Button]
    public void AutoFill_Id()
    {
        imge_ID.Clear();
        for (int i = 0; i < game_Imge.Count; i++)
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


