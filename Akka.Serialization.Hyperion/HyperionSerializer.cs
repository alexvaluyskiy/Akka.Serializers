using System;
using Akka.Actor;
using Hyperion;
using System.IO;

namespace Akka.Serialization
{
    public class HyperionSerializer : Serializer
    {
        private readonly Hyperion.Serializer _serializer;

        public HyperionSerializer(ExtendedActorSystem system) : base(system)
        {
            _serializer =
                new Hyperion.Serializer(new SerializerOptions());
        }

        public override byte[] ToBinary(object obj)
        {
            if (obj is IActorRef) return IActorRefSerializer((IActorRef)obj);
            if (obj is ActorPath) return ActorPathSerializer((ActorPath)obj);

            return ObjectSerializer(obj);
        }

        public override object FromBinary(byte[] bytes, Type type)
        {
            if (type == typeof(IActorRef)) return IActorRefDeserializer(bytes);
            if (type == typeof(ActorPath)) return ActorPathDeserializer(bytes);

            return ObjectDeserializer(bytes, type);
        }

        public override int Identifier => 33;

        public override bool IncludeManifest => false;

        private byte[] ObjectSerializer(object obj)
        {
            using (var ms = new MemoryStream())
            {
                _serializer.Serialize(obj, ms);
                return ms.ToArray();
            }
        }

        private object ObjectDeserializer(byte[] bytes, Type type)
        {
            using (var ms = new MemoryStream(bytes))
            {
                var res = _serializer.Deserialize<object>(ms);
                return res;
            }
        }

        private byte[] IActorRefSerializer(IActorRef actorRef)
        {
            var str = Serialization.SerializedActorPath(actorRef);
            return ObjectSerializer(str);
        }

        private object IActorRefDeserializer(byte[] bytes)
        {
            var path = (string)ObjectDeserializer(bytes, typeof(string));
            return system.Provider.ResolveActorRef(path);
        }

        private byte[] ActorPathSerializer(ActorPath obj)
        {
            var str = obj.ToSerializationFormat();
            return ObjectSerializer(str);
        }

        private object ActorPathDeserializer(byte[] bytes)
        {
            var path = (string)ObjectDeserializer(bytes, typeof(string));
            ActorPath actorPath;
            if (ActorPath.TryParse(path, out actorPath))
            {
                return actorPath;
            }

            return null;
        }
    }
}
