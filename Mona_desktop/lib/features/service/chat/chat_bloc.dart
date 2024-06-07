import 'package:bloc/bloc.dart';
import 'package:equatable/equatable.dart';
import 'package:injectable/injectable.dart';
import 'package:meta/meta.dart';
import 'package:mona_desktop/core/dto/message_dto.dart';
import 'package:signalr_netcore/signalr_client.dart';
import 'package:talker_flutter/talker_flutter.dart';

part 'chat_event.dart';
part 'chat_state.dart';

@LazySingleton()
class ChatBloc extends Bloc<ChatEvent, ChatState> {
  ChatBloc(this.hubConnection, this.talker) : super(ChatInitial()) {
    on<OpenChat>((event, emit) async {
      try {
        var chatId = event.chatId;
        List<dynamic> jsonResponse = [];
        if (event.chatId == null) {
          jsonResponse = await hubConnection.invoke('getMessagesByUserId',
              args: [event.receiverId]) as List<dynamic>;
        } else {
          jsonResponse = await hubConnection.invoke('getChatMessages',
              args: [event.chatId!]) as List<dynamic>;
        }
        List<MessageDto> messages =
            jsonResponse.map((json) => MessageDto.fromJson(json)).toList();

        if (messages.isNotEmpty && chatId == null) {
          chatId = messages[0].chatId;
        }

        emit(ChatOpened(
            chatId: chatId,
            chatName: event.chatName,
            receiverId: event.receiverId));

        // TODO(): Add loading until emitted
        emit(ChatLoaded(messages: messages));
      } catch (e, st) {
        talker.handle(e, st);
      }
    });
  }

  final HubConnection hubConnection;
  final Talker talker;
}
