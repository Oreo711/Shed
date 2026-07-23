namespace Modules.StateMachineLite._Project.Develop.Runtime.Modules.State_Machine
{
	/// <summary>
	/// Marker for an object that owns a <see cref="StateMachine{TContext,TTrigger}"/> and exposes its data to the
	/// states as a context. Usually the actor itself implements this and hands its own implementation
	/// details to its states.
	/// </summary>
	public interface IStateContext {}
}
