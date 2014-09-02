﻿using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class SMConsoleBotSection {


  SMConsoleData _data;
  
  // Scroll Vectors for ScrollViews
  Vector2 _botLeftWindowScroll;
  Vector2 _botMidWindowScroll;
  Vector2 _botRightWindowScroll;

  // Texture for stack trace jump button
  Texture2D _warpTex;


  public SMConsoleBotSection()
  {
    _data = SMConsoleData.Instance;
    _warpTex = (Texture2D)Resources.Load("GUI/Sprites/warp", typeof(Texture2D));
  }

  public void drawBotSection(float width, float height)
  {
    // Selected Message display
    _botLeftWindowScroll = GUILayout.BeginScrollView(_botLeftWindowScroll, GUILayout.MaxHeight(height - _data.currentScrollViewHeight), GUILayout.Width(width * 0.3f));
    displaySelectedMessage(height);
    GUILayout.EndScrollView();

    // Stack trace display
    _botMidWindowScroll = GUILayout.BeginScrollView(_botMidWindowScroll, GUILayout.MaxHeight(height - _data.currentScrollViewHeight), GUILayout.Width(width * 0.7f));
    displayStackTrace(height);
    GUILayout.EndScrollView();
  }

  // displays the selected message
  void displaySelectedMessage(float height)
  {
    string selectedMessage;
    if (!_data.canCollapse)
    {
      selectedMessage = _data.selectedLogMessage.log;
    }
    else
    {
      selectedMessage = _data.selectedCollapsedMessage.message.log;
    }
    EditorGUILayout.SelectableLabel(selectedMessage, GUI.skin.label, GUILayout.MaxHeight(height - _data.currentScrollViewHeight));
  }

  // displays the stack trace of selected message
  void displayStackTrace(float height)
  {
    string stackTrace;
    if (!_data.canCollapse)
    {
      stackTrace = _data.selectedLogMessage.stackTrace;
    }
    else
    {
      stackTrace = _data.selectedCollapsedMessage.message.stackTrace;
    }

    if (stackTrace != null)
    {
      string[] stackTraces = stackTrace.Split('\n');
      foreach (string trace in stackTraces)
      {
        GUILayout.BeginHorizontal();

        string lineEntry = stackTraces[stackTraces.Length - 1];
        if (_data.isEntryJumpable(trace))
        {
          if (GUILayout.Button(_warpTex, GUILayout.Width(32), GUILayout.Height(32))) 
            _data.jumpToSelectedScript(trace);
        }

        GUIStyle skin = GUI.skin.label;
        skin.wordWrap = true;
        EditorGUILayout.SelectableLabel(trace, skin, GUILayout.MaxHeight(height - _data.currentScrollViewHeight));
        GUILayout.EndHorizontal();
      }
    }
  }
    


}