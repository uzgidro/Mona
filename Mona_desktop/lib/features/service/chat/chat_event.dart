part of 'chat_bloc.dart';

@immutable
sealed class ChatEvent {}

final class OpenChat extends ChatEvent {
  final ChatDto chatDto;

  OpenChat({required this.chatDto});
}
