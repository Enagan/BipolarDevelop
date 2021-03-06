//Made By: Engana
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Enum designed to be used only inside the system to ease in archiving and registering listners
/// </summary>
enum BipolarEvent { PlayerRoomChange, ObjectRoomChange, PlayerAbilityObtain, PlayerAbilityScan, TutorialMessageTrigger, PauseGame, CheckPoint, JackedInActivation, PlayerChanged }

/// <summary>
/// System designed to handle events specific to bipolar, in situations where a lot of different
/// classes will have to observe certain happenings ingame.
/// </summary>
public class BipolarEventHandlerSystem
{
  // Dictionary to contains lists of objects for a type of event
  Dictionary<BipolarEvent, List<object>> _listners = new Dictionary<BipolarEvent, List<object>>();

  #region Register Listners

  /// <summary>
  /// Registers a new listner for Player Room Change Events
  /// </summary>
  public void RegisterPlayerRoomChangeListner(IPlayerRoomChangeListner listner)
  {
    RegisterEventListner(BipolarEvent.PlayerRoomChange, listner);
  }

  /// <summary>
  /// Registers a new listner for Object Room Change Events
  /// </summary>
  public void RegisterObjectRoomChangeListner(IObjectRoomChangeListner listner)
  {
    RegisterEventListner(BipolarEvent.ObjectRoomChange, listner);
  }

  /// <summary>
  /// Registers a new listener for Player Ability Obtain Events
  /// </summary>
  public void RegisterPlayerAbilityObtainListener(IPlayerAbilityObtainListener listener)
  {
    RegisterEventListner(BipolarEvent.PlayerAbilityObtain, listener);
  }

  /// <summary>
  /// Registers a new listener for TutorialMessageTrigger Events
  /// </summary>
  public void RegisterTutorialMessageTriggerListener(ITutorialMessageTriggerListener listener)
  {
    RegisterEventListner(BipolarEvent.TutorialMessageTrigger, listener);
  }

  /// <summary>
  /// Registers a new listener for AbilityScan Events
  /// </summary>
  public void RegisterPlayerAbilityScanListener(IPlayerAbilityScanListener listener)
  {
    RegisterEventListner(BipolarEvent.PlayerAbilityScan, listener);
  }

  /// <summary>
  /// Registers a new listener for Pause Events
  /// </summary>
  public void RegisterPauseListener(IPauseListener listener)
  {
    RegisterEventListner(BipolarEvent.PauseGame, listener);
  }

  /// <summary>
  /// Registers a new listener for CheckPoint change events
  /// </summary>
  public void RegisterCheckPointListener(ICheckPointRegisterListener listener)
  {
    RegisterEventListner(BipolarEvent.CheckPoint, listener);
  }

  /// <summary>
  /// Registers a new listener for PlayerChanged Events
  /// </summary>
  public void RegisterPlayerChangedListener(IPlayerChangedListener listener)
  {
      RegisterEventListner(BipolarEvent.PlayerChanged, listener);
  }
  /// <summary>
  /// Registers a new listener for Jacked-In mode activation events
  /// </summary>
  public void RegisterJackedInActivationListener(IJackedInActivationListener listener)
  {
    RegisterEventListner(BipolarEvent.JackedInActivation, listener);
//
  }
 
  /// <summary>
  /// Master Register event function, receives the cooresponding enum, and listner
  /// archiving them for future sending of events
  /// </summary>
  private void RegisterEventListner(BipolarEvent eventType, object listner)
  {
    if (!_listners.ContainsKey(eventType))
    {
      _listners.Add(eventType, new List<object>());
    }
    _listners[eventType].Add(listner);
  }
  #endregion

  #region Send Events
  /// <summary>
  /// Sends a Player Room Change Event, should receive the new room's name
  /// </summary>
  public void SendPlayerRoomChangeEvent(string newRoomName)
  {
    if (!_listners.ContainsKey(BipolarEvent.PlayerRoomChange))
    {
      return;
    }
    foreach (IPlayerRoomChangeListner listner in _listners[BipolarEvent.PlayerRoomChange])
    {
      listner.ListenPlayerRoomChange(newRoomName);
    }
  }

