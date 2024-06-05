import 'package:mona_desktop/core/dto/dto_export.dart';

abstract class AbstractAuthRepository {
  Future<TokenPairDto> signIn(SignInRequest signInRequest);
  Future<TokenPairDto> refresh(TokenPairDto tokenPairDto);
  Future<int> signUp(SignUpRequest signUpRequest);
}
