using UnityEditor;
using UnityEngine;


namespace Shed.Editor
{
	public static class RectTransformMenuExtensions
	{
		[MenuItem("CONTEXT/RectTransform/" + RectTransformBakePositionToAnchorsImpl.CommandName, isValidateFunction: false, priority: 510)]
		private static void BakePositionToAnchors (MenuCommand command)
		{
			RectTransform context = (RectTransform)command.context;

			RectTransformBakePositionToAnchorsImpl.Do(context);
		}
	}
}
