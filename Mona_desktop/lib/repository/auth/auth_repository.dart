import 'package:dio/dio.dart';
import 'package:mona_desktop/core/models/models_export.dart';
import 'package:mona_desktop/repository/auth/abstract_auth_repository.dart';

class AuthRepository implements AbstractAuthRepository {
  AuthRepository({required this.dio});

  final Dio dio;

  @override
  Future<LoginResponse> login() async {

    var response = await dio.post('http://127.0.0.1:5031/auth/sign-in',
        data: {'username': 'Abbos', 'password': 'asdasd'});
    var loginResponse = LoginResponse.fromJson(response.data);
    return loginResponse;
  }
}
