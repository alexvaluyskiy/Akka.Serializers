using System;
using Akka.Serialization.Testkit;
using Akka.Serializer.MsgPack;
using Xunit;

namespace Akka.Serialization.MsgPack.Tests
{
    public class MsgPackPrimiviteSerializerTests : PrimitiveSerializerTests
    {
        public MsgPackPrimiviteSerializerTests() : base(typeof(MsgPackSerializer))
        {
        }

        [Fact(Skip = "MsgPack supports DateTime only in Utc format")]
        public override void Can_Serialize_DateTime()
        {
        }
    }

    public class MsgPackImmutableMessagesTests : CustomMessagesTests
    {
        public MsgPackImmutableMessagesTests() : base(typeof(MsgPackSerializer))
        {
        }

        [Fact(Skip = "MsgPack does not support messages without public constructor")]
        public override void Can_serialize_EmptySingleton()
        {
        }

        [Fact(Skip = "MsgPack does not support deserialization of object type")]
        public override void Can_Serialize_MessageWithObjectPropertyComplex()
        {
        }
    }

    public class MsgPackAkkaMessagesTests : AkkaMessagesTests
    {
        public MsgPackAkkaMessagesTests() : base(typeof(MsgPackSerializer))
        {
        }
    }

    public class MsgPackCollectionsTests : CollectionsTests
    {
        public MsgPackCollectionsTests() : base(typeof(MsgPackSerializer))
        {
        }
    }
}
