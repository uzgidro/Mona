import 'package:bloc/bloc.dart';
import 'package:equatable/equatable.dart';
import 'package:injectable/injectable.dart';
import 'package:meta/meta.dart';
import 'package:mona_desktop/core/dto/message_dto.dart';
import 'package:mona_desktop/core/dto/message_request.dart';
import 'package:mona_desktop/features/service/chat/chat_service.dart';
import 'package:talker_flutter/talker_flutter.dart';

part 'chat_event.dart';
part 'chat_state.dart';

@lazySingleton
class ChatBloc extends Bloc<ChatEvent, ChatState> {
  ChatBloc(this.chatService, this.talker) : super(ChatInitial()) {
    on<OpenChat>((event, emit) async {
      try {
        emit(ChatOpened(
            chatId: event.chatId,
            chatName: event.chatName,
            receiverId: event.receiverId));

        if (event.chatId != null) {
          List<MessageDto> jsonResponse =
              await chatService.fetchMessagesByChatId(event.chatId!);
          // List<MessageDto> messages =
          //     jsonResponse.map((json) => MessageDto.fromJson(json as Map<String, dynamic>)).toList();

          // TODO(): Add loading until emitted
          emit(ChatLoaded(messages: jsonResponse));
        }
      } catch (e, st) {
        talker.handle(e, st);
      }
    });

    on<SendMessage>((event, emit) async {
      try {
        await chatService.sendMessage(event.messageRequest);
      } catch (e, st) {
        talker.handle(e, st);
      }
    });
  }

  final ChatService chatService;
  final Talker talker;
}
