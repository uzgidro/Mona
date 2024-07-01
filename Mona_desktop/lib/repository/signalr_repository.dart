import 'package:injectable/injectable.dart';
import 'package:mona_desktop/core/di/scope_names.dart';
import 'package:mona_desktop/core/dto/message_request.dart';
import 'package:signalr_netcore/signalr_client.dart';

@Injectable(scope: ScopeNames.message)
class SignalRRepository {
  final HubConnection hubConnection;

  SignalRRepository({required this.hubConnection});

  Future startConnection() async {
    if (hubConnection.state?.index == 0) {
      await hubConnection.start();
    }
  }

  Future invokeGetChats() async {
    return await hubConnection.invoke(HubMethods.getChats);
  }

  Future invokeGetUsers() async {
    return await hubConnection.invoke(HubMethods.getUsers);
  }

  Future invokeGetMessagesByChatId(String chatId) async {
    return await hubConnection
        .invoke(HubMethods.getMessagesByChatId, args: [chatId]);
  }

  Future sendMessage(MessageRequest messageRequest) async {
    return await hubConnection
        .send(HubMethods.sendMessage, args: [messageRequest]);
  }

  void receiveMessage(Function(List<Object?>?) method) {
    hubConnection.on(HubListeners.receiveMessage, method);
  }

  void updateChat(Function(List<Object?>?) method) {
    hubConnection.on(HubListeners.updateChat, method);
  }
}

class HubMethods {
  static const String getChats = 'getChats';
  static const String getUsers = 'getUsers';
  static const String getMessagesByChatId = 'getMessagesByChatId';
  static const String sendMessage = 'sendMessage';
}

class HubListeners {
  static const String receiveMessage = 'ReceiveMessage';
  static const String updateChat = 'UpdateChat';
// static const String getMessagesByChatId = 'getMessagesByChatId';
// static const String sendMessage = 'sendMessage';
}
