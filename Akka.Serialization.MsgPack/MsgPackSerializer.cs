using System;
using System.Threading;
using Akka.Actor;
using Akka.Serialization.MsgPack.Resolvers;
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
                SerializableResolver.Instance,
                //ExceptionFallbackResolver.Instance,
                AkkaResolver.Instance,
                ImmutableCollectionResolver.Instance,              
                TypelessContractlessStandardResolver.Instance);
        }

        public MsgPackSerializer(ExtendedActorSystem system) : base(system)
        {
            LocalSystem.Value = system;
        }

        public override byte[] ToBinary(object obj)
        {
            return MessagePackSerializer.NonGeneric.Serialize(obj.GetType(), obj);
        }

        public override object FromBinary(byte[] bytes, Type type)
        {
            return MessagePackSerializer.NonGeneric.Deserialize(type, bytes);
        }

        public override int Identifier => 30;

        public override bool IncludeManifest => false;
    }
}
