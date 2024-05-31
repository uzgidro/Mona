import 'package:bloc/bloc.dart';
import 'package:injectable/injectable.dart';
import 'package:meta/meta.dart';
import 'package:mona_desktop/core/dto/chat_dto.dart';

part 'chat_event.dart';
part 'chat_state.dart';

@LazySingleton()
class ChatBloc extends Bloc<ChatEvent, ChatState> {
  ChatBloc() : super(ChatInitial()) {
    on<OpenChat>((event, emit) {
      emit(ChatOpened(chatDto: event.chatDto));
    });
  }
}
