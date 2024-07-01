import 'package:injectable/injectable.dart';
import 'package:mona_desktop/core/di/scope_names.dart';
import 'package:mona_desktop/core/dto/dto_export.dart';
import 'package:mona_desktop/core/dto/message_request.dart';
import 'package:mona_desktop/repository/signalr_repository.dart';

@Injectable(scope: ScopeNames.message)
class ChatService {
  final SignalRRepository repository;

  ChatService({required this.repository});

  Future<List<MessageDto>> fetchMessagesByChatId(String chatId) async {
    List<dynamic> jsonResponse =
        await repository.invokeGetMessagesByChatId(chatId) as List<dynamic>;
    return jsonResponse.map((json) => MessageDto.fromJson(json)).toList();
  }

  Future sendMessage(MessageRequest messageRequest) async {
    await repository.sendMessage(messageRequest);
  }

  void receiveMessage(Function(MessageDto) onMessageReceived) {
    repository.receiveMessage((response) {
      var responseList = response as List<dynamic>;
      var message = MessageDto.fromJson(responseList[0]);
      onMessageReceived(message);
    });
  }

  void updateChat(Function(ChatDto) onChatReceived) {
    repository.updateChat((response) {
      var responseList = response as List<dynamic>;
      var chat = ChatDto.fromJson(responseList[0]);
      onChatReceived(chat);
    });
  }
}
