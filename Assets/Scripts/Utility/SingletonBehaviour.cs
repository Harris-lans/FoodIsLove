using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
	#region Global Variables

		public static T Instance;
		private Scene _CurrentScene;

		[SerializeField]
		private bool _DestroyOnSceneChange = true;
		[SerializeField]
		private bool _DestroyGameObjectOfDuplicateInstance = false;

	#endregion
	
	#region Life Cycle

		private void Awake () 
		{
			if (Instance == null)
			{
				Instance = (T)this;
				SingletonAwake();
			}
			else
			{
				if (_DestroyGameObjectOfDuplicateInstance)
				{
					Destroy(gameObject);
				}
				else
				{
					Destroy(this);
				}
			}
		}

		private void OnEnable()
		{
			SingletonOnEnable();
		}

		private void OnDisable()
		{
			SingletonOnDisable();
		}

		private void Start()
		{
			// Subscribing to scene change event
			SceneManager.activeSceneChanged += DestroySingletonInstance;
			SingletonStart();
		}

		private void Update()
		{
			SingletonUpdate();
		}
		
		private void OnDestroy()
		{
			SingletonOnDestroy();
		}

	#endregion

	#region Class Functions

		private void DestroySingletonInstance(Scene scene1, Scene scene2)
		{
			if (_DestroyOnSceneChange)
			{
				// Deleting the singleton Instance when the scene changes
				Instance = null;
			}
		}

	#endregion

	#region Virtual Functions

		protected virtual void SingletonAwake()
		{

		}
		protected virtual void SingletonStart()
		{

		}
		protected virtual void SingletonUpdate()
		{

		}
		protected virtual void SingletonOnDestroy()
		{

		}

		protected virtual void SingletonOnEnable()
		{

		}

		protected virtual void SingletonOnDisable()
		{
			
		}

	#endregion
}
