import 'package:mona_desktop/core/dto/dto_export.dart';

abstract class AbstractAuthRepository {
  Future<SignInResponse> signIn(SignInRequest signInRequest);
  Future<int> signUp(SignUpRequest signUpRequest);
}
