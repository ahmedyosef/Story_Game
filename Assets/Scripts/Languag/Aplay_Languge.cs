using RTLTMPro;
using UnityEngine;

public class Aplay_Languge : MonoBehaviour
{
    public LayoutDirection languageDirection;
    public RTLTextMeshPro text;
    public Loclization_Datat loclization_Datat;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text=gameObject.GetComponent<RTLTextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public enum LayoutDirection
    {
        LeftToRight,
        RightToLeft
    }
}
