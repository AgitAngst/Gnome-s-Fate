using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class States : MonoBehaviour
{
    public enum CharacterState
    {
        Idle,
        Running,
        Dead,
        
    }
    
    public enum ObstacleType
    {
        Obstacle,
        Enemy
    }

    public enum CharacterLine
    {
        Left,
        Center,
        Right
    }
    
}
