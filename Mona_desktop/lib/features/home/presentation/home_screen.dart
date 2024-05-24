import 'package:flutter/material.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:go_router/go_router.dart';
import 'package:mona_desktop/core/constants.dart';
import 'package:mona_desktop/core/di/injections.dart';

class HomeScreen extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
   return Scaffold(
     body: ElevatedButton(
       onPressed: () async {
         await getIt<FlutterSecureStorage>().delete(key: accessToken);
         await getIt<FlutterSecureStorage>().delete(key: refreshToken);
         context.go('/');
       },
       child: Text('Выход'),
     ),
   );
  }

}