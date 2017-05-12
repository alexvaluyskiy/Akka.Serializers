using System;
using Akka.Serialization.Testkit;
using Xunit;

namespace Akka.Serialization.ProtobufNet.Tests
{
    public class ProtobufNetPrimiviteSerializerTests : PrimitiveSerializerTests
    {
        public ProtobufNetPrimiviteSerializerTests() : base(typeof(ProtobufNetSerializer))
        {
        }

        [Fact(Skip = "Protobuf-net does not support ValueTuple. The will fix it soon")]
        public override void Can_Serialize_ValueTuple()
        {
        }

        [Fact(Skip = "Protobuf-net does not support DateTimeOffset. The will fix it soon")]
        public override void Can_Serialize_DateTimeOffset()
        {
        }

        [Fact(Skip = "Protobuf-net does not support BigInteger")]
        public override void Can_Serialize_BigInteger()
        {
            base.Can_Serialize_BigInteger();
        }
    }

    public class ProtobufNetImmutableMessagesTests : CustomMessagesTests
    {
        public ProtobufNetImmutableMessagesTests() : base(typeof(ProtobufNetSerializer))
        {
        }
    }

    public class ProtobufNetAkkaMessagesTests : AkkaMessagesTests
    {
        public ProtobufNetAkkaMessagesTests() : base(typeof(ProtobufNetSerializer))
        {
        }
    }

    public class ProtobufNetCollectionsTests : CollectionsTests
    {
        public ProtobufNetCollectionsTests() : base(typeof(ProtobufNetSerializer))
        {
        }
    }
}
