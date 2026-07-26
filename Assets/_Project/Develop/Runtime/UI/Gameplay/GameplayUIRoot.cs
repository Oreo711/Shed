using UnityEngine;


namespace Shed.Runtime.UI.Gameplay
{
	public class GameplayUIRoot : MonoBehaviour
	{
		[field: SerializeField] public Transform HUDLayer            { get; private set; }
		[field: SerializeField] public Transform PopupsLayer         { get; private set; }
		[field: SerializeField] public Transform VFXUnderPopupsLayer { get; private set; }
		[field: SerializeField] public Transform VFXOverPopupsLayer  { get; private set; }
	}
}
