using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 调用模板进行操作
/// </summary>
public class PaperFurryCameraControl : MonoBehaviour
{
    public CameraPosition cP;
    public CharacterInteractionPosition CharacterInteractionPosition;


    void Start() 
    {
        CharacterInteractionPosition = FindObjectOfType<CharacterInteractionPosition>();
    }

    void Update() 
    {
        cP.CameraPositionSet(CharacterInteractionPosition.position_now_x,CharacterInteractionPosition.position_now_y+2,-10);  
    }
}