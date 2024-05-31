part of 'chat_bloc.dart';

@immutable
sealed class ChatState {}

final class ChatInitial extends ChatState {}

final class ChatOpened extends ChatState {
  final ChatDto chatDto;

  ChatOpened({required this.chatDto});
}
