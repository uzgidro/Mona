part of 'chat_bloc.dart';

@immutable
sealed class ChatEvent extends Equatable {}

final class OpenChat extends ChatEvent {
  final ChatDto chatDto;

  OpenChat({required this.chatDto});

  @override
  List<Object?> get props => [chatDto];
}