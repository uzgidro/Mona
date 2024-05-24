import 'package:mona_desktop/core/models/models_export.dart';
import 'package:mona_desktop/core/models/sign_in_request.dart';
import 'package:mona_desktop/core/models/sign_up_request.dart';

abstract class AbstractAuthRepository {
  Future<SignInResponse> signIn(SignInRequest signInRequest);
  Future<int> signUp(SignUpRequest signUpRequest);
}
