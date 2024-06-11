import 'package:bloc/bloc.dart';
import 'package:equatable/equatable.dart';
import 'package:injectable/injectable.dart';
import 'package:meta/meta.dart';
import 'package:mona_desktop/core/dto/message_dto.dart';
import 'package:mona_desktop/features/service/chat/chat_service.dart';
import 'package:talker_flutter/talker_flutter.dart';

part 'chat_event.dart';
part 'chat_state.dart';

@Injectable()
class ChatBloc extends Bloc<ChatEvent, ChatState> {
  ChatBloc(this.chatService, this.talker) : super(ChatInitial()) {
    on<OpenChat>((event, emit) async {
      try {
        emit(ChatOpened(
            chatId: event.chatId,
            chatName: event.chatName,
            receiverId: event.receiverId));

        if (event.chatId != null) {
          List<dynamic> jsonResponse = await chatService
              .fetchMessagesByChatId(event.chatId!) as List<dynamic>;
          List<MessageDto> messages =
              jsonResponse.map((json) => MessageDto.fromJson(json)).toList();

          // TODO(): Add loading until emitted
          emit(ChatLoaded(messages: messages));
        }
      } catch (e, st) {
        talker.handle(e, st);
      }
    });
  }

  final ChatService chatService;
  final Talker talker;
}
