using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using Akka.Actor;
using Akka.Serializer.MsgPack.Resolvers;
using Akka.Util.Internal;
using MessagePack;
using MessagePack.Resolvers;
using MessagePack.ImmutableCollection;

namespace Akka.Serialization
{
    public class MsgPackSerializer : Serializer
    {
        internal static AsyncLocal<ActorSystem> LocalSystem = new AsyncLocal<ActorSystem>();

        static MsgPackSerializer()
        {
            CompositeResolver.RegisterAndSetAsDefault(
                ActorPathResolver.Instance,
                ActorRefResolver.Instance,
                PrimitiveObjectResolver.Instance,
                ImmutableCollectionResolver.Instance,
                ContractlessStandardResolver.Instance);
        }

        public MsgPackSerializer(ExtendedActorSystem system) : base(system)
        {
            LocalSystem.Value = system;
        }

        public override byte[] ToBinary(object obj)
        {
            if (obj is Exception) return SerializeException(obj as Exception);

            return MessagePackSerializer.NonGeneric.Serialize(obj.GetType(), obj);
        }

        public override object FromBinary(byte[] bytes, Type type)
        {
            if (typeof(Exception).IsAssignableFrom(type)) return DeserializeException(bytes, type);

            return MessagePackSerializer.NonGeneric.Deserialize(type, bytes);
        }

        public override int Identifier => 30;

        public override bool IncludeManifest => false;

        private byte[] SerializeException(Exception ex)
        {
            var exceptionSurrogate = ExceptionToSurrogate(ex);
            return MessagePackSerializer.Serialize(exceptionSurrogate);
        }

        private object DeserializeException(byte[] bytes, Type underlyingType)
        {
            var surrogate = MessagePackSerializer.Deserialize<ExceptionSurrogate>(bytes);

            return SurrogateToException(surrogate, underlyingType);
        }

        private ExceptionSurrogate ExceptionToSurrogate(Exception ex)
        {
            if (ex == null) return null;

            var exceptionSurrogate = new ExceptionSurrogate();
            exceptionSurrogate.ClassName = ex.GetType().FullName;
            exceptionSurrogate.Message = ex.Message;
            exceptionSurrogate.StackTraceString = ex.StackTrace;
            exceptionSurrogate.Source = ex.Source;
            exceptionSurrogate.InnerException = ExceptionToSurrogate(ex.InnerException);
            exceptionSurrogate.HResult = ex.HResult;

            string[] exclude =
            {
                "ClassName",
                "Message",
                "StackTraceString",
                "Source",
                "InnerException",
                "HelpURL",
                "RemoteStackTraceString",
                "RemoteStackIndex",
                "ExceptionMethod",
                "HResult",
                "Data",
                "TargetSite",
                "HelpLink",
                "StackTrace"
            };

#if SERIALIZABLE
            var serializationInfo = new SerializationInfo(ex.GetType(), new FormatterConverter());
            ex.GetObjectData(serializationInfo, new StreamingContext());

            foreach (var serializationEntry in serializationInfo)
            {
                if (exclude.Contains(serializationEntry.Name)) continue;

                var serializedValue = new SerializedExceptionValue
                {
                    Key = serializationEntry.Name,
                    TypeQualifiedName = serializationEntry.ObjectType.AssemblyQualifiedName,
                    Value = serializationEntry.Value
                };

                exceptionSurrogate.Properties.Add(serializedValue);
            }
#else
            var properties = ex.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in ex.GetType().GetTypeInfo().DeclaredProperties)
            {
                if (exclude.Contains(property.Name)) continue;

                var serializedValue = new SerializedExceptionValue
                {
                    Key = property.Name,
                    TypeQualifiedName = property.PropertyType.FullName,
                    Value = property.GetValue(ex)
                };

                exceptionSurrogate.Properties.Add(serializedValue);
            }
#endif
            return exceptionSurrogate;
        }

        private Exception SurrogateToException(ExceptionSurrogate surrogate, Type underlyingType)
        {
            Exception obj = null;
#if SERIALIZABLE
            var serializationInfo = new SerializationInfo(underlyingType, new FormatterConverter());
            serializationInfo.AddValue("ClassName", surrogate.ClassName);
            serializationInfo.AddValue("Message", surrogate.Message);
            serializationInfo.AddValue("StackTraceString", surrogate.StackTraceString);
            serializationInfo.AddValue("Source", surrogate.Source);
            serializationInfo.AddValue("HelpURL", "");
            serializationInfo.AddValue("RemoteStackTraceString", null);
            serializationInfo.AddValue("RemoteStackIndex", 0);
            serializationInfo.AddValue("ExceptionMethod", null);
            serializationInfo.AddValue("HResult", surrogate.HResult);

            var innerException = surrogate.InnerException == null ? null : SurrogateToException(surrogate.InnerException, Type.GetType(surrogate.InnerException.ClassName));
            serializationInfo.AddValue("InnerException", innerException);

            foreach (var property in surrogate.Properties)
            {
                serializationInfo.AddValue(
                    property.Key,
                    Convert.ChangeType(property.Value, Type.GetType(property.TypeQualifiedName)));
            }

            ConstructorInfo constructorInfo = underlyingType.GetConstructor(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                new[] { typeof(SerializationInfo), typeof(StreamingContext) },
                null);

            if (constructorInfo != null)
            {
                object[] args = { serializationInfo, new StreamingContext() };
                obj = constructorInfo.Invoke(args).AsInstanceOf<Exception>();
            }
#else
            const BindingFlags All = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            TypeInfo ExceptionTypeInfo = typeof(Exception).GetTypeInfo();

            obj = (Exception)Activator.CreateInstance(underlyingType);
            ExceptionTypeInfo.GetField("_message", All).SetValue(obj, surrogate.Message);
            if (surrogate.Source != null) ExceptionTypeInfo.GetField("_source", All).SetValue(obj, surrogate.Source);
            if (!string.IsNullOrEmpty(surrogate.StackTraceString)) ExceptionTypeInfo.GetField("_stackTraceString", All).SetValue(obj, surrogate.StackTraceString);
            if (surrogate.InnerException != null)
            {
                var innerException = SurrogateToException(surrogate.InnerException, Type.GetType(surrogate.InnerException.ClassName));
                ExceptionTypeInfo.GetField("_innerException", All).SetValue(obj, innerException);
            }

            foreach (var property in surrogate.Properties)
            {
                underlyingType.GetProperty(property.Key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty).SetValue(obj, property.Value);
            }
#endif

            return obj;
        }
    }

    public class ExceptionSurrogate
    {
        public string ClassName { get; set; }

        public string Message { get; set; }

        public string StackTraceString { get; set; }

        public string Source { get; set; }

        public int HResult { get; set; }

        public ExceptionSurrogate InnerException { get; set; }

        public List<SerializedExceptionValue> Properties { get; set; } = new List<SerializedExceptionValue>();
    }

    public class SerializedExceptionValue
    {
        public string Key { get; set; }

        public string TypeQualifiedName { get; set; }

        public object Value { get; set; }
    }
}
