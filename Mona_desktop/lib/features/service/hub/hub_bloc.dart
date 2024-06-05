import 'package:bloc/bloc.dart';
import 'package:equatable/equatable.dart';
import 'package:injectable/injectable.dart';
import 'package:meta/meta.dart';
import 'package:mona_desktop/core/dto/dto_export.dart';
import 'package:mona_desktop/core/dto/message_dto.dart';
import 'package:mona_desktop/core/middleware/jwt_service.dart';
import 'package:mona_desktop/repository/repository_export.dart';
import 'package:signalr_netcore/signalr_client.dart';
import 'package:talker_flutter/talker_flutter.dart';

part 'hub_event.dart';
part 'hub_state.dart';

@LazySingleton()
class HubBloc extends Bloc<HubEvent, HubState> {
  HubBloc(this.authRepository, this.jwtService, this.talker, this.hubConnection)
      : super(HubInitial()) {
    on<StartConnection>((event, emit) async {
      try {
        await hubConnection.start();
        var jsonResponse =
            await hubConnection.invoke('getChats') as List<dynamic>;
        List<ChatDto> chatList =
            jsonResponse.map((json) => ChatDto.fromJson(json)).toList();

        // TODO(): Add loading until emitted
        emit(HubStarted(chatList: chatList));
      } catch (e, st) {
        talker.handle(e, st);
      }
    });

    on<GetChatMessages>((event, emit) async {
      try {
        var jsonResponse = await hubConnection
            .invoke('getChatMessages', args: [event.chatId]) as List<dynamic>;
        List<MessageDto> messages =
            jsonResponse.map((json) => MessageDto.fromJson(json)).toList();

        // TODO(): Add loading until emitted
        emit(ChatLoaded(messages: messages));
      } catch (e, st) {
        talker.handle(e, st);
      }
    });
  }

  final HubConnection hubConnection;
  final AbstractAuthRepository authRepository;
  final JwtService jwtService;
  final Talker talker;
}
