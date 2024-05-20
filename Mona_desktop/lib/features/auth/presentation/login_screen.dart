import 'package:flutter/material.dart';
import 'package:mona_desktop/core/injections.dart';
import 'package:mona_desktop/repository/repository_export.dart';

class LoginScreen extends StatelessWidget {

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: ElevatedButton(
        child: Text('login'),
        onPressed: () {
          getIt<AbstractAuthRepository>().login();
        },
      ),
    );
  }
}