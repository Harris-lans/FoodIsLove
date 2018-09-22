using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class ASkill : MonoBehaviour 
{
    #region Member Variables

        [Header("Generic Skill Details")]
	    public string Name;
        public Sprite Thumbnail;
        public SkillTag Tag;
        public SkillTypes Type;
        [SerializeField]
	    protected float _CooldownTime;
        public SkillStates State;

        [Header("Skill Events")] 
        public UnityEvent OnSkillCastEvent;
        public UnityEvent OnSkillCooldownCompleteEvent;

        public float TimeBeforeAvailability;
        private Animator _Animator;

    #endregion

    #region Life Cycle

        protected virtual void Start()
        {
            State = SkillStates.AVAILABLE;
            _Animator = GetComponent<Animator>();
            TimeBeforeAvailability = _CooldownTime;
        }

    #endregion

    #region Member Functions

        public virtual void Cast(object data)
        {
            // Cancelling cast if the skill is on cooldown or unavailable
            if (State != SkillStates.AVAILABLE)
            {
                return;
            }

            OnSkillCastEvent.Invoke();
            StartCoroutine(StartCooldown());
        }

        private IEnumerator StartCooldown()
        {
            State = SkillStates.ON_COOLDOWN;
            ResetCoolDown();
            
            while (TimeBeforeAvailability <= _CooldownTime)
            {
                TimeBeforeAvailability += 0.25f;
                yield return new WaitForSeconds(0.25f);
            }
            
            OnSkillCooldownCompleteEvent.Invoke();
            State = SkillStates.AVAILABLE;
        }

        protected void ResetCoolDown()
        {
            // Setting timer
            TimeBeforeAvailability = 0;
        }

    #endregion

    #region Properties

        public float CoolDownFraction
        {
            get 
            {
                return TimeBeforeAvailability / _CooldownTime;
            }
        }

    #endregion

    #region Local Enums

        public enum SkillStates : byte
        {
            AVAILABLE = 0,
            ON_COOLDOWN,
            UNAVAILABLE
        }

        public enum SkillTag : byte
        {
            PRIMARY_ACTIVE = 0,
            SECONDARY_ACTIVE,
            PASSIVE
        }

    #endregion
}
