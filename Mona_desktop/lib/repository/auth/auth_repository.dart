import 'package:dio/dio.dart';
import 'package:injectable/injectable.dart';
import 'package:mona_desktop/core/models/models_export.dart';
import 'package:mona_desktop/repository/auth/abstract_auth_repository.dart';
import 'package:mona_desktop/repository/repository_export.dart';

@Injectable(as: AbstractAuthRepository)
class AuthRepository implements AbstractAuthRepository {
  AuthRepository({required this.dio});

  final Dio dio;

  @override
  Future<SignInResponse> signIn(SignInRequest signInRequest) async {
    var response = await dio.post('http://127.0.0.1:5031/auth/sign-in',
        data: signInRequest.toJson());
    var loginResponse = SignInResponse.fromJson(response.data);
    return loginResponse;
  }

  @override
  Future<int> signUp(SignUpRequest signUpRequest) async {
    var response = await dio.post('http://127.0.0.1:5031/auth/sign-up',
        data: signUpRequest.toJson());

    return response.statusCode!;
  }
}
