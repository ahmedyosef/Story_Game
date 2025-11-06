using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sort_Story_Data", menuName = "Scriptable Objects/Sort_Story_Data")]
public class Sort_Story_Data : ScriptableObject
{
    public List<Sprite> game_Imge;
    public Sprite point;
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
}

