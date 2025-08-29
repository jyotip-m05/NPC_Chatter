using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Script
{
    [Serializable]
    public struct Talk
    {
        [SerializeField] public string question;
        [SerializeField] public string answer;

        public Talk(string question, string answer)
        {
            this.question = question;
            this.answer = answer;
        }
    }
}
