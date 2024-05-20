import 'dart:async';

import 'package:flutter/foundation.dart';
import 'package:flutter/material.dart';
import 'package:mona_desktop/core/injections.dart';
import 'package:mona_desktop/features/mona.dart';
import 'package:talker_flutter/talker_flutter.dart';

void main() {
  // DI
  configureDependencies();
  // Gray Screen Error Handling
  FlutterError.onError = (details) => getIt<Talker>().handle(details);
  // Platform Error Handling
  PlatformDispatcher.instance.onError = (exception, stackTrace) {
    getIt<Talker>().handle(exception, stackTrace);
    return true;
  };
  // Run App With Error Handling
  runZonedGuarded(() => runApp(MonaApp()), (error, stack) {
    getIt<Talker>().handle(error, stack);
  });
}
