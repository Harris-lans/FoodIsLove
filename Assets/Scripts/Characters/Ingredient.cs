using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;

public abstract class Ingredient : MonoBehaviour
{
    #region Member Variables

        [Header("Character Stats")]
        [SerializeField] 
        protected float _HP;

        [SerializeField] 
        protected float _Speed;

        [Space, Header("Ingredient Events")]
        [HideInInspector]
        public UnityEvent MinionHurtEvent;
        [HideInInspector]
        public UnityEvent MinionDeathEvent;

        protected float _CurrentHP;

    #endregion

    #region Life Cycle

        protected void Awake()
        {
            _CurrentHP = _HP;
        } 

    #endregion

    #region Member Functions 

        public virtual void Damage(float damageAmount, GameObject damageOwner)
        {
            _CurrentHP -= damageAmount;

            if (_CurrentHP <= 0)
            {
                IngredientDie();
                return;
            }

            MinionHurtEvent.Invoke();
        }

        protected virtual void IngredientDie()
        {
            MinionDeathEvent.Invoke();
            PhotonNetwork.Destroy(GetComponent<PhotonView>());
        }
    
    #endregion

    #region Properties

        public float HealthFraction
        {
            get 
            {
                return (_CurrentHP / _HP);
            }
        }

    #endregion
}
