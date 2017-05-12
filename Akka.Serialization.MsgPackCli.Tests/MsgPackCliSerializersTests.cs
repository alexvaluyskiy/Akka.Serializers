using System;
using Akka.Serialization.Testkit;
using Xunit;

namespace Akka.Serialization.MsgPackCli.Tests
{
    public class MsgPackCliSerializersTests : PrimitiveSerializerTests
    {
        public MsgPackCliSerializersTests() : base(typeof(MsgPackCliSerializer))
        {
        }
    }

    public class MsgPackCliImmutableMessagesTests : CustomMessagesTests
    {
        public MsgPackCliImmutableMessagesTests() : base(typeof(MsgPackCliSerializer))
        {
        }

        [Fact(Skip = "MsgPack.Cli does not support messages without fields or properties")]
        public override void Can_serialize_EmptyMessage()
        {
        }

        [Fact(Skip = "MsgPack.Cli does not support messages without fields or properties")]
        public override void Can_serialize_EmptySingleton()
        {
        }

        [Fact(Skip = "MsgPack.Cli does not support immutable messages")]
        public override void Can_Serialize_ImmutableMessage()
        {
        }

        [Fact(Skip = "MsgPack.Cli does not support immutable messages")]
        public override void Can_Serialize_ImmutableMessageWithDefaultParameters()
        {
        }

        [Fact(Skip = "MsgPack.Cli does not support immutable messages")]
        public override void Can_Serialize_ImmutableMessageWithTwoConstructors()
        {
        }

        [Fact(Skip = "MsgPack.Cli does not support deserialization of object type")]
        public override void Can_Serialize_MessageWithObjectPropertyPrimitive()
        {
        }

        [Fact(Skip = "MsgPack.Cli does not support deserialization of object type")]
        public override void Can_Serialize_MessageWithObjectPropertyComplex()
        {
        }
    }

    public class MsgPackCliAkkaMessagesTests : AkkaMessagesTests
    {
        public MsgPackCliAkkaMessagesTests() : base(typeof(MsgPackCliSerializer))
        {
        }
    }

    public class MsgPackCliCollectionsTests : CollectionsTests
    {
        public MsgPackCliCollectionsTests() : base(typeof(MsgPackCliSerializer))
        {
        }
    }
}
