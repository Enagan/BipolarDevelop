//Made by: Engana
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// RoomFactoryInstancedObjectsRegistry is used by the RoomFactory to register already instanced rooms, and their objects.
/// As well as keeping a connection between the instanced objects and their originating definitions
/// </summary>
public class RoomFactoryInstancedObjectsRegistry
{
  // Complex data type, should be seen as:
  // Key: RoomDef -> Value: { RoomParentObject, { Key:ObjectDef, Value:ObjectInstance } }
  private Dictionary<RoomDefinition, KeyValuePair<GameObject, Dictionary<RoomObjectDefinition, GameObject>>> _instancedRegistry =
    new Dictionary<RoomDefinition, KeyValuePair<GameObject, Dictionary<RoomObjectDefinition, GameObject>>>();

  #region Setters
  public void RegisterRoom(RoomDefinition room, GameObject roomParentObject)
  {
    if (_instancedRegistry.ContainsKey(room))
    {
      SMConsole.Log("Error: Room " + room.roomName + " already registered in the registry. Perhaps a room is not being cleaned on deletion?", "SceneSystem", SMLogType.ERROR);
      return;
    }
    _instancedRegistry.Add(room,
                          new KeyValuePair<GameObject, Dictionary<RoomObjectDefinition, GameObject>>(
                            roomParentObject,
                            new Dictionary<RoomObjectDefinition, GameObject>()));
  }

  public void RegisterObjectInRoom(RoomDefinition roomDef, RoomObjectDefinition objDef, GameObject obj)
  {
    if (RoomIsRegistered(roomDef))
    {
      _instancedRegistry[roomDef].Value.Add(objDef, obj);
    }
    else
    {
      SMConsole.Log("Error: Object addition to room " + roomDef.roomName + " failed. Room does not exist in registry", "SceneSystem", SMLogType.ERROR);
    }
  }

  /// <summary>
  /// Removes a room and all it's objects from the registry
  /// </summary>
  public void RemoveRoomFromRegistry(RoomDefinition roomDef)
  {
    if (RoomIsRegistered(roomDef))
    {
      _instancedRegistry.Remove(roomDef);
    }
    else
    {
      SMConsole.Log("Error: Removal of room " + roomDef.roomName + " failed. Room does not exist in registry", "SceneSystem", SMLogType.ERROR);
    }
  }
  /// <summary>
  /// Remove an object from a given room's registry
  /// </summary>
  public void UnregisterObjectFromRoom(RoomDefinition roomDef, RoomObjectDefinition objectDef)
  {
    if (RoomIsRegistered(roomDef))
    {
      _instancedRegistry[roomDef].Value.Remove(objectDef);
    }
    else
    {
      SMConsole.Log("Error: Removal of object " + objectDef.objectPrefabPath + " failed. Room does not exist in registry", "SceneSystem", SMLogType.ERROR);
    }
  }
  #endregion

  #region Getters
  /// <summary>
  /// Retrieves the instanced game object for a given object definition in a given room
  /// </summary>
  public GameObject GetGameObjectFromDefinition(RoomDefinition roomDef, RoomObjectDefinition objDef)
  {
    if (RoomIsRegistered(roomDef))
    {
      return _instancedRegistry[roomDef].Value[objDef];
    }
    else
    {
      SMConsole.Log("Error: Object retrieval in room " + roomDef.roomName + " failed. Room does not exist in registry", "SceneSystem", SMLogType.ERROR);
      return null;
    }
  }

  /// <summary>
  /// Gets the Object definition of a given GameObject in a given room
  /// </summary>
  public RoomObjectDefinition GetDefinitionFromGameObject(RoomDefinition roomDef, GameObject obj)
  {
    if (RoomIsRegistered(roomDef))
    {
      foreach (KeyValuePair<RoomObjectDefinition, GameObject> objects in _instancedRegistry[roomDef].Value)
      {
        if (objects.Value == obj)
        {
          return objects.Key;
        }
      }
    }
    else
    {
      SMConsole.Log("Error: Object Definition retrieval in room " + roomDef.roomName + " failed. Room does not exist in registry", "SceneSystem", SMLogType.ERROR);

      return null;
    }
    SMConsole.Log("Error: Could not find object definition in registry", "SceneSystem", SMLogType.ERROR);

    return null;
  }

  /// <summary>
  /// Retrieves all game objects in a given room
  /// </summary>
  public Dictionary<RoomObjectDefinition, GameObject> GetAllGameObjectsFromRoom(RoomDefinition roomDef)
  {
    if (RoomIsRegistered(roomDef))
    {
      return _instancedRegistry[roomDef].Value;
    }
    else
    {
      SMConsole.Log("Error: Object retrieval in room " + roomDef.roomName + " failed. Room does not exist in registry", "SceneSystem", SMLogType.ERROR);
      return null;
    }
  }

  /// <summary>
  /// Get the parent object of a registered room
  /// </summary>
  public GameObject getRoomParentObject(RoomDefinition roomDef)
  {
    return RoomIsRegistered(roomDef) ? _instancedRegistry[roomDef].Key : null;
  }
  #endregion

  #region Checkers
  /// <summary>
  /// Checks if a room is registered
  /// </summary>
  public bool RoomIsRegistered(RoomDefinition room)
  {
    return _instancedRegistry.ContainsKey(room);
  }

  #endregion
}
