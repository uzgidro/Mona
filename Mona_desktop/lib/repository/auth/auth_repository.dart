import 'package:dio/dio.dart';
import 'package:mona_desktop/core/models/models_export.dart';
import 'package:mona_desktop/repository/auth/abstract_auth_repository.dart';

class AuthRepository implements AbstractAuthRepository {
  AuthRepository({required this.dio});

  final Dio dio;

  @override
  Future<LoginResponse> login(String username, String password) async {
    var response = await dio.post('http://127.0.0.1:5031/auth/sign-in',
        data: {'username': username, 'password': password});
    var loginResponse = LoginResponse.fromJson(response.data);
    return loginResponse;
  }
}
