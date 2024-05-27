import 'dart:async';

import 'package:flutter_secure_storage/flutter_secure_storage.dart';
import 'package:injectable/injectable.dart';
import 'package:mona_desktop/core/di/injections.dart';
import 'package:mona_desktop/core/dto/dto_export.dart';
import 'package:talker_flutter/talker_flutter.dart';

const String accessToken = 'accessToken';
const String refreshToken = 'refreshToken';

@Injectable()
class JwtService {
  final FlutterSecureStorage storage;

  JwtService({required this.storage});

  Future<String> getAccessToken() async {
    try {
      return await storage.read(key: accessToken) as String;
    } catch (e, st) {
      getIt<Talker>().handle(e, st);
      return '';
    }
  }

  Future<String> getRefreshToken() async {
    try {
      return await storage.read(key: refreshToken) as String;
    } catch (e, st) {
      getIt<Talker>().handle(e, st);
      return '';
    }
  }

  Future<bool> saveTokens(SignInResponse response) async {
    try {
      await storage.write(key: accessToken, value: response.accessToken);
      await storage.write(key: refreshToken, value: response.refreshToken);
      return true;
    } catch (e, st) {
      getIt<Talker>().handle(e, st);
      return false;
    }
  }

  Future<bool> removeTokens() async {
    try {
      await storage.delete(key: accessToken);
      await storage.delete(key: refreshToken);
      return true;
    } catch (e, st) {
      getIt<Talker>().handle(e, st);
      return false;
    }
  }

  Future<bool> isAuthenticated() async {
    return await storage.read(key: accessToken) != null;
  }
}
