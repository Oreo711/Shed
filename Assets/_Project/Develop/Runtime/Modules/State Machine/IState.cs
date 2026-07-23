using System;
using JetBrains.Annotations;


namespace Modules.StateMachineLite._Project.Develop.Runtime.Modules.State_Machine
{
	/// <summary>
	/// A single synchronous state. Receives the machine on every call so it can read
	/// <see cref="IStateMachine{TContext,TTrigger}.Context"/> and request its own transitions via
	/// <see cref="IStateMachine{TContext,TTrigger}.Fire"/>. Instances are created once and reused, so any mutable
	/// per-visit data must be reset in <see cref="OnEnter"/>, not in the constructor.
	/// </summary>
	[PublicAPI]
	public interface IState<TContext, TTrigger>
		where TContext : class, IStateContext
		where TTrigger : struct, Enum
	{
		void OnEnter (IStateMachine<TContext, TTrigger> machine);

		void OnExit (IStateMachine<TContext, TTrigger> machine);

		void Tick (IStateMachine<TContext, TTrigger> machine, float deltaTime);
	}
}
