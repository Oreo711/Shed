using System;
using JetBrains.Annotations;


namespace _Project.Develop.Runtime.Gameplay
{
	[PublicAPI]
	public static class TickerExtensions
	{
		/// <summary>
		/// Executes the action if at least one tick has occurred.
		/// If multiple ticks have occurred, the action is still executed only once.
		/// This is useful for scenarios where you want to ensure an action is performed
		/// at regular intervals but don't need to handle every single tick individually.
		/// For example, updating a UI element or triggering an event that doesn't require
		/// high-frequency updates.
		/// </summary>
		/// <param name="ticker"></param>
		/// <param name="onTick"></param>
		/// <param name="deltaTime"></param>
		public static void Do (ref this Ticker ticker, Action onTick, float deltaTime)
		{
			if (onTick == null) throw new ArgumentNullException(nameof(onTick));

			if (ticker.Tick(deltaTime) > 0) onTick.Invoke();
		}

		/// <summary>
		/// Allocation-free version of <see cref="Do"/>.
		/// <inheritdoc cref="Do"/>
		/// </summary>
		/// <param name="ticker"></param>
		/// <param name="owner"></param>
		/// <param name="onTick"></param>
		/// <param name="deltaTime"></param>
		public static void DoNonAlloc<T> (ref this Ticker ticker, T owner, Action<T> onTick, float deltaTime) where T : class
		{
			if (onTick == null) throw new ArgumentNullException(nameof(onTick));

			if (ticker.Tick(deltaTime) > 0) onTick.Invoke(owner);
		}

		/// <summary>
		/// Executes the action for each tick that has occurred.
		/// This is useful for scenarios where you need to handle every tick individually,
		/// such as processing game logic updates, animations, or other time-sensitive operations.
		/// </summary>
		/// <param name="ticker"></param>
		/// <param name="onTick"></param>
		/// <param name="deltaTime"></param>
		public static void DoEach (ref this Ticker ticker, Action onTick, float deltaTime)
		{
			if (onTick == null) throw new ArgumentNullException(nameof(onTick));

			int ticks = ticker.Tick(deltaTime);

			for (int i = 0; i < ticks; i++) onTick.Invoke();
		}

		/// <summary>
		/// Allocation-free version of <see cref="DoEach"/>.
		/// <inheritdoc cref="DoEach"/>
		/// </summary>
		/// <param name="ticker"></param>
		/// <param name="owner"></param>
		/// <param name="onTick"></param>
		/// <param name="deltaTime"></param>
		public static void DoEachNonAlloc<T> (ref this Ticker ticker, T owner, Action<T> onTick, float deltaTime) where T : class
		{
			if (onTick == null) throw new ArgumentNullException(nameof(onTick));

			int ticks = ticker.Tick(deltaTime);

			for (int i = 0; i < ticks; i++) onTick.Invoke(owner);
		}

		/// <summary>
		/// Executes the action once, passing the number of ticks that have occurred as a parameter.
		/// This is useful for scenarios where you want to perform an action based on the number
		/// of ticks that have occurred, such as updating a score, adjusting a timer,
		/// or triggering events that depend on the number of intervals that have passed.
		/// </summary>
		/// <param name="ticker"></param>
		/// <param name="onTick"></param>
		/// <param name="deltaTime"></param>
		public static void DoBatched (ref this Ticker ticker, Action<int> onTick, float deltaTime)
		{
			if (onTick == null) throw new ArgumentNullException(nameof(onTick));

			int ticks = ticker.Tick(deltaTime);

			if (ticks > 0) onTick.Invoke(ticks);
		}

		/// <summary>
		/// Allocation-free version of <see cref="DoBatched"/>.
		/// <inheritdoc cref="DoBatched"/>
		/// </summary>
		/// <param name="ticker"></param>
		/// <param name="owner"></param>
		/// <param name="onTick"></param>
		/// <param name="deltaTime"></param>
		public static void DoBatchedNonAlloc<T> (ref this Ticker ticker, T owner, Action<T, int> onTick, float deltaTime) where T : class
		{
			if (onTick == null) throw new ArgumentNullException(nameof(onTick));

			int ticks = ticker.Tick(deltaTime);

			if (ticks > 0) onTick.Invoke(owner, ticks);
		}
	}
}
