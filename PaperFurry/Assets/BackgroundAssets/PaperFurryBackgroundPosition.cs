using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperFurryBackgroundPosition : MonoBehaviour
{

    public CharacterInteractionPosition CharacterInteractionPosition;
    void Start() 
    {
        CharacterInteractionPosition = FindObjectOfType<CharacterInteractionPosition>();
    }
    void Update()
    {
        transform.position = new Vector3(CharacterInteractionPosition.position_now_x,transform.position.y,transform.position.z);
    }
}
