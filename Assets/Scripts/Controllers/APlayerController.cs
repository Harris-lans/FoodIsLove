using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public abstract class APlayerController : MonoBehaviour 
{
	#region Member Variables

		[Header("Hero Details")]
		[SerializeField]
		protected HeroController _HeroCharacter;

		[Space, Header("Combat Data")]
		[SerializeField]
		protected SO_CombatData _CombatData;

        protected GridSystem _GridSystem;
		protected PhotonView _PhotonView;
        public CookingPot CookingPot;

	#endregion

	#region Life Cycle

		private void Awake() 
		{
			// Getting the required level data
		    _GridSystem = GridSystem.Instance;
			_PhotonView = GetComponent<PhotonView>();
		}

		protected virtual void Start() 
		{
			_CombatData = Resources.Load<SO_CombatData>("CombatData");

            // Spawning cooking pot for the player
            CookingPot cookingPotPrefab = Resources.Load<CookingPot>("CookingPot");
		    CookingPot = Instantiate(cookingPotPrefab, Vector3.zero, Quaternion.identity);
		    CookingPot.Initialize(_PhotonView);
		}

	#endregion

	#region Member Functions

		public virtual void Initialize(HeroController hero)
		{
			_HeroCharacter = hero;
		    hero.OwnerID = _PhotonView.ViewID;
        }

		protected virtual void OnSelectedNode(ANode node)
		{
			if (_HeroCharacter != null)
			{
				_HeroCharacter.MoveToNode(node);
			}
		}

		protected virtual void OnSelectedIngredient(object ingredientData)
		{
			IngredientMinion ingredient = (IngredientMinion)ingredientData;
			_HeroCharacter.Cook(ingredient, CookingPot);
		}

    #endregion
}
