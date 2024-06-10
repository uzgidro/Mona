part of 'chat_bloc.dart';

@immutable
sealed class ChatEvent extends Equatable {}

final class OpenChat extends ChatEvent {
  final String chatName;
  final String receiverId;
  final String? chatId;

  OpenChat(
      {required this.chatName, required this.receiverId, required this.chatId});

  @override
  List<Object?> get props => [receiverId, chatId, chatName];
}
