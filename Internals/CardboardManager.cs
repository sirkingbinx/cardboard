using UnityEngine;
using Cardboard.Start;
using System.Collections.Generic;
using Cardboard.Classes;

namespace Cardboard.Internals
{
    public class CardboardManager : MonoBehaviour
    {
        public static CardboardManager instance { get; private set; }

        void Start()
        {
            instance = this;
        }
    }
}
