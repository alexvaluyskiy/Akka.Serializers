using System;
using Akka.Serialization.Testkit;

namespace Akka.Serialization.JsonNet.Tests
{
    public class JsonNetAkkaMessagesTests : AkkaMessagesTests
    {
        public JsonNetAkkaMessagesTests() : base(typeof(JsonNetSerializer))
        {
        }
    }

    public class JsonNetCollectionsTests : CollectionsTests
    {
        public JsonNetCollectionsTests() : base(typeof(JsonNetSerializer))
        {
        }
    }

    public class JsonNetCustomMessagesTests : CustomMessagesTests
    {
        public JsonNetCustomMessagesTests() : base(typeof(JsonNetSerializer))
        {
        }
    }

    public class JsonNetCyclicReferencesTests : CyclicReferencesTests
    {
        public JsonNetCyclicReferencesTests() : base(typeof(JsonNetSerializer))
        {
        }
    }

    public class JsonNetExceptionsTests : ExceptionsTests
    {
        public JsonNetExceptionsTests() : base(typeof(JsonNetSerializer))
        {
        }
    }

    public class JsonNetImmutableMessagesTests : ImmutableMessagesTests
    {
        public JsonNetImmutableMessagesTests() : base(typeof(JsonNetSerializer))
        {
        }
    }

    public class JsonNetObjectReferencesTests : ObjectReferencesTests
    {
        public JsonNetObjectReferencesTests() : base(typeof(JsonNetSerializer))
        {
        }
    }

    public class JsonNetPolymorphismTests : PolymorphismTests
    {
        public JsonNetPolymorphismTests() : base(typeof(JsonNetSerializer))
        {
        }
    }

    public class JsonNetPrimiviteSerializerTests : PrimitiveSerializerTests
    {
        public JsonNetPrimiviteSerializerTests() : base(typeof(JsonNetSerializer))
        {
        }
    }
}
