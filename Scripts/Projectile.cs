using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum projectTileType
{
    rock, arrow, fireball
};
public class Projectile : MonoBehaviour
{
    [SerializeField]
    private int attackDamage;

    [SerializeField]
    private projectTileType pType;

    public int AttackDamage
    {
        get { return attackDamage; }
    }
    public projectTileType PType
    {
        get
        {
            return pType;
        }
    }
}
