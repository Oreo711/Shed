using System;
using JetBrains.Annotations;


namespace Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine
{
	/// <summary>
	///     Narrow runtime view of a state machine handed to its states. Exposes only what a state legitimately needs at
	///     runtime — the shared context and the ability to request a transition — and deliberately hides the
	///     configuration surface (<c>AddState</c>/<c>AllowTransition</c>/<c>Start</c>) so states cannot reconfigure the
	///     machine that drives them.
	/// </summary>
	[PublicAPI]
	public interface IStateMachine<TContext, TTrigger>
		where TContext : class, IStateContext
		where TTrigger : struct, Enum
	{
		TContext Context {get;}

		void Fire (TTrigger trigger);
	}
}
