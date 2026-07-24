using System;
using System.Collections.Generic;
using JetBrains.Annotations;


namespace Modules.StateMachineLite.Develop.Runtime.Modules.State_Machine
{
	/// <summary>
	/// A minimal synchronous, trigger-driven finite state machine.
	/// States are represented by classes implementing <see cref="IState{TContext,TTrigger}"/> and are registered with <see cref="AddState{TState}"/>.
	/// The machine is started with <see cref="Start"/> and stopped with <see cref="Stop"/>.
	/// The current state is ticked with <see cref="Tick"/>.
	/// Transitions are triggered with <see cref="Fire"/>.
	///
	/// States are declared per-state with their own transition table:
	/// <code>
	/// machine.AddState&lt;IdleState&gt;()
	///        .AllowTransition&lt;ConcreteState&gt;(Trigger.SomeTrigger)
	///        .SetAsInitial();
	/// </code>
	///
	/// States can drive their own transitions by calling <see cref="Fire"/>. A trigger fired from inside
	/// <c>Tick</c>/<c>OnEnter</c> is applied at a safe point (after the current call returns) so a state is never
	/// exited while its own <c>Tick</c> is still on the stack.
	/// </summary>
	[PublicAPI]
	public sealed class StateMachine<TContext, TTrigger> : IStateMachine<TContext, TTrigger>
		where TContext : class, IStateContext
		where TTrigger : struct, Enum
	{
		private readonly Dictionary<Type, Entry> _entries = new();

		private Type  _initialType;
		private Entry _current;

		private bool     _isTicking;
		private bool     _draining;
		private bool     _hasPending;
		private TTrigger _pending;

		public TContext Context {get;}

		public bool IsRunning => _current != null;

		public StateMachine (TContext context)
		{
			Context = context ?? throw new ArgumentNullException(nameof(context));
		}

		public Builder AddState<TState> () where TState : class, IState<TContext, TTrigger>, new()
		{
			return new Builder(this, typeof(TState), Ensure<TState>());
		}

		/// <summary>Enters the initial state. Throws if already running or if no initial state was configured.</summary>
		public void Start ()
		{
			if (IsRunning)
				throw new InvalidOperationException("State machine is already running.");

			if (_initialType == null)
				throw new InvalidOperationException("Initial state is not set. Call SetAsInitial() on one of the states.");

			Validate();

			_hasPending = false;
			_current    = _entries[_initialType];
			_current.State.OnEnter(this);

			Drain();
		}

		/// <summary>Exits the current state (if any) and leaves the machine stopped. Safe to call when not running.</summary>
		public void Stop ()
		{
			if (!IsRunning)
				return;

			_current.State.OnExit(this);
			_current    = null;
			_hasPending = false;
		}

		public void Restart ()
		{
			Stop();
			Start();
		}

		public void Fire (TTrigger trigger)
		{
			_pending    = trigger; // last shot within a call wins
			_hasPending = true;

			if (!_isTicking && !_draining)
				Drain();
		}

		public void Tick (float deltaTime)
		{
			if (!IsRunning)
				return;

			_isTicking = true;

			try
			{
				_current.State.Tick(this, deltaTime);
			} finally
			{
				_isTicking = false;
			}

			Drain(); // apply transitions requested from inside Tick — not on Tick's stack
		}

		private void Drain ()
		{
			if (_draining)
				return;

			_draining = true;

			while (_hasPending && IsRunning)
			{
				_hasPending = false;

				if (!_current.Transitions.TryGetValue(_pending, out Type nextType))
					continue; // no transition for this trigger in the current state — ignore

				_current.State.OnExit(this);
				_current = _entries[nextType];
				_current.State.OnEnter(this); // may Fire again — the loop will pick it up
			}

			_draining = false;
		}

		private Entry Ensure<TState> () where TState : class, IState<TContext, TTrigger>, new()
		{
			Type type = typeof(TState);

			if (!_entries.TryGetValue(type, out Entry entry))
				_entries.Add(type, entry = new Entry(new TState()));

			return entry;
		}

		private void Validate ()
		{
			foreach (Entry entry in _entries.Values)
			{
				foreach (Type target in entry.Transitions.Values)
				{
					if (!_entries.ContainsKey(target))
						throw new InvalidOperationException($"Transition target '{target.Name}' was never registered via AddState().");
				}
			}
		}

		internal sealed class Entry
		{
			public readonly IState<TContext, TTrigger> State;
			public readonly Dictionary<TTrigger, Type> Transitions = new();

			public Entry (IState<TContext, TTrigger> state) => State = state;
		}

		[PublicAPI]
		public readonly struct Builder
		{
			private readonly StateMachine<TContext, TTrigger> _machine;
			private readonly Type                             _stateType;
			private readonly Entry                            _entry;

			internal Builder (StateMachine<TContext, TTrigger> machine, Type stateType, Entry entry)
			{
				_machine   = machine;
				_stateType = stateType;
				_entry     = entry;
			}

			public Builder AllowTransition<TTarget> (TTrigger trigger)
				where TTarget : class, IState<TContext, TTrigger>, new()
			{
				if (!_entry.Transitions.TryAdd(trigger, typeof(TTarget)))
					throw new ArgumentException($"Trigger '{trigger}' is already mapped for state '{_stateType.Name}'.");

				_machine.Ensure<TTarget>(); // the target registers itself, so AddState order does not matter

				return this;
			}

			public Builder SetAsInitial ()
			{
				_machine._initialType = _stateType;

				return this;
			}
		}
	}
}
