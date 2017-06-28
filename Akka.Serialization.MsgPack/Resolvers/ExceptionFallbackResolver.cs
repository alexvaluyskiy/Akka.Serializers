using System;
using System.Reflection;
using MessagePack;
using MessagePack.Formatters;

namespace Akka.Serialization.MsgPack.Resolvers
{
    public class ExceptionFallbackResolver : IFormatterResolver
    {
        public static readonly IFormatterResolver Instance = new ExceptionFallbackResolver();
        ExceptionFallbackResolver() { }

        public IMessagePackFormatter<T> GetFormatter<T>() => FormatterCache<T>.Formatter;

        static class FormatterCache<T>
        {
            public static readonly IMessagePackFormatter<T> Formatter;
            static FormatterCache() => Formatter = (IMessagePackFormatter<T>)ExceptionFallbackFormatterHelper.GetFormatter<T>();
        }
    }

    internal static class ExceptionFallbackFormatterHelper
    {
        internal static object GetFormatter<T>()
        {
            return typeof(Exception).IsAssignableFrom(typeof(T)) ? new ExceptionFallbackFormatter<T>() : null;
        }
    }

    public class ExceptionFallbackFormatter<T> : IMessagePackFormatter<T>
    {
        public int Serialize(ref byte[] bytes, int offset, T value, IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }

            var exception = value as Exception;
            
            var startOffset = offset;
            offset += MessagePackBinary.WriteString(ref bytes, offset, exception?.GetType().FullName);
            offset += MessagePackBinary.WriteString(ref bytes, offset, exception?.Message);
            offset += MessagePackBinary.WriteString(ref bytes, offset, exception?.StackTrace);
            offset += MessagePackBinary.WriteString(ref bytes, offset, exception?.Source);
            //offset += MessagePackBinary.WriteString(ref bytes, offset, null);
            offset += MessagePackBinary.WriteInt32(ref bytes, offset, exception?.HResult ?? 0);

            return offset - startOffset;
        }

        public T Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return default(T);
            }

            int startOffset = offset;

            const BindingFlags All = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
            TypeInfo ExceptionTypeInfo = typeof(Exception).GetTypeInfo();
            var obj = Activator.CreateInstance(typeof(T));

            var className = MessagePackBinary.ReadString(bytes, offset, out readSize);
            offset += readSize;
            var message = MessagePackBinary.ReadString(bytes, offset, out readSize);
            offset += readSize;
            var stackTrace = MessagePackBinary.ReadString(bytes, offset, out readSize);
            offset += readSize;
            var source = MessagePackBinary.ReadString(bytes, offset, out readSize);
            offset += readSize;
            var hResult = MessagePackBinary.ReadInt32(bytes, offset, out readSize);
            offset += readSize;
            
            ExceptionTypeInfo?.GetField("_message", All)?.SetValue(obj, message);
            if (!string.IsNullOrEmpty(stackTrace)) ExceptionTypeInfo?.GetField("_stackTraceString", All)?.SetValue(obj, stackTrace);
            if (!string.IsNullOrEmpty(source)) ExceptionTypeInfo?.GetField("_source", All)?.SetValue(obj, source);

            readSize = offset - startOffset;
            return (T)obj;
        }
    }
}
