using NUnit.Framework;

namespace StopLossKata.Tests.Convensions
{
    [TestFixture]
    public abstract class SpecificationFor<T>
    {
        protected T Subject;
        
        protected abstract T Given();

        protected abstract void When();

        [TestFixtureSetUp]
        public void SetUp()
        {
            Subject = Given();
            When();
        }

        protected void CheckExists(object value)
        {
            Assert.IsNotNull(value);
        }

        protected void CheckValue<TValue>(TValue expectedValue, TValue actualValue)
        {
            Assert.AreEqual(expectedValue, actualValue);
        }
    }
}