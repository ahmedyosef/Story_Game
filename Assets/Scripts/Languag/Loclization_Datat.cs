using NUnit.Framework;
using System.Collections.Generic;
using System;
using System.Globalization;
using UnityEngine;

[CreateAssetMenu(fileName = "Loclization_Datat", menuName = "Scriptable Objects/Loclization_Datat")]
public class Loclization_Datat : ScriptableObject
{
    public LayoutDirection languageDirection;
    public List<LocalizedText> localizationText;

    public enum LayoutDirection
    {
        LeftToRight,
        RightToLeft
    }

    [Serializable]
    public class LocalizedText
    {
        public string id;
        [TextArea]
        public string languageData;
    }
}
