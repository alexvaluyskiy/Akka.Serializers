using System;
using Akka.Serialization.Testkit;
using Xunit;

namespace Akka.Serialization.Jil.Tests
{
    public class JilAkkaMessagesTests : AkkaMessagesTests
    {
        public JilAkkaMessagesTests() : base(typeof(JilSerializer))
        {
        }
    }

    public class JilCollectionsTests : CollectionsTests
    {
        public JilCollectionsTests() : base(typeof(JilSerializer))
        {
        }
    }

    public class JilCustomMessagesTests : CustomMessagesTests
    {
        public JilCustomMessagesTests() : base(typeof(JilSerializer))
        {
        }
    }

    // Jil does not support cyclic references
    public abstract class JilCyclicReferencesTests : CyclicReferencesTests
    {
        public JilCyclicReferencesTests() : base(typeof(JilSerializer))
        {
        }
    }

    public class JilExceptionsTests : ExceptionsTests
    {
        public JilExceptionsTests() : base(typeof(JilSerializer))
        {
        }
    }

    public class JilImmutableMessagesTests : ImmutableMessagesTests
    {
        public JilImmutableMessagesTests() : base(typeof(JilSerializer))
        {
        }
    }

    // Jil does not support preserving of object references
    public abstract class JilObjectReferencesTests : ObjectReferencesTests
    {
        public JilObjectReferencesTests() : base(typeof(JilSerializer))
        {
        }
    }

    public class JilPolymorphismTests : PolymorphismTests
    {
        public JilPolymorphismTests() : base(typeof(JilSerializer))
        {
        }
    }

    public class JilPrimiviteSerializerTests : PrimitiveSerializerTests
    {
        public JilPrimiviteSerializerTests() : base(typeof(JilSerializer))
        {
        }

        [Fact(Skip="Jil does not support Uri")]
        public override void Can_Serialize_Uri()
        {
        }
    }
}
