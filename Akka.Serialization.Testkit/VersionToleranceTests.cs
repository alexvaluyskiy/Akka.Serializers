using FluentAssertions;
using System;
using Xunit;
using static Akka.Serialization.Testkit.VersionTolerantMessages;

namespace Akka.Serialization.Testkit
{
    public abstract class VersionToleranceTests : TestKit.Xunit.TestKit
    {
        protected VersionToleranceTests(Type serializerType) : base(ConfigFactory.GetConfig(serializerType))
        {
        }

        [Fact]
        public void Can_DeserializeToADifferentClassWithSameMembers()
        {
            var message = new BasicMessageV1 { Name = "John" };
            var actual = AssertAndReturn<BasicMessageV1, BasicMessageV2>(message);
            actual.Name.Should().Be(message.Name);
        }

        protected TOut AssertAndReturn<TIn, TOut>(TIn message)
        {
            var serializer = Sys.Serialization.FindSerializerFor(message);
            var serialized = serializer.ToBinary(message);
            var result = serializer.FromBinary(serialized, typeof(TOut));
            return (TOut)result;
        }
    }

    public static class VersionTolerantMessages
    {
        public class BasicMessageV1
        {
            public string Name { get; set; }
        }

        public class BasicMessageV2
        {
            public string Name { get; set; }
        }
    }
}
