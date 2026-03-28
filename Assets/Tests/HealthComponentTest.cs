using Assets.Player;
using NUnit.Framework;

namespace Tests {
    public class HealthComponentTest {
        [Test]
        public void TakeDamageTest() {
            HealthComponent health = new HealthComponent();
            health.MaxHealth = 3;
            health.WoundedHealth = 1;
            health.InitHealth();
            Assert.AreEqual(health.MaxHealth, health.Health);
            Assert.IsFalse(health.IsWounded);
            Assert.IsTrue(health.IsAlive);

            health.TakeDamage(1);
            Assert.IsFalse(health.IsWounded);
            Assert.IsTrue(health.IsAlive);

            health.TakeDamage(1);
            Assert.IsTrue(health.IsWounded);
            Assert.IsTrue(health.IsAlive);

            health.TakeDamage(1);
            Assert.IsFalse(health.IsWounded);
            Assert.IsFalse(health.IsAlive);
        }

        [Test]
        public void HealTest() {
            HealthComponent health = new HealthComponent();
            health.MaxHealth = 3;
            health.WoundedHealth = 1;
            health.Health = 1;
            Assert.IsTrue(health.IsWounded);
            Assert.IsTrue(health.IsAlive);

            health.Heal();
            Assert.IsFalse(health.IsWounded);
            Assert.IsTrue(health.IsAlive);
        }
    }
}
