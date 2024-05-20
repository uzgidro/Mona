import 'package:mona_desktop/core/models/models_export.dart';

abstract class AbstractAuthRepository {
  Future<LoginResponse> login();
}