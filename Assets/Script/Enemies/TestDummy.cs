using System;
using System.Collections;
using System.Collections.Generic;
using JLProject;
using UnityEngine;

namespace MyNamespace{
    public class TestDummy : Entity{
        public float speed;

        public EventItem eventItem;
        // Use this for initialization
        void Start() {
            MovementSpeed = speed;
            Faction = Damage.Faction.Enemy;
        }

        protected override void Movement() {
        }


        protected override void Attack() {
        }
    }
}
