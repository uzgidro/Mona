import 'package:flutter/material.dart';
import 'package:mona_desktop/core/routes/router.dart';

class MonaApp extends StatelessWidget {
  const MonaApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp.router(
      title: 'Mona',
      theme: ThemeData(
        useMaterial3: true,
        colorScheme: ColorScheme.fromSeed(seedColor: Colors.blueAccent),
      ),
      routerConfig: router,
    );
  }
}