using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Akka.Serialization.Testkit;
using FluentAssertions;
using MsgPack.Serialization;
using Xunit;

namespace Akka.Serialization.MsgPackCli.Tests
{
    public class MsgPackCliPolymorphismTests : PolymorphismTests
    {
        public MsgPackCliPolymorphismTests() : base(typeof(MsgPackCliSerializer))
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

        [Fact(Skip = "MsgPack.Cli does not support deserialization of object type")]
        public override void Can_Serialize_CollectionWithObjectTypePrimitive()
        {
        }

        [Fact(Skip = "MsgPack.Cli does not support deserialization of object type")]
        public override void Can_Serialize_CollectionWithObjectTypeComplex()
        {
        }

        [Fact(Skip = "MsgPack.Cli does not support deserialization of object type")]
        public override void Can_Serialize_DictionaryKeyWithObjectTypePrimitive()
        {
        }

        [Fact(Skip = "MsgPack.Cli does not support deserialization of object type")]
        public override void Can_Serialize_DictionaryKeyWithObjectTypeComplex()
        {
        }

        [Fact(Skip = "MsgPack.Cli does not support deserialization of object type")]
        public override void Can_Serialize_TupleItemWithObjectTypePrimitive()
        {
        }

        [Fact(Skip = "MsgPack.Cli does not support deserialization of object type")]
        public override void Can_Serialize_TupleItemWithObjectTypeComplex()
        {
        }

        [Fact]
        public void Can_Serialize_MessageWithObjectProperty_MsgPackCliRuntimeType()
        {
            var expected = new PolymorhphicMessages.MsgPackWithObjectPropertyRuntimeType
            {
                Name = "John",
                Data = 435345345
            };
            var actual = AssertAndReturn(expected);
            actual.Name.Should().Be(expected.Name);
            actual.Data.Should().Be(expected.Data);
        }

        [Fact(Skip = "Not implemented yet")]
        public void Can_Serialize_CollectionWithObjectType_MsgPackCliRuntimeType()
        {
            AssertEqual(PolymorhphicMessages.ExampleListWithRuntimeType);
        }

        [Fact(Skip = "Not implemented yet")]
        public void Can_Serialize_DictionaryKeyWithObjectType_MsgPackCliRuntimeType()
        {
            AssertEqual(PolymorhphicMessages.ExampleDictionaryWithRuntimeType);
        }

        [Fact(Skip = "Not implemented yet")]
        public void Can_Serialize_TupleItemWithObjectType_MsgPackCliRuntimeType()
        {
            AssertEqual(PolymorhphicMessages.ExampleTupleWithRuntimeType);
        }

        [Fact]
        public void Can_Serialize_MessageWithObjectProperty_MsgPackCliKnownType()
        {
            AssertEqual(new PolymorhphicMessages.MsgPackWithObjectPropertyKnownType { Name = "John", Data = "Scott" });
            AssertEqual(new PolymorhphicMessages.MsgPackWithObjectPropertyKnownType { Name = "John", Data = new Uri("http://getakka.net") });
        }

        [Fact]
        public void Can_Serialize_MessageWithObjectProperty_MsgPackCliKnownType_AssertOnUnknownType()
        {
            var wrongKnownType = new PolymorhphicMessages.MsgPackWithObjectPropertyKnownType { Name = "John", Data = 435345345 };

            Action dangerousCode = () => AssertEqual(wrongKnownType);
            dangerousCode.ShouldThrow<SerializationException>();
        }

        [Fact(Skip = "Not implemented yet")]
        public void Can_Serialize_CollectionWithObjectType_MsgPackCliKnownType()
        {
            AssertEqual(PolymorhphicMessages.ExampleListWithKnownType);
        }

        [Fact(Skip = "Not implemented yet")]
        public void Can_Serialize_DictionaryKeyWithObjectType_MsgPackCliKnownType()
        {
            AssertEqual(PolymorhphicMessages.ExampleDictionaryWithKnownType);
        }

        [Fact(Skip = "Not implemented yet")]
        public void Can_Serialize_TupleItemWithObjectType_MsgPackCliKnownType()
        {
            AssertEqual(PolymorhphicMessages.ExampleTupleWithRuntimeType);
        }
    }

    public static class PolymorhphicMessages
    {
        public class MsgPackWithObjectPropertyRuntimeType
        {
            public string Name { get; set; }

            [MessagePackRuntimeType]
            public object Data { get; set; }
        }

        [MessagePackRuntimeCollectionItemType]
        public static List<object> ExampleListWithRuntimeType { get; } = new List<object> { 5, 7, 856, 34 };

        [MessagePackRuntimeDictionaryKeyType]
        public static Dictionary<object, string> ExampleDictionaryWithRuntimeType { get; } = new Dictionary<object, string>
        {
            [5] = "Name",
            [6] = "Name2",
            [7] = "Name3"
        };

        [MessagePackRuntimeTupleItemType(2)]
        public static Tuple<int, object> ExampleTupleWithRuntimeType { get; } = Tuple.Create<int, object>(25, "John");

        public class MsgPackWithObjectPropertyKnownType
        {
            public string Name { get; set; }

            [MessagePackKnownType("0", typeof(string))]
            [MessagePackKnownType("1", typeof(Uri))]
            public object Data { get; set; }

            protected bool Equals(MsgPackWithObjectPropertyKnownType other)
            {
                return string.Equals(Name, other.Name) && Data.Equals(other.Data);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != this.GetType()) return false;
                return Equals((MsgPackWithObjectPropertyKnownType)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return (Name.GetHashCode() * 397) ^ Data.GetHashCode();
                }
            }
        }

        [MessagePackKnownCollectionItemType("exlist", typeof(int))]
        public static List<object> ExampleListWithKnownType { get; } = new List<object> { 5, 7, 856, 34 };

        [MessagePackKnownDictionaryKeyTypeAttribute("exdict", typeof(int))]
        public static Dictionary<object, string> ExampleDictionaryWithKnownType { get; } = new Dictionary<object, string>
        {
            [5] = "Name",
            [6] = "Name2",
            [7] = "Name3"
        };

        [MessagePackKnownTupleItemTypeAttribute(2, "extuple", typeof(int))]
        public static Tuple<int, object> ExampleTupleWithKnownType { get; } = Tuple.Create<int, object>(25, "John");
    }
}