using System;
using UnityEngine;

namespace Assets.Player {
    public class HealthComponent : Component {
        public int MaxHealth = 3;
        public int WoundedHealth = 1;
        public int Health;

        public bool IsAlive {
            get {
                return Health > 0;
            }
        }

        public bool IsWounded {
            get {
                return IsAlive && Health <= WoundedHealth;
            }
        }

        public void TakeDamage(int damage) {
            Health = Math.Max(0, Health - Math.Max(0, damage));
        }

        public void Heal() {
            Health = MaxHealth;
        }

        public void InitHealth() {
            Health = MaxHealth;
        }
    }
}
