using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Keybindings", menuName = "Keyboard Bindings Asset")]
public class KeyBindings : ScriptableObject
{

	[System.Serializable]
	public class KeyBindingCheck
	{
		public BindableActions action;
		public KeyCode key;
	}

	
	[System.Serializable]
	public class AxisBindingCheck
	{
		public BindableActions axis;
		public KeyCode positivekey, negativekey;

	}
	public AxisBindingCheck[] axisBindingChecks = new AxisBindingCheck[2];
	public KeyBindingCheck[] keyBindingChecks = new KeyBindingCheck[5];
	


}
