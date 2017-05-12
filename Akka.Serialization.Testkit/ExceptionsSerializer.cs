using FluentAssertions;
using System;
using System.Runtime.Serialization;
using Xunit;

namespace Akka.Serialization.Testkit
{
    public abstract class ExceptionsSerializerTests : TestKit.Xunit.TestKit
    {
        protected ExceptionsSerializerTests(Type serializerType) : base(ConfigFactory.GetConfig(serializerType))
        {
        }

        [Fact]
        public void Can_Serialize_Exception()
        {
            var exception = new SampleExceptions.BasicException();
            AssertAndReturn(exception).Should().BeOfType<SampleExceptions.BasicException>();
        }

        [Fact]
        public void Can_Serialize_ExceptionWithMessage()
        {
            var exception = new SampleExceptions.BasicException("Some message");
            AssertAndReturn(exception).Should().BeOfType<SampleExceptions.BasicException>();
        }

        [Fact]
        public void Can_Serialize_ExceptionWithMessageAndInnerException()
        {
            var exception = new SampleExceptions.BasicException("Some message", new ArgumentNullException());
            AssertAndReturn(exception).Should().BeOfType<SampleExceptions.BasicException>();
        }

        [Fact]
        public void Can_Serialize_ExceptionWithStackTrace()
        {
            try
            {
                throw new SampleExceptions.BasicException();
            }
            catch (SampleExceptions.BasicException ex)
            {
                var actual = AssertAndReturn(ex);
                AssertException(ex, actual);
            }
        }

        [Fact]
        public void Can_Serialize_ExceptionWithCustomFields()
        {
            var exception = new SampleExceptions.ExceptionWithCustomFields("Some message", "John", 16);
            var actual = AssertAndReturn(exception);
            AssertException(exception, actual);
            actual.Name.Should().Be(exception.Name);
            actual.Age.Should().Be(exception.Age);
        }

        private void AssertException(Exception expected, Exception actual)
        {
            if (expected == null && actual == null) return;
            actual.Should().BeOfType(expected.GetType());
            actual.Message.Should().Be(expected.Message);
            actual.StackTrace.Should().Be(expected.StackTrace);
            actual.Source.Should().Be(expected.Source);
            AssertException(expected.InnerException, actual.InnerException);
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

    public static class SampleExceptions
    {
        public class BasicException : Exception
        {
            public BasicException()
            {
            }

            public BasicException(string message) : base(message)
            {
            }

            public BasicException(string message, Exception innerException) : base(message, innerException)
            {
            }

            protected BasicException(SerializationInfo info, StreamingContext context) : base(info, context)
            {
            }
        }

        public class ExceptionWithCustomFields : Exception
        {
            private string name;
            private int age;

            public ExceptionWithCustomFields()
            {
            }

            public ExceptionWithCustomFields(string message, string name, int age) 
                : this(message, name, age, null)
            {

            }

            public ExceptionWithCustomFields(string message, string name, int age, Exception innerException) 
                : base(message, innerException)
            {
                this.name = name;
                this.age = age;
            }

            protected ExceptionWithCustomFields(SerializationInfo info, StreamingContext context) : base(info, context)
            {
                name = info.GetString("Name");
                age = info.GetInt32("Age");
            }

            public string Name => name;
            public int Age => age;

            public override void GetObjectData(SerializationInfo info, StreamingContext context)
            {
                if (info == null)
                {
                    throw new ArgumentNullException(nameof(info));
                }

                info.AddValue("Name", name);
                info.AddValue("Age", age);
                base.GetObjectData(info, context);
            }
        }
    }
}
