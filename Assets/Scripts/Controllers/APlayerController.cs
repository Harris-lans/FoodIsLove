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

        protected GridSystem _GridSystem;
		protected ASkill _SelectedSkill;
		protected PhotonView _PhotonView;

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
			// Spawning cooking pot for the player
			CookingPot cookingPotPrefab = Resources.Load<CookingPot>("CookingPot");
			var cookingPot = Instantiate(cookingPotPrefab, Vector3.zero, Quaternion.identity);
			cookingPot.Initialize(_PhotonView);
		}

	#endregion

	#region Member Functions

		public virtual void Initialize(HeroController hero)
		{
			_HeroCharacter = hero;
		}

		protected virtual void OnSelectedNode(GridPosition selectedCell, ANode node)
		{
			_HeroCharacter.MoveToNode(selectedCell, node);
		}

		protected virtual void OnSelectedIngredient(object ingredientData)
		{
			IngredientMinion ingredient = (IngredientMinion)ingredientData;
			_HeroCharacter.Cook(ingredient);
		}

	#endregion

}
