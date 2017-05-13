using System;
using Akka.Serialization.Testkit;
using Xunit;

namespace Akka.Serialization.ProtobufNet.Tests
{
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

    public class ProtobufNetCustomMessagesTests : CustomMessagesTests
    {
        public ProtobufNetCustomMessagesTests() : base(typeof(ProtobufNetSerializer))
        {
        }
    }

    // protobuf-net does not support resolving of Cyclic references
    public abstract class ProtobufNetCyclicReferencesTests : CyclicReferencesTests
    {
        public ProtobufNetCyclicReferencesTests() : base(typeof(ProtobufNetSerializer))
        {
        }
    }

    public class ProtobufNetExceptionsTests : ExceptionsTests
    {
        public ProtobufNetExceptionsTests() : base(typeof(ProtobufNetSerializer))
        {
        }
    }

    public class ProtobufNetImmutableMessagesTests : ImmutableMessagesTests
    {
        public ProtobufNetImmutableMessagesTests() : base(typeof(ProtobufNetSerializer))
        {
        }
    }

    // protobuf-net does not support preserving of object references
    public abstract class ProtobufNetObjectReferencesTests : ObjectReferencesTests
    {
        public ProtobufNetObjectReferencesTests() : base(typeof(ProtobufNetSerializer))
        {
        }
    }

    // protobuf-net does not support polymorphism
    // TODO: try attributes
    public abstract class ProtobufNetPolymorphismTests : PolymorphismTests
    {
        public ProtobufNetPolymorphismTests() : base(typeof(ProtobufNetSerializer))
        {
        }
    }

    public class ProtobufNetPrimiviteSerializerTests : PrimitiveSerializerTests
    {
        public ProtobufNetPrimiviteSerializerTests() : base(typeof(ProtobufNetSerializer))
        {
        }
    }
}
