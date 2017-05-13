using FluentAssertions;
using System;
using Xunit;

namespace Akka.Serialization.Testkit
{
    public abstract class CyclicReferencesTests : TestKit.Xunit.TestKit
    {
        protected CyclicReferencesTests(Type serializerType) : base(ConfigFactory.GetConfig(serializerType))
        {
        }

        [Fact]
        public void Can_Serialize_Cyclic_References()
        {
            var bar = new CyclicMessages.Bar();
            bar.Self = bar;
            bar.XYZ = 234;

            var actual = AssertAndReturn(bar);
            actual.Self.Should().BeSameAs(actual);
            actual.XYZ.Should().Be(bar.XYZ);
        }

        protected T AssertAndReturn<T>(T message)
        {
            var serializer = Sys.Serialization.FindSerializerFor(message);
            var serialized = serializer.ToBinary(message);
            var result = serializer.FromBinary(serialized, typeof(T));
            return (T)result;
        }

        protected void AssertEqual<T>(T message)
        {
            var deserialized = AssertAndReturn(message);
            Assert.Equal(message, deserialized);
        }
    }

    public static class CyclicMessages
    {
        public class Bar
        {
            public long Boo { get; set; }
            public Bar Self { get; set; }
            public int XYZ { get; set; }
        }
    }
}
