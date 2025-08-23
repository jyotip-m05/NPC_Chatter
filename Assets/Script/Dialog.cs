using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Script
{
    [Serializable]
    public struct Talk
    {
        [SerializeField] public string text;
        // [SerializeField] public int id;

        public Talk(string text, TextAsset asset) : this()
        {
        }
    }

    [System.Serializable]
    public class Dialog
    {
        [SerializeField] private List<Talk> talks;
        // [SerializeField] public List<TextAlignmentOptions> assets;

        public List<Talk> Talks
        {
            get { return talks; }
        }

        // public List<TextAlignmentOptions> Assets
        // {
        //     get { return assets; }
        // }
    }
}
