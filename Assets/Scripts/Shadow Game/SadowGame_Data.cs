using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SadowGame_Data", menuName = "Scriptable Objects/SadowGame_Data")]
public class SadowGame_Data : ScriptableObject
{
    public List<Sprite> game_Imge;
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

