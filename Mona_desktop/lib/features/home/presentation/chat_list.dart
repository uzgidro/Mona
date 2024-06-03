import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:mona_desktop/core/di/injections.dart';
import 'package:mona_desktop/core/dto/chat_dto.dart';
import 'package:mona_desktop/features/service/service_export.dart';
import 'package:signalr_netcore/hub_connection.dart';

class ChatList extends StatefulWidget {
  @override
  State<ChatList> createState() => _ChatListState();
}

class _ChatListState extends State<ChatList> {
  final hubBloc = getIt<HubBloc>();
  final chatBloc = getIt<ChatBloc>();
  double _width = 240;
  double _minWidth = 140;
  final List<ChatDto> list = [];
  final hubConnection = getIt<HubConnection>();

  @override
  Widget build(BuildContext context) {
    return BlocListener(
      bloc: hubBloc,
      listener: (context, state) {
        if (state is HubStarted) {
          print(state.chatList);
          setState(() {
            list.addAll(state.chatList);
          });
        }
      },
      child: Container(
        decoration: BoxDecoration(
            border: Border(
          right: BorderSide(width: 0.25),
        )),
        child: Row(
          children: [
            SizedBox(
              width: _width,
              child: ListView.builder(
                itemCount: list.length,
                itemBuilder: (context, index) {
                  return ListTile(
                    title: Text(list[index].chatName),
                    subtitle: Text(list[index].message),
                    hoverColor: Colors.grey,
                    onTap: () async {
                      chatBloc.add(OpenChat(chatDto: list[index]));
                    },
                  );
                },
              ),
            ),
            SizedBox(
              height: MediaQuery.of(context).size.height,
              child: GestureDetector(
                onPanUpdate: (details) {
                  setState(() {
                    _width = (_width + details.delta.dx).clamp(
                        _minWidth, MediaQuery.of(context).size.width * 0.7);
                  });
                },
                child: MouseRegion(
                  cursor: SystemMouseCursors.resizeUpLeftDownRight,
                  child: SizedBox(
                    width: 4,
                  ),
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
