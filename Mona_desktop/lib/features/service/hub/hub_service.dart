import 'package:injectable/injectable.dart';
import 'package:mona_desktop/core/dto/dto_export.dart';
import 'package:mona_desktop/repository/signalr_repository.dart';

@Injectable()
class HubService {
  final SignalRRepository repository;

  HubService({required this.repository});

  Future startConnection() async {
    await repository.startConnection();
  }

  Future<List<ChatDto>> fetchChats() async {
    var jsonResponse = await repository.invokeGetChats() as List<dynamic>;
    return jsonResponse.map((json) => ChatDto.fromJson(json)).toList();
  }

  Future<List<UserDto>> fetchContacts() async {
    var jsonResponse = await repository.invokeGetUsers() as List<dynamic>;
    return jsonResponse.map((json) => UserDto.fromJson(json)).toList();
  }
}
