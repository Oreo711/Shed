using LitMotion;


namespace Shed.Runtime.Utilities.LitMotion
{
	public struct JumpMotionOptions : IMotionOptions
	{
		public readonly float Height;

		public JumpMotionOptions (float height = 1)
		{
			Height = height;
		}
	}
}
