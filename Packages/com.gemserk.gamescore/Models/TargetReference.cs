using System;
using Game.Utilities;
using UnityEngine;

namespace Game.Models
{
    public class TargetReference : MonoBehaviour
    {
        [NonSerialized]
        public Target target = new Target();
    }
}