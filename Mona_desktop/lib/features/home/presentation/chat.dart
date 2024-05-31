import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:mona_desktop/core/di/injections.dart';
import 'package:mona_desktop/core/dto/message_dto.dart';
import 'package:mona_desktop/features/service/service_export.dart';

class Chat extends StatefulWidget {
  @override
  State<Chat> createState() => _ChatState();
}

class _ChatState extends State<Chat> {
  final chatBloc = getIt<ChatBloc>();
  final hubBloc = getIt<HubBloc>();
  var chatTitle = '';
  List<MessageDto> messages = [];

  @override
  Widget build(BuildContext context) {
    return MultiBlocListener(
      listeners: [
        BlocListener(
            bloc: chatBloc,
            listener: (context, state) {
              if (state is ChatOpened) {
                setState(() {
                  chatTitle = state.chatDto.chatName;
                });
                hubBloc.add(GetChatMessages(chatId: state.chatDto.chatId));
              }
            }),
        BlocListener(
          bloc: hubBloc,
          listener: (context, state) {
            if (state is ChatLoaded) {
              setState(() {
                messages = state.messages;
              });
            }
          },
        )
      ],
      child: Expanded(
          child: Column(
        children: [
          // top bar
          Container(
            decoration:
                BoxDecoration(border: Border(bottom: BorderSide(width: 0.25))),
            child: Padding(
              padding: const EdgeInsets.all(12.0),
              child: Row(
                mainAxisAlignment: MainAxisAlignment.spaceBetween,
                children: [
                  Text(
                    chatTitle,
                    style: TextStyle(fontWeight: FontWeight.w600, fontSize: 16),
                  ),
                  Container(
                    color: Theme.of(context).colorScheme.tertiary,
                    height: 36,
                    width: 36,
                  )
                ],
              ),
            ),
          ),
          // messages
          Expanded(
              child: Container(
            color: Theme.of(context).colorScheme.tertiary,
            child: ListView.builder(
              itemCount: messages.length,
              itemBuilder: (context, index) {
                return Padding(
                  padding: const EdgeInsets.all(8.0),
                  child: Container(
                    padding:
                        EdgeInsets.only(bottom: 8, top: 8, left: 16, right: 16),
                    decoration: BoxDecoration(
                        color: Theme.of(context).colorScheme.background,
                        borderRadius: BorderRadius.circular(50)),
                    child: Text(messages[index].senderName),
                  ),
                );
              },
            ),
          )),
        ],
      )),
    );
  }
}
