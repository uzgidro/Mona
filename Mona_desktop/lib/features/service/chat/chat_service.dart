import 'package:injectable/injectable.dart';
import 'package:mona_desktop/core/dto/dto_export.dart';
import 'package:mona_desktop/features/service/signalr_repository.dart';

@Injectable()
class ChatService {
  final SignalRRepository repository;

  ChatService({required this.repository});

  Future<List<MessageDto>> fetchMessagesByChatId(String chatId) async {
    List<dynamic> jsonResponse =
        await repository.invokeGetMessagesByChatId(chatId) as List<dynamic>;
    return jsonResponse.map((json) => MessageDto.fromJson(json)).toList();
  }
}
