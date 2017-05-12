using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Akka.Serialization.Testkit
{
    public abstract class CustomMessagesTests : TestKit.Xunit.TestKit
    {
        protected CustomMessagesTests(Type serializerType) : base(ConfigFactory.GetConfig(serializerType))
        {
        }

        [Fact]
        public virtual void Can_serialize_EmptyMessage()
        {
            var message = new CustomMessage.EmptyMessage();
            AssertAndReturn(message).Should().BeOfType<CustomMessage.EmptyMessage>();
        }

        [Fact]
        public virtual void Can_serialize_EmptySingleton()
        {
            var message = CustomMessage.EmptySingleton.Instance;
            AssertAndReturn(message).Should().BeOfType<CustomMessage.EmptySingleton>();
        }

        [Fact]
        public void Can_Serialize_MessageWithPublicSetters()
        {
            var actual = new CustomMessage.MessageWithPublicSetters()
            {
                Name = "John",
                Age = 15
            };
            AssertEqual(actual);
        }

        [Fact]
        public virtual void Can_Serialize_ImmutableMessageWithReadonlyFields()
        {
            var actual = new CustomMessage.ImmutableMessageWithPublicFields();
            actual.Name = "John";
            actual.Age = 15;
            AssertEqual(actual);
        }

        [Fact]
        public virtual void Can_Serialize_MessageWithObjectPropertyPrimitive()
        {
            var actual = new CustomMessage.ImmutableMessageWithObjectTypes()
            {
                Name = "John",
                Data = 34534534
            };
            AssertEqual(actual);
        }

        [Fact]
        public virtual void Can_Serialize_MessageWithObjectPropertyComplex()
        {
            var actual = new CustomMessage.ImmutableMessageWithObjectTypes()
            {
                Name = "John",
                Data = new CustomMessage.MessageWithPublicSetters()
                {
                    Name = "John",
                    Age = 15
                }
            };
            AssertEqual(actual);
        }

        [Fact]
        public void Can_Serialize_ImmutableMessageWithGenericTypes()
        {
            var actual = new CustomMessage.ImmutableMessageWithGenericTypes<CustomMessage.MessageWithPublicSetters>()
            {
                Name = "John",
                Data = new CustomMessage.MessageWithPublicSetters()
                {
                    Name = "John",
                    Age = 15
                }
            };
            AssertEqual(actual);
        }

        [Fact]
        public virtual void Can_Serialize_ImmutableMessage()
        {
            var actual = new CustomMessage.ImmutableMessage("John", 15);
            AssertEqual(actual);
        }

        [Fact]
        public virtual void Can_Serialize_ImmutableMessageWithDefaultParameters()
        {
            var actual = new CustomMessage.ImmutableMessageWithDefaultParameters("John");
            AssertEqual(actual);
        }

        [Fact]
        public virtual void Can_Serialize_ImmutableMessageWithTwoConstructors()
        {
            var actual = new CustomMessage.ImmutableMessageWithDefaultParameters("John");
            AssertEqual(actual);
        }

        protected T AssertAndReturn<T>(T message)
        {
            var serializer = Sys.Serialization.FindSerializerFor(message);
            var serialized = serializer.ToBinary(message);
            var result = serializer.FromBinary(serialized, typeof(T));
            return (T)result;
        }

        protected void AssertEqual<T>(T message)
        {
            var deserialized = AssertAndReturn(message);
            Assert.Equal(message, deserialized);
        }
    }

    public static class CustomMessage
    {
        public class EmptyMessage { }

        public class EmptySingleton
        {
            public static EmptySingleton Instance { get; } = new EmptySingleton();

            private EmptySingleton() { }
        }

        public class MessageWithPublicSetters
        {
            public int Age { get; set; }

            public string Name { get; set; }

            private bool Equals(MessageWithPublicSetters other)
            {
                return String.Equals(Name, (string)other.Name) && Age == other.Age;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                return obj is MessageWithPublicSetters && Equals((MessageWithPublicSetters)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ Age;
                }
            }
        }

        public class ImmutableMessageWithPublicFields
        {
            public int Age;

            public string Name;

            private bool Equals(ImmutableMessageWithPublicFields other)
            {
                return string.Equals(Name, other.Name) && Age == other.Age;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                return obj is ImmutableMessageWithPublicFields && Equals((ImmutableMessageWithPublicFields)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ Age;
                }
            }
        }

        public class ImmutableMessageWithObjectTypes
        {
            public string Name { get; set; }

            public object Data { get; set; }

            private bool Equals(ImmutableMessageWithObjectTypes other)
            {
                return String.Equals(Name, other.Name) && Equals(Data, other.Data);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                return obj is ImmutableMessageWithObjectTypes && Equals((ImmutableMessageWithObjectTypes)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (Data != null ? Data.GetHashCode() : 0);
                }
            }
        }

        public sealed class ImmutableMessageWithGenericTypes<T>
        {
            public string Name { get; set; }

            public T Data { get; set; }

            private bool Equals(ImmutableMessageWithGenericTypes<T> other)
            {
                return String.Equals(Name, (string)other.Name) && EqualityComparer<T>.Default.Equals(Data, other.Data);
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                return obj is ImmutableMessageWithGenericTypes<T> && Equals((ImmutableMessageWithGenericTypes<T>)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ EqualityComparer<T>.Default.GetHashCode(Data);
                }
            }
        }

        public class ImmutableMessage
        {
            public ImmutableMessage(string name, int age)
            {
                Age = age;
                Name = name;
            }

            public int Age { get; }

            public string Name { get; }

            private bool Equals(ImmutableMessage other)
            {
                return String.Equals(Name, (string)other.Name) && Age == other.Age;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                return obj is ImmutableMessage && Equals((ImmutableMessage)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ Age;
                }
            }
        }

        public class ImmutableMessageWithDefaultParameters
        {
            public ImmutableMessageWithDefaultParameters(string name, int age = 10)
            {
                Name = name;
                Age = age;
            }

            public int Age { get; }

            public string Name { get; }

            private bool Equals(ImmutableMessageWithDefaultParameters other)
            {
                return String.Equals(Name, (string)other.Name) && Age == other.Age;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                return obj is ImmutableMessageWithDefaultParameters && Equals((ImmutableMessageWithDefaultParameters)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ Age;
                }
            }
        }

        public class ImmutableMessageWithTwoConstructors
        {
            public ImmutableMessageWithTwoConstructors(int age, string name)
            {
                Age = age;
                Name = name;
            }

            public ImmutableMessageWithTwoConstructors(string name)
            {
                Name = name;
                Age = 4;
            }

            public int Age { get; }

            public string Name { get; }

            private bool Equals(ImmutableMessageWithTwoConstructors other)
            {
                return String.Equals(Name, (string)other.Name) && Age == other.Age;
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                return obj is ImmutableMessageWithTwoConstructors && Equals((ImmutableMessageWithTwoConstructors)obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ Age;
                }
            }
        }
    }
}
