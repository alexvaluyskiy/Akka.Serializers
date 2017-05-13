using System;
using Akka.Serialization.Testkit;
using Xunit;

namespace Akka.Serialization.Hyperion.Tests
{
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

    public class HyperionCustomMessagesTests : CustomMessagesTests
    {
        public HyperionCustomMessagesTests() : base(typeof(HyperionSerializer))
        {
        }
    }

    public class HyperionCyclicReferencesTests : CyclicReferencesTests
    {
        public HyperionCyclicReferencesTests() : base(typeof(HyperionSerializer))
        {
        }
    }

    public class HyperionExceptionsSerializerTests : ExceptionsTests
    {
        public HyperionExceptionsSerializerTests() : base(typeof(HyperionSerializer))
        {
        }
    }

    public class HyperionImmutableMessagesTests : ImmutableMessagesTests
    {
        public HyperionImmutableMessagesTests() : base(typeof(HyperionSerializer))
        {
        }
    }

    public class HyperionObjectReferencesTests : ObjectReferencesTests
    {
        public HyperionObjectReferencesTests() : base(typeof(HyperionSerializer))
        {
        }
    }

    public class HyperionPolymorphismTests : PolymorphismTests
    {
        public HyperionPolymorphismTests() : base(typeof(HyperionSerializer))
        {
        }
    }

    public class HyperionPrimiviteSerializerTests : PrimitiveSerializerTests
    {
        public HyperionPrimiviteSerializerTests() : base(typeof(HyperionSerializer))
        {
        }
    }
}
