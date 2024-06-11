import 'package:injectable/injectable.dart';
import 'package:signalr_netcore/signalr_client.dart';

@Injectable()
class SignalRRepository {
  final HubConnection hubConnection;

  SignalRRepository({required this.hubConnection});

  Future startConnection() async {
    await hubConnection.start();
  }

  Future invokeGetChats() async {
    return hubConnection.invoke(HubMethods.getChats);
  }

  Future invokeGetUsers() async {
    return hubConnection.invoke(HubMethods.getUsers);
  }

  Future invokeGetMessagesByChatId(String chatId) async {
    return hubConnection.invoke(HubMethods.getMessagesByChatId, args: [chatId]);
  }
}

class HubMethods {
  static const String getChats = 'getChats';
  static const String getUsers = 'getUsers';
  static const String getMessagesByChatId = 'getMessagesByChatId';
}