  /// <summary>
  /// Sends an Object Room Change Event, should receive the previous and new room's name as well as the object that transitioned
  /// </summary>
  public void SendObjectRoomChangeEvent(string prevRoomName, string newRoomName, GameObject objectPastDoor)
  {
    if (!_listners.ContainsKey(BipolarEvent.ObjectRoomChange))
    {
      return;
    }
    foreach (IObjectRoomChangeListner listner in _listners[BipolarEvent.ObjectRoomChange])
    {
      listner.ListenObjectRoomChange(prevRoomName, newRoomName, objectPastDoor);
    }
  }

  /// <summary>
  /// Sends a Player Ability Obtain Event, should receive the new room's name
  /// </summary>
  public void SendPlayerAbilityObtainEvent(string newAbilityName)
  {
    if (!_listners.ContainsKey(BipolarEvent.PlayerAbilityObtain))
    {
      return;
    }
    foreach (IPlayerAbilityObtainListener listener in _listners[BipolarEvent.PlayerAbilityObtain])
    {
      listener.ListenPlayerAbilityObtain(newAbilityName);
    }
  }

  /// <summary>
  /// Sends a Player Ability Obtain Event, should receive the new room's name
  /// </summary>
  public void SendTutorialMessageTriggerEvent(string tutorialMessage)
  {
    if (!_listners.ContainsKey(BipolarEvent.TutorialMessageTrigger))
    {
      return;
    }
    foreach (ITutorialMessageTriggerListener listener in _listners[BipolarEvent.TutorialMessageTrigger])
    {
      listener.ListenTutorialMessageTrigger(tutorialMessage);
    }
  }

  /// <summary>
  /// Sends a Player Ability Scan Event, should receive bool indicating if its scanning or not
  /// </summary>
  public void SendPlayerAbilityScanEvent(bool isScanning)
  {
    if (!_listners.ContainsKey(BipolarEvent.PlayerAbilityScan))
    {
      return;
    }
    foreach (IPlayerAbilityScanListener listener in _listners[BipolarEvent.PlayerAbilityScan])
    {
      listener.ListenPlayerScan(isScanning);
    }
  }

  /// <summary>
  /// Sends a Game Event
  /// </summary>
  public void SendPauseEvent(bool isPaused)
  {
    if (!_listners.ContainsKey(BipolarEvent.PauseGame))
    {
      return;
    }
    foreach (IPauseListener listener in _listners[BipolarEvent.PauseGame])
    {
      listener.ListenPause(isPaused);
    }
  }

  /// <summary>
  /// Sends a Game Event for playerChanged
  /// </summary>
  public void SendPlayerChangedEvent(PlayerController newPlayer)
  {
      if (!_listners.ContainsKey(BipolarEvent.PlayerChanged))
      {
          return;
      }
      foreach (IPlayerChangedListener listener in _listners[BipolarEvent.PlayerChanged])
      {
          listener.ListenPlayerChange(newPlayer);
      }
  }

  /// <summary>
  /// Sends a Checkpoint register event
  /// </summary>
  public void SendCheckPointRegisterEvent(CheckPoint checkpoint)
  {
    if (!_listners.ContainsKey(BipolarEvent.PauseGame))
    {
      return;
    }
    foreach (ICheckPointRegisterListener listener in _listners[BipolarEvent.CheckPoint])
    {
      listener.ListenCheckPointChange(checkpoint);
    }
  }


  /// <summary>
  /// Sends a Jacked-In mode activation event
  /// </summary>
  public void SendJackedInActivationEvent(Camera camera)
  {
    if (!_listners.ContainsKey(BipolarEvent.PauseGame))
    {
      return;
    }
    foreach (IJackedInActivationListener listener in _listners[BipolarEvent.JackedInActivation])
    {
      listener.ListenJackedInActivation(camera);
    }
  }

  /// <summary>
  /// Sends a Jacked-In mode deactivation event
  /// </summary>
  public void SendJackedInDeactivationEvent()
  {
    if (!_listners.ContainsKey(BipolarEvent.PauseGame))
    {
      return;
    }
    foreach (IJackedInActivationListener listener in _listners[BipolarEvent.JackedInActivation])
    {
      listener.ListenJackedInDeactivation();
    }
  }
  #endregion

}
