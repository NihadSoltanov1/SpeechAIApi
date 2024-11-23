using Grpc.Core;
using SpeechServiceLab.gRPC;
using SpeechServiceLab.Services;

namespace SpeechServiceLab.gRPC
{
    public class GrpcSpeechService : SpeechService.SpeechServiceBase
    {
        private readonly AzureSpeechService _speechService;
        private readonly AzureTranslationService _translationService;

        public GrpcSpeechService(AzureSpeechService speechService, AzureTranslationService translationService)
        {
            _speechService = speechService;
            _translationService = translationService;
        }

        public override async Task<SpeechResponse> SpeechToText(AudioRequest request, ServerCallContext context)
        {
            var text = await _speechService.SpeechToTextAsync(request.AudioData.ToByteArray());
            return new SpeechResponse { Text = text };
        }

        public override async Task<AudioResponse> TextToSpeech(TextRequest request, ServerCallContext context)
        {
            var audioData = await _speechService.TextToSpeechAsync(request.Text);
            return new AudioResponse { AudioData = Google.Protobuf.ByteString.CopyFrom(audioData) };
        }

        public override async Task<TranslateResponse> Translate(TranslateRequest request, ServerCallContext context)
        {
            var translatedText = await _translationService.TranslateTextAsync(request.Text, request.ToLanguage);
            return new TranslateResponse { TranslatedText = translatedText };
        }
    }
}
