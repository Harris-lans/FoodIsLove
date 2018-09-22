using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientHero : Ingredient 
{
    [Header("Hero specific Stats")]
    [SerializeField]
    private float _Power;

    [SerializeField]
    private float _Influence;
}
