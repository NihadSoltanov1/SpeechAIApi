// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: Services/speech_service.proto
// </auto-generated>
#pragma warning disable 0414, 1591, 8981, 0612
#region Designer generated code

using grpc = global::Grpc.Core;

namespace SpeechServiceLab.Grpc {
  public static partial class SpeechService
  {
    static readonly string __ServiceName = "SpeechService";

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::SpeechServiceLab.Grpc.SpeechToTextRequest> __Marshaller_SpeechToTextRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::SpeechServiceLab.Grpc.SpeechToTextRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::SpeechServiceLab.Grpc.SpeechToTextResponse> __Marshaller_SpeechToTextResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::SpeechServiceLab.Grpc.SpeechToTextResponse.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::SpeechServiceLab.Grpc.TextToSpeechRequest> __Marshaller_TextToSpeechRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::SpeechServiceLab.Grpc.TextToSpeechRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::SpeechServiceLab.Grpc.TextToSpeechResponse> __Marshaller_TextToSpeechResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::SpeechServiceLab.Grpc.TextToSpeechResponse.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::SpeechServiceLab.Grpc.TranslateRequest> __Marshaller_TranslateRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::SpeechServiceLab.Grpc.TranslateRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::SpeechServiceLab.Grpc.TranslateResponse> __Marshaller_TranslateResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::SpeechServiceLab.Grpc.TranslateResponse.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::SpeechServiceLab.Grpc.SpeechToTextRequest, global::SpeechServiceLab.Grpc.SpeechToTextResponse> __Method_SpeechToText = new grpc::Method<global::SpeechServiceLab.Grpc.SpeechToTextRequest, global::SpeechServiceLab.Grpc.SpeechToTextResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "SpeechToText",
        __Marshaller_SpeechToTextRequest,
        __Marshaller_SpeechToTextResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::SpeechServiceLab.Grpc.TextToSpeechRequest, global::SpeechServiceLab.Grpc.TextToSpeechResponse> __Method_TextToSpeech = new grpc::Method<global::SpeechServiceLab.Grpc.TextToSpeechRequest, global::SpeechServiceLab.Grpc.TextToSpeechResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "TextToSpeech",
        __Marshaller_TextToSpeechRequest,
        __Marshaller_TextToSpeechResponse);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::SpeechServiceLab.Grpc.TranslateRequest, global::SpeechServiceLab.Grpc.TranslateResponse> __Method_Translate = new grpc::Method<global::SpeechServiceLab.Grpc.TranslateRequest, global::SpeechServiceLab.Grpc.TranslateResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "Translate",
        __Marshaller_TranslateRequest,
        __Marshaller_TranslateResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::SpeechServiceLab.Grpc.SpeechServiceReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of SpeechService</summary>
    [grpc::BindServiceMethod(typeof(SpeechService), "BindService")]
    public abstract partial class SpeechServiceBase
    {
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::SpeechServiceLab.Grpc.SpeechToTextResponse> SpeechToText(global::SpeechServiceLab.Grpc.SpeechToTextRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::SpeechServiceLab.Grpc.TextToSpeechResponse> TextToSpeech(global::SpeechServiceLab.Grpc.TextToSpeechRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::System.Threading.Tasks.Task<global::SpeechServiceLab.Grpc.TranslateResponse> Translate(global::SpeechServiceLab.Grpc.TranslateRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for SpeechService</summary>
    public partial class SpeechServiceClient : grpc::ClientBase<SpeechServiceClient>
    {
      /// <summary>Creates a new client for SpeechService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public SpeechServiceClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for SpeechService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public SpeechServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected SpeechServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected SpeechServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::SpeechServiceLab.Grpc.SpeechToTextResponse SpeechToText(global::SpeechServiceLab.Grpc.SpeechToTextRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return SpeechToText(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::SpeechServiceLab.Grpc.SpeechToTextResponse SpeechToText(global::SpeechServiceLab.Grpc.SpeechToTextRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_SpeechToText, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::SpeechServiceLab.Grpc.SpeechToTextResponse> SpeechToTextAsync(global::SpeechServiceLab.Grpc.SpeechToTextRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return SpeechToTextAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::SpeechServiceLab.Grpc.SpeechToTextResponse> SpeechToTextAsync(global::SpeechServiceLab.Grpc.SpeechToTextRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_SpeechToText, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::SpeechServiceLab.Grpc.TextToSpeechResponse TextToSpeech(global::SpeechServiceLab.Grpc.TextToSpeechRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return TextToSpeech(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::SpeechServiceLab.Grpc.TextToSpeechResponse TextToSpeech(global::SpeechServiceLab.Grpc.TextToSpeechRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_TextToSpeech, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::SpeechServiceLab.Grpc.TextToSpeechResponse> TextToSpeechAsync(global::SpeechServiceLab.Grpc.TextToSpeechRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return TextToSpeechAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::SpeechServiceLab.Grpc.TextToSpeechResponse> TextToSpeechAsync(global::SpeechServiceLab.Grpc.TextToSpeechRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_TextToSpeech, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::SpeechServiceLab.Grpc.TranslateResponse Translate(global::SpeechServiceLab.Grpc.TranslateRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return Translate(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::SpeechServiceLab.Grpc.TranslateResponse Translate(global::SpeechServiceLab.Grpc.TranslateRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_Translate, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::SpeechServiceLab.Grpc.TranslateResponse> TranslateAsync(global::SpeechServiceLab.Grpc.TranslateRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return TranslateAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::SpeechServiceLab.Grpc.TranslateResponse> TranslateAsync(global::SpeechServiceLab.Grpc.TranslateRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_Translate, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected override SpeechServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new SpeechServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static grpc::ServerServiceDefinition BindService(SpeechServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_SpeechToText, serviceImpl.SpeechToText)
          .AddMethod(__Method_TextToSpeech, serviceImpl.TextToSpeech)
          .AddMethod(__Method_Translate, serviceImpl.Translate).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    public static void BindService(grpc::ServiceBinderBase serviceBinder, SpeechServiceBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_SpeechToText, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::SpeechServiceLab.Grpc.SpeechToTextRequest, global::SpeechServiceLab.Grpc.SpeechToTextResponse>(serviceImpl.SpeechToText));
      serviceBinder.AddMethod(__Method_TextToSpeech, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::SpeechServiceLab.Grpc.TextToSpeechRequest, global::SpeechServiceLab.Grpc.TextToSpeechResponse>(serviceImpl.TextToSpeech));
      serviceBinder.AddMethod(__Method_Translate, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::SpeechServiceLab.Grpc.TranslateRequest, global::SpeechServiceLab.Grpc.TranslateResponse>(serviceImpl.Translate));
    }

  }
}
#endregion
