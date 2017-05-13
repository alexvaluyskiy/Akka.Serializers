using FluentAssertions;
using System;
using Xunit;

namespace Akka.Serialization.Testkit
{
    public abstract class ObjectReferencesTests : TestKit.Xunit.TestKit
    {
        protected ObjectReferencesTests(Type serializerType) : base(ConfigFactory.GetConfig(serializerType))
        {
        }

        [Fact]
        public virtual void Can_PreserveObjectReference()
        {
            var bar = new ObjectReferencesMessages.Baz();
            bar.Foo = 50555;
            bar.Bar = 234;

            var sameReference = new ObjectReferencesMessages.SameReference();
            sameReference.One = bar;
            sameReference.Two = bar;

            var actual = AssertAndReturn(sameReference);
            actual.One.Should().BeSameAs(actual.Two);
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

    public static class ObjectReferencesMessages
    {
        public class Baz
        {
            public long Foo { get; set; }
            public int Bar { get; set; }
        }

        public class SameReference
        {
            public Baz One { get; set; }
            public Baz Two { get; set; }
        }
    }
}
