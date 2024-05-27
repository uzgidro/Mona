import 'package:dio/dio.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:injectable/injectable.dart';
import 'package:mona_desktop/core/di/injections.dart';
import 'package:mona_desktop/core/middleware/jwt_service.dart';
import 'package:talker_dio_logger/talker_dio_logger.dart';
import 'package:talker_flutter/talker_flutter.dart';

@module
abstract class InjectableModule {
  @lazySingleton
  Talker get talker => TalkerFlutter.init();

  @lazySingleton
  Dio provideDio(Talker talker) {
    final dio = Dio();
    dio.interceptors.add(InterceptorsWrapper(
      onRequest: (options, handler) async {
        var accessToken = await getIt<JwtService>().getAccessToken();
        options.headers["Authorization"] = "Bearer $accessToken";
        return handler.next(options);
      },
    ));
    dio.interceptors.add(
      TalkerDioLogger(
        talker: talker,
        settings: const TalkerDioLoggerSettings(
          printRequestHeaders: true,
          printResponseHeaders: true,
        ),
      ),
    );
    return dio;
  }

  @lazySingleton
  FlutterSecureStorage get secureStorage => const FlutterSecureStorage();
}
