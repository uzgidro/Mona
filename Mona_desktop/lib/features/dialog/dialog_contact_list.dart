import 'package:flutter/material.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:mona_desktop/core/di/injections.dart';
import 'package:mona_desktop/core/dto/dto_export.dart';
import 'package:mona_desktop/features/service/service_export.dart';

class DialogContactList extends StatefulWidget {
  @override
  State<DialogContactList> createState() => _DialogContactListState();
}

class _DialogContactListState extends State<DialogContactList> {
  final hubBloc = getIt<HubBloc>();
  List<UserDto> users = [];

  @override
  void initState() {
    super.initState();
    hubBloc.add(LoadContacts());
  }

  @override
  Widget build(BuildContext context) {
    return BlocListener<HubBloc, HubState>(
      bloc: hubBloc,
      listener: (context, state) {
        if (state is ContactsLoaded) {
          setState(() {
            users.addAll(state.contacts);
          });
        }
      },
      child: Dialog(
        child: Padding(
          padding: const EdgeInsets.all(8.0),
          child: Column(
            mainAxisSize: MainAxisSize.min,
            mainAxisAlignment: MainAxisAlignment.center,
            children: [
              const Text('Контакты'),
              const SizedBox(height: 15),
              SizedBox(
                height: 300,
                width: 350,
                child: ListView.builder(
                    itemCount: users.length,
                    itemBuilder: (context, index) {
                      return ListTile(title: Text(users[index].name));
                    }),
              ),
              TextButton(
                onPressed: () {
                  Navigator.pop(context);
                },
                child: const Text('Закрыть'),
              ),
            ],
          ),
        ),
      ),
    );
  }
}
