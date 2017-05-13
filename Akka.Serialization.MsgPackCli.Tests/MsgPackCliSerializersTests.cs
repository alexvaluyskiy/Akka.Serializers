using System;
using Akka.Serialization.Testkit;
using Xunit;

namespace Akka.Serialization.MsgPackCli.Tests
{
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

    public class MsgPackCliCustomMessagesTests : CustomMessagesTests
    {
        public MsgPackCliCustomMessagesTests() : base(typeof(MsgPackCliSerializer))
        {
        }
    }

    // MsgPack.CLI does not support resolving of Cyclic references
    public abstract class MsgPackCliCyclicReferencesTests : CyclicReferencesTests
    {
        public MsgPackCliCyclicReferencesTests() : base(typeof(MsgPackCliSerializer))
        {
        }
    }

    public class MsgPackCliExceptionsTests : ExceptionsTests
    {
        public MsgPackCliExceptionsTests() : base(typeof(MsgPackCliSerializer))
        {
        }
    }

    // MsgPack.CLI does not support any immutable messages
    public abstract class MsgPackCliImmutableMessagesTests : ImmutableMessagesTests
    {
        public MsgPackCliImmutableMessagesTests() : base(typeof(MsgPackCliSerializer))
        {
        }
    }

    // MsgPack.CLI does not support preserving of object references
    public abstract class MsgPackCliObjectReferencesTests : ObjectReferencesTests
    {
        public MsgPackCliObjectReferencesTests() : base(typeof(MsgPackCliSerializer))
        {
        }
    }

    public class MsgPackCliPrimitiveSerializerTests : PrimitiveSerializerTests
    {
        public MsgPackCliPrimitiveSerializerTests() : base(typeof(MsgPackCliSerializer))
        {
        }
    }
}
