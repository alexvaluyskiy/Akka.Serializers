using System;
using System.Runtime.Remoting.Messaging;
using Akka.Actor;
using Akka.Serializer.MsgPack.Resolvers;
using MessagePack;
using MessagePack.Resolvers;
using MessagePack.ImmutableCollection;

namespace Akka.Serialization.MsgPack
{
    public class MsgPackSerializer : Serializer
    {
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
            // TODO: hack to pass a context to formatters
            CallContext.SetData("ActorSystem", system);
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
