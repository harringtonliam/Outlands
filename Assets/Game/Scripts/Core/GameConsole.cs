using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Core
{
    public class GameConsole : MonoBehaviour
    {
        [SerializeField] int maxLineCount = 10;

        List<string> consoleLines;

        private static GameConsole _instance;

        public static event Action onLineAdded;

        public static int MaxLineCount
        {
            get { return _instance.maxLineCount; }
        }

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
                consoleLines = new List<string>();
            }
            
        }

        public static void AddNewLine (string newLine)
        {
            _instance.consoleLines.Add(newLine);
            _instance.RemoveLine();
            if (onLineAdded != null)
            {
                onLineAdded();
            }

        }

        public static string GetLastLine()
        {
            if (_instance.consoleLines.Count > 0)
            {
                return _instance.consoleLines.Last();
            }
            else
            {
                return string.Empty;
            }
            
        }

        public static List<string> GetAllLines()
        {
            return _instance.consoleLines;
        }

        private void RemoveLine()
        {
            if (consoleLines.Count>maxLineCount)
            {
                try
                {
                    consoleLines.RemoveAt(0);
                }
                catch (Exception)
                {
                    Debug.LogError("GameConsole - failed ot remove first time");
                }

            }

        }

        



    }


}


