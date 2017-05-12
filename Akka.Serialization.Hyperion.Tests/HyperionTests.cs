using System;
using Akka.Serialization.Testkit;

namespace Akka.Serialization.Hyperion.Tests
{
    public class HyperionPrimiviteSerializerTests : PrimitiveSerializerTests
    {
        public HyperionPrimiviteSerializerTests() : base(typeof(HyperionSerializer))
        {
        }
    }

    public class HyperionImmutableMessagesTests : CustomMessagesTests
    {
        public HyperionImmutableMessagesTests() : base(typeof(HyperionSerializer))
        {
        }
    }

    public class HyperionAkkaMessagesTests : AkkaMessagesTests
    {
        public HyperionAkkaMessagesTests() : base(typeof(HyperionSerializer))
        {
        }
    }

    public class HyperionCollectionsTests : CollectionsTests
    {
        public HyperionCollectionsTests() : base(typeof(HyperionSerializer))
        {
        }
    }
}
