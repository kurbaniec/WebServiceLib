using MTCG.Cards.DamageUtil;
using NUnit.Framework;

namespace MTCG_Test.Unit
{
    public class DamageTest
    {
        [Test, TestCase(TestName = "Damage comparison A > B", Description =
             "Damage A has higher Value that Damage B")]
        public void AHasHigherDamageThanB()
        {
            var a = Damage.Normal(10);
            var b = Damage.Normal(5);

            var result = a.CompareTo(b);
            
            Assert.GreaterOrEqual(result, 1);
        }
        
        [Test, TestCase(TestName = "Damage comparison A < B", Description =
             "Damage B has higher Value that Damage A")]
        public void BHasHigherDamageThanA()
        {
            var a = Damage.Normal(5);
            var b = Damage.Normal(10);

            var result = a.CompareTo(b);
            
            Assert.LessOrEqual(result, -1);
        }
        
        [Test, TestCase(TestName = "Damage comparison A = B", Description =
             "Both Damages share the same Value")]
        public void AEqualToB()
        {
            var a = Damage.Normal(10);
            var b = Damage.Normal(10);

            var result = a.CompareTo(b);
            
            Assert.AreEqual(0, result);
        }
        
        [Test, TestCase(TestName = "Damage comparison A > B", Description =
             "Damage A has higher Damage because of Infinity modifier")]
        public void AIsInftyBIsNot()
        {
            var a = Damage.Infty();
            var b = Damage.Normal(10);

            var result = a.CompareTo(b);
            
            Assert.GreaterOrEqual(result, 1);
        }
        
        [Test, TestCase(TestName = "Damage comparison A < B", Description =
             "Damage B has higher Damage because of Infinity modifier")]
        public void BIsInftyAIsNot()
        {
            var a = Damage.Normal(10);
            var b = Damage.Infty();

            var result = a.CompareTo(b);
            
            Assert.GreaterOrEqual(result, -1);
        }
        
        [Test, TestCase(TestName = "Damage comparison A = B", Description =
             "Both Damages share the Infinity modifier")]
        public void AEqualToBWithInftyModifier()
        {
            var a = Damage.Infty();
            var b = Damage.Infty();

            var result = a.CompareTo(b);
            
            Assert.AreEqual(0, result);
        }
        
        [Test, TestCase(TestName = "Add damage Value", Description =
             "Add damage Value to Damage a")]
        public void AddDamage()
        {
            var a = Damage.Normal(10);

            a.Add(100);
            var result = a.Value;
            
            Assert.AreEqual(110, result);
        }
        
        [Test, TestCase(TestName = "Apply Infinity modifier", Description =
             "Apply Infinity modifier to Damage a")]
        public void SetInfty()
        {
            var a = Damage.Normal(10);

            a.SetInfty();
            var result = a.IsInfty;
            
            Assert.IsTrue(result);
        }
        
        [Test, TestCase(TestName = "Set no damage (Infty)", Description =
             "Set no damage with Damage containing Infinity modifier")]
        public void NoDamageInfty()
        {
            var a = Damage.Infty();

            a.SetNoDamage();
            var isInfty = a.IsInfty;
            var value = a.Value;
            
            Assert.IsFalse(isInfty);
            Assert.AreEqual(0, value);
        }
        
        [Test, TestCase(TestName = "Set no damage (Value)", Description =
             "Set no damage with Damage containing regular Value")]
        public void NoDamageValue()
        {
            var a = Damage.Normal(10);

            a.SetNoDamage();
            var isInfty = a.IsInfty;
            var value = a.Value;
            
            Assert.IsFalse(isInfty);
            Assert.AreEqual(0, value);
        }
        
    }
}