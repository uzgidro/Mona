import 'package:flutter/material.dart';
import 'package:mona_desktop/core/di/injections.dart';
import 'package:mona_desktop/features/auth/bloc/auth_bloc.dart';
import 'package:mona_desktop/features/dialog/dialog_export.dart';

class DrawerComponent extends StatefulWidget {
  @override
  State<DrawerComponent> createState() => _DrawerComponentState();
}

class _DrawerComponentState extends State<DrawerComponent> {
  final authBloc = getIt<AuthBloc>();

  @override
  Widget build(BuildContext context) {
    return Drawer(
      child: Padding(
        padding: const EdgeInsets.all(12.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            TextButton(
              onPressed: () {
                showDialog<String>(
                    context: context,
                    builder: (BuildContext context) => DialogContactList());
                Scaffold.of(context).closeDrawer();
              },
              child: Padding(
                padding: const EdgeInsets.all(8.0),
                child: Row(children: [
                  Icon(Icons.account_circle_outlined),
                  SizedBox(width: 24),
                  Text('Контакты')
                ]),
              ),
            ),
            Divider(),
            TextButton(
              onPressed: () async {
                authBloc.add(SignOutEvent());
              },
              child: Padding(
                padding: const EdgeInsets.all(8.0),
                child: Row(children: [
                  Icon(Icons.logout_outlined),
                  SizedBox(width: 24),
                  Text('Выход')
                ]),
              ),
            ),
          ],
        ),
      ),
    );
  }
}
