using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 调用模板进行操作
/// </summary>
public class PaperFurryCameraControl3D : MonoBehaviour
{
    public CameraPosition cP;
    public PaperFurryPosition3D PaperFurryPosition3D;


    void Start() 
    {
        PaperFurryPosition3D = FindObjectOfType<PaperFurryPosition3D>();
    }

    void Update() 
    {
        cP.CameraPositionSet(PaperFurryPosition3D.position_now_x,PaperFurryPosition3D.position_now_y+2,PaperFurryPosition3D.position_now_z-5);  
    }
}