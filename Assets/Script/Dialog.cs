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

    [Serializable]
    public struct Chat
    {
        [SerializeField] public string massage;
        [SerializeField] public int side; // 0 left, 1 right
        public Chat(string massage, int side)
        {
            this.massage = massage;
            this.side = side;
        }
    }
}
