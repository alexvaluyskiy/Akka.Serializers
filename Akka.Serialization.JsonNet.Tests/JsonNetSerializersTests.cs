using System;
using Akka.Serialization.Testkit;

namespace Akka.Serialization.JsonNet.Tests
{
    public class JsonNetPrimiviteSerializerTests : PrimitiveSerializerTests
    {
        public JsonNetPrimiviteSerializerTests() : base(typeof(JsonNetSerializer))
        {
        }
    }

    public class JsonNetImmutableMessagesTests : CustomMessagesTests
    {
        public JsonNetImmutableMessagesTests() : base(typeof(JsonNetSerializer))
        {
        }
    }

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

    public abstract class JsonNetCyclicReferencesTests : CyclicReferencesTests
    {
        public JsonNetCyclicReferencesTests() : base(typeof(JsonNetSerializer))
        {
        }
    }

    public class JsonNetExceptionsTests : ExceptionsSerializerTests
    {
        public JsonNetExceptionsTests() : base(typeof(JsonNetSerializer))
        {
        }
    }
}
