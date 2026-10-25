using Shed.Runtime.UI.Gameplay;


namespace Shed.Runtime.Gameplay.Features
{
	public class MonologueService
	{
		private bool _hasHeardKnocking;
		private bool _hasEncounteredDoor;
		private bool _hasEncounteredWindow;
		private bool _hasEncounteredCloset;
		private bool _hasPacifiedCloset;
		private bool _hasBeenLowOnBattery;
		private bool _hasBeenLowOnEnergy;
		private bool _hasShot;

		private readonly GameplayScreenPresenter _screen;

		public MonologueService (GameplayScreenPresenter screen)
		{
			_screen = screen;
		}

		public void Initialize ()
		{
			_screen.ProcessMonologue("Phew.. this shed will protect me from them. I have to make sure they don't get in.");
		}

		public void ShowKnockingMonologue ()
		{
			if (!_hasHeardKnocking)
			{
				_screen.ProcessMonologue(
					"One of them is knocking on the door..\nIf it opens it, I'll have to turn off the flashlight and stay quiet, it might leave if I do.");
				_hasHeardKnocking = true;
			}
		}

		public void ShowDoorEncounterMonologue ()
		{
			if (!_hasEncounteredDoor)
			{
				_screen.ProcessMonologue("...");
				_hasEncounteredDoor = true;
			}
		}

		public void ShowWindowEncounterMonologue ()
		{
			if (!_hasEncounteredWindow)
			{
				_screen.ProcessMonologue("Something is at the window. Maybe the flashlight could scare it away?");
				_hasEncounteredWindow = true;
			}
		}

		public void ShowClosetEncounterMonologue ()
		{
			if (!_hasEncounteredCloset)
			{
				_screen.ProcessMonologue("Something is rummaging through the closet, I can't let it get out.");
				_hasEncounteredCloset = true;
			}
		}

		public void ShowPacifiedClosetMonologue ()
		{
			if (!_hasPacifiedCloset)
			{
				_screen.ProcessMonologue("It's gone from the closet. \nI'm sure it will be back, I should be careful.");
				_hasPacifiedCloset = true;
			}
		}

		public void ShowLowBatteryMonologue ()
		{
			if (!_hasBeenLowOnBattery)
			{
				_screen.ProcessMonologue("The batteries are dying.. Good thing I have a box of them. I think i put it on the table.");
				_hasBeenLowOnBattery = true;
			}
		}

		public void ShowLowEnergyMonologue ()
		{
			if (!_hasBeenLowOnEnergy)
			{
				_screen.ProcessMonologue("I'm so tired...");
				_hasBeenLowOnEnergy = true;
			}
		}

		public void ShowShotMonologue ()
		{
			if (!_hasShot)
			{
				_screen.ProcessMonologue("I'm out of bullets.. if I let one of them in again I'll die for sure.");
				_hasShot = true;
			}
		}
	}
}
