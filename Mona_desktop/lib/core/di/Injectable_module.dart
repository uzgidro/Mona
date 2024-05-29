import 'package:dio/dio.dart';
import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:injectable/injectable.dart';
import 'package:mona_desktop/core/middleware/jwt_service.dart';
import 'package:mona_desktop/core/middleware/middleware.dart';
import 'package:signalr_netcore/signalr_client.dart';
import 'package:talker_dio_logger/talker_dio_logger.dart';
import 'package:talker_flutter/talker_flutter.dart';

@module
abstract class InjectableModule {
  @lazySingleton
  Talker get talker => TalkerFlutter.init();

  @lazySingleton
  Dio provideDio(Talker talker, JwtService service) {
    final dio = Dio();
    dio.interceptors.add(InterceptorsWrapper(
      onRequest: (options, handler) async {
        var accessToken = await service.getAccessToken();
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
  HubConnection hubConnection(JwtService service) => HubConnectionBuilder()
      .withUrl("http://127.0.0.1:5031/chat",
          options: HttpConnectionOptions(
              accessTokenFactory: () async => await service.getAccessToken()))
      .build();

  @lazySingleton
  FlutterSecureStorage get secureStorage => const FlutterSecureStorage();
}
