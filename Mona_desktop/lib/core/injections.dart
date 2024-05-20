import 'package:dio/dio.dart';
import 'package:flutter_bloc/flutter_bloc.dart';
import 'package:get_it/get_it.dart';
import 'package:mona_desktop/features/auth/bloc/auth_bloc.dart';
import 'package:mona_desktop/repository/repository_export.dart';
import 'package:talker_bloc_logger/talker_bloc_logger.dart';
import 'package:talker_dio_logger/talker_dio_logger.dart';
import 'package:talker_flutter/talker_flutter.dart';

final getIt = GetIt.instance;

void configureDependencies() {
  // Talker (Logger)
  getIt.registerLazySingleton(() => TalkerFlutter.init());

  Bloc.observer = TalkerBlocObserver(talker: getIt<Talker>());

  final dio = Dio();
  dio.interceptors.add(
    TalkerDioLogger(
      talker: getIt<Talker>(),
      settings: const TalkerDioLoggerSettings(
        printRequestHeaders: true,
        printResponseHeaders: true,
      ),
    ),
  );
  // Auth repository
  getIt.registerLazySingleton<AbstractAuthRepository>(
      () => AuthRepository(dio: dio));

  // Auth BLoC
  getIt.registerLazySingleton(() => AuthBloc(getIt<AbstractAuthRepository>()));

}
