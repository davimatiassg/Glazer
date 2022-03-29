using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class InputManager : MonoBehaviour
{
	public static InputManager instance;

	[SerializeField] private KeyBindings keybindings;

	void Awake()
	{
		if(instance == null)
		{
			instance = this;
			
		}
		else if(instance != this)
		{
			Destroy(this);	
		}
		DontDestroyOnLoad(this.gameObject);
	}

	// Update is called once per frame
	public KeyCode GetKeyForAction(BindableActions kbAction)
	{
		foreach(KeyBindings.KeyBindingCheck kbc in  keybindings.keyBindingChecks)
		{
			if(kbc.action == kbAction)
			{
				return kbc.key;
			}
		}
		return KeyCode.None;
	}

	public bool GetButton(BindableActions key)
	{
		foreach(KeyBindings.KeyBindingCheck kbc in  keybindings.keyBindingChecks)
		{
			if(kbc.action == key)
			{
				return Input.GetKey(kbc.key);
			}
		}
		return false;
	}

	public bool GetButtonDown(BindableActions key)
	{
		foreach(KeyBindings.KeyBindingCheck kbc in  keybindings.keyBindingChecks)
		{
			if(kbc.action == key)
			{
				return Input.GetKeyDown(kbc.key);
			}
		}
		return false;
	}

	public bool GetButtonUp(BindableActions key)
	{
		foreach(KeyBindings.KeyBindingCheck kbc in  keybindings.keyBindingChecks)
		{
			if(kbc.action == key)
			{
				return Input.GetKeyUp(kbc.key);
			}
		}
		return false;
	}
	public int GetAxisRaw(BindableActions axis)
	{ 
		foreach(KeyBindings.AxisBindingCheck abc in keybindings.axisBindingChecks)
		{
			if(abc.axis == axis)
			{
				return Convert.ToInt32(Input.GetKey(abc.positivekey)) - Convert.ToInt32(Input.GetKey(abc.negativekey));
			}
		}
		return 0;
	}
/*
	GetButton(string key)
	{
		if(Input.GetKey(keybindings.CheckKey(key)))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool GetButtonDown(string key)
	{
		if(Input.GetKeyDown(keybindings.CheckKey(key)))
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool GetButtonUp(string key)
	{
		if(Input.GetKeyUp(keybindings.CheckKey(key)))
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	*/
}
