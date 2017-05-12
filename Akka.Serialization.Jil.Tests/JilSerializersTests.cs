using System;
using Akka.Serialization.Testkit;
using Xunit;

namespace Akka.Serialization.Jil.Tests
{
    public class JilPrimiviteSerializerTests : PrimitiveSerializerTests
    {
        public JilPrimiviteSerializerTests() : base(typeof(JilSerializer))
        {
        }

        [Fact(Skip="Jil does not support Uri")]
        public override void Can_Serialize_Uri()
        {
        }

        [Fact(Skip = "Jil supports DateTime only in Utc format")]
        public override void Can_Serialize_DateTime()
        {
        }
    }

    public class JilImmutableMessagesTests : CustomMessagesTests
    {
        public JilImmutableMessagesTests() : base(typeof(JilSerializer))
        {
        }

        [Fact(Skip = "Jil does not support Parameterless constructor")]
        public override void Can_Serialize_ImmutableMessage()
        {
        }

        [Fact(Skip = "Jil does not support Parameterless constructor")]
        public override void Can_Serialize_ImmutableMessageWithDefaultParameters()
        {
        }

        [Fact(Skip = "Jil does not support Parameterless constructor")]
        public override void Can_Serialize_ImmutableMessageWithTwoConstructors()
        {
        }

        [Fact(Skip = "Jil does not support deserialization of object type")]
        public override void Can_Serialize_MessageWithObjectPropertyPrimitive()
        {
        }

        [Fact(Skip = "Jil does not support deserialization of object type")]
        public override void Can_Serialize_MessageWithObjectPropertyComplex()
        {
        }
    }

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
}
