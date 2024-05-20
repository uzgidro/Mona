import 'package:dio/dio.dart';
import 'package:get_it/get_it.dart';
import 'package:mona_desktop/features/auth/bloc/auth_bloc.dart';
import 'package:mona_desktop/repository/repository_export.dart';

final getIt = GetIt.instance;

void configureDependencies() {
  getIt.registerLazySingleton<AbstractAuthRepository>(
      () => AuthRepository(dio: Dio()));

  getIt.registerLazySingleton(() => AuthBloc(getIt<AbstractAuthRepository>()));
}
