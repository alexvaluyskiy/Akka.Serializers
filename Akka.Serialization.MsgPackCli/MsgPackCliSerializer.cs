using System;
using Akka.Actor;
using MsgPack.Serialization;

namespace Akka.Serialization.MsgPackCli
{
    public class MsgPackCliSerializer : Serializer
    {
        public MsgPackCliSerializer(ExtendedActorSystem system) : base(system)
        {
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


        public override int Identifier => 31;

        public override bool IncludeManifest { get; } = false;

        private byte[] ObjectSerializer(object obj)
        {
            var serializer = SerializationContext.Default.GetSerializer(obj.GetType());
            return serializer.PackSingleObject(obj);
        }

        private object ObjectDeserializer(byte[] bytes, Type type)
        {
            var serializer = SerializationContext.Default.GetSerializer(type);
            return serializer.UnpackSingleObject(bytes);
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
