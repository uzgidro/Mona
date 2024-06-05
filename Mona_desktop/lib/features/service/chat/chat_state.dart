part of 'chat_bloc.dart';

@immutable
sealed class ChatState extends Equatable {}

final class ChatInitial extends ChatState {
  @override
  List<Object?> get props => [];
}

final class ChatOpened extends ChatState {
  final ChatDto chatDto;

  ChatOpened({required this.chatDto});

  @override
  List<Object?> get props => [chatDto];
}
