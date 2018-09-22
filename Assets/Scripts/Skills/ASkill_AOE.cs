using UnityEngine;

public abstract class ASkill_AOE : ASkill 
{
    #region Member Variables

        [Header("Area of Effect Attack Description")]
        [SerializeField]
        protected float _Radius;
        [SerializeField]
        protected float _Damage;
        [SerializeField]
        protected float _ForceToApply;

        [Header("VFX")]
        [SerializeField]
        protected GameObject _ParticleEffects;

    #endregion
}
