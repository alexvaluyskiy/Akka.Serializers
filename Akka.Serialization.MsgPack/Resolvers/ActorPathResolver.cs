using System;
using System.Collections.Generic;
using Akka.Actor;
using MessagePack;
using MessagePack.Formatters;

namespace Akka.Serializer.MsgPack.Resolvers
{
    public class ActorPathResolver : IFormatterResolver
    {
        // Resolver should be singleton.
        public static IFormatterResolver Instance = new ActorPathResolver();

        ActorPathResolver()
        {
        }

        // GetFormatter<T>'s get cost should be minimized so use type cache.
        public IMessagePackFormatter<T> GetFormatter<T>()
        {
            return FormatterCache<T>.Formatter;
        }

        static class FormatterCache<T>
        {
            public static readonly IMessagePackFormatter<T> Formatter;

            // generic's static constructor should be minimized for reduce type generation size!
            // use outer helper method.
            static FormatterCache()
            {
                Formatter = (IMessagePackFormatter<T>)ActorPathResolverGetFormatterHelper.GetFormatter(typeof(T));
            }
        }
    }

    internal static class ActorPathResolverGetFormatterHelper
    {
        // If type is concrete type, use type-formatter map
        static readonly Dictionary<Type, object> FormatterMap = new Dictionary<Type, object>()
        {
            {typeof(ActorPath), new ActorPathFormatter<ActorPath>()},
            {typeof(ChildActorPath), new ActorPathFormatter<ChildActorPath>()},
            {typeof(RootActorPath), new ActorPathFormatter<RootActorPath>()}
        };

        internal static object GetFormatter(Type t)
        {
            object formatter;
            if (FormatterMap.TryGetValue(t, out formatter))
            {
                return formatter;
            }

            // If type can not get, must return null for fallback mecanism.
            return null;
        }
    }

    public class ActorPathFormatter<T> : IMessagePackFormatter<T> where T : ActorPath
    {
        public int Serialize(ref byte[] bytes, int offset, T value, IFormatterResolver formatterResolver)
        {
            if (value == null)
            {
                return MessagePackBinary.WriteNil(ref bytes, offset);
            }

            var startOffset = offset;
            offset += MessagePackBinary.WriteString(ref bytes, offset, value.ToSerializationFormat());
            return offset - startOffset;
        }

        public T Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
        {
            if (MessagePackBinary.IsNil(bytes, offset))
            {
                readSize = 1;
                return null;
            }

            var path = MessagePackBinary.ReadString(bytes, offset, out readSize);

            ActorPath actorPath;
            if (ActorPath.TryParse(path, out actorPath))
            {
                return (T)actorPath;
            }

            return null;
        }
    }
}
