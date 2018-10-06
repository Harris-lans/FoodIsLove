using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEditor.Animations;
using UnityEngine;

public class IngredientHero : Ingredient 
{
    [Header("Hero specific Stats")]
    [SerializeField]
    private float _Power;

    [SerializeField]
    private float _Influence;

    [Header("UI Icons")]
    public AnimatorController ClashAnimation;

    public bool IsLocal { get; private set; }

    #region Life Cycle

        private void Awake()
        {
            IsLocal = GetComponent<PhotonView>().IsMine;
        }

    #endregion
}
